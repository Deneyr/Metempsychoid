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
    public class EmperorActiveBehavior : ACardBehavior
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
            layer.RegisterNotifBehavior(new AddVictoryPointsNotifBehavior(starEntity.CardSocketed, this.NbPoints));
        }

        public override ICardBehavior Clone()
        {
            return new EmperorActiveBehavior(this.NbPoints);
        }
    }
}