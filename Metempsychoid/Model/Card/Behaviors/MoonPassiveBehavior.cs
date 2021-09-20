using Metempsychoid.Model.Constellations;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardGameLayer.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Card.Behaviors
{
    public class MoonPassiveBehavior : ACardBehavior
    {
        private ConstellationPattern patternToMatch; 

        public int Value
        {
            get;
            private set;
        }

        public MoonPassiveBehavior(int value, ConstellationPattern patternToMatch)
        {
            this.Value = value;

            this.patternToMatch = patternToMatch;
        }

        public override void OnActionsOccured(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionsOccured)
        {
            if (starEntity.CardSocketed.Card.IsAwakened)
            {
                if (actionsOccured.Any(pElem => pElem is IModifyStarEntityAction))
                {
                    this.UpdateValue(layer, starEntity);
                }
            }
        }

        public override void OnAwakened(BoardGameLayer layer, StarEntity starEntity)
        {
            this.UpdateValue(layer, starEntity);
        }

        private void UpdateValue(BoardGameLayer layer, StarEntity starEntity)
        {
            List<StarEntity> allyStarEntities = layer.StarSystem.Where(pElem => pElem.CardSocketed != null && pElem.CardSocketed.Card.CurrentOwner == starEntity.CardSocketed.Card.CurrentOwner).ToList();

            int bonus = 0;
            Dictionary<ConstellationNode, StarEntity> nodeToStarEntity = new Dictionary<ConstellationNode, StarEntity>();
            Dictionary<ConstellationLink, List<StarLinkEntity>> linkToStarLinkEntity = new Dictionary<ConstellationLink, List<StarLinkEntity>>();
            while (allyStarEntities.Count != 0)
            {
                StarEntity currentStarEntity = allyStarEntities.ElementAt(0);
                allyStarEntities.RemoveAt(0);

                if(this.patternToMatch.CreateConstellationSystem(layer, currentStarEntity, nodeToStarEntity, linkToStarLinkEntity))
                {
                    allyStarEntities = allyStarEntities.Except(nodeToStarEntity.Values).ToList();
                    bonus++;
                }
            }

            bonus *= this.Value;

            bool mustSetValue = starEntity.CardSocketed.Card.BehaviorToValueModifier.TryGetValue(this, out int currentValue) == false || currentValue != bonus;

            if (mustSetValue)
            {
                layer.PendingActions.Add(new SetCardValueModifier(starEntity.CardSocketed, this, bonus));
            }
        }

        public override void OnUnawakened(BoardGameLayer layer, CardEntity ownerCardEntity)
        {
            layer.PendingActions.Add(new ClearCardValueModifier(ownerCardEntity, this));
        }

        public override ICardBehavior Clone()
        {
            return new MoonPassiveBehavior(this.Value, this.patternToMatch);
        }
    }
}
