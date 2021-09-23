using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardGameLayer.Actions;
using Metempsychoid.Model.Layer.BoardNotifLayer.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Card.Behaviors
{
    public class JudgementActiveBehavior : ACardActiveBehavior
    {
        public override void OnAwakened(BoardGameLayer layer, StarEntity starEntity)
        {
            this.ActivateBehaviorEffect(layer, starEntity, null);
        }

        protected override bool ActivateBehaviorEffect(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionsOccured)
        {
            if (base.ActivateBehaviorEffect(layer, starEntity, actionsOccured))
            {
                layer.RegisterNotifBehavior(new DeleteCardNotifBehavior(this, starEntity.CardSocketed));

                return true;
            }
            return false;
        }

        public override void OnBehaviorStart(ACardNotifBehavior behavior)
        {
            behavior.NbBehaviorUse = 1;

            List<StarEntity> starsSortedByPower = behavior.NodeLevel.BoardGameLayer.StarSystem
                .Where(pElem => pElem.CardSocketed != null)
                .OrderByDescending(pElem => pElem.CardSocketed.CardValue).ToList();

            if (starsSortedByPower.Count > 0)
            {
                int maxValue = starsSortedByPower.First().CardSocketed.CardValue;
                List<StarEntity> mostPowerStars = new List<StarEntity>();

                foreach(StarEntity star in starsSortedByPower)
                {
                    if(star.CardSocketed.CardValue == maxValue)
                    {
                        mostPowerStars.Add(star);
                    }
                    else
                    {
                        break;
                    }
                }

                (behavior as DeleteCardNotifBehavior).FromStarEntities = mostPowerStars;
            }
        }

        public override ICardBehavior Clone()
        {
            JudgementActiveBehavior clone = new JudgementActiveBehavior();

            clone.InitFrom(this);

            return clone;
        }
    }
}
