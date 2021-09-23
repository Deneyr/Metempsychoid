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
    public class EmperorActiveBehavior : ACardActiveBehavior
    {
        public int NbPoints
        {
            get;
            private set;
        }

        public EmperorActiveBehavior(int nbPoints)
        {
            this.NbPoints = nbPoints;
        }

        public override void OnAwakened(BoardGameLayer layer, StarEntity starEntity)
        {
            this.ActivateBehaviorEffect(layer, starEntity, null);
        }

        protected override bool ActivateBehaviorEffect(BoardGameLayer layer, StarEntity starEntity, List<IBoardGameAction> actionsOccured)
        {
            if (base.ActivateBehaviorEffect(layer, starEntity, actionsOccured))
            {
                layer.RegisterNotifBehavior(new AddVictoryPointsNotifBehavior(starEntity.CardSocketed, this.NbPoints));

                return true;
            }
            return false;
        }

        public override ICardBehavior Clone()
        {
            EmperorActiveBehavior clone = new EmperorActiveBehavior(this.NbPoints);

            clone.InitFrom(this);

            return clone;
        }
    }
}