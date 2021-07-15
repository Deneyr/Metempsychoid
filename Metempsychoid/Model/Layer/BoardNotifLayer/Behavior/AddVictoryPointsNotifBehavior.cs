using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model.Card;

namespace Metempsychoid.Model.Layer.BoardNotifLayer.Behavior
{
    public class AddVictoryPointsNotifBehavior : ABoardNotifBehavior
    {
        public override bool IsThereEndButton
        {
            get
            {
                return true;
            }
        }

        public int NbVictoryPointsToAdd
        {
            get;
            private set;
        }

        public AddVictoryPointsNotifBehavior(CardEntity ownerCardEntity, int nbVictoryPointsToAdd) : base(ownerCardEntity)
        {
            this.NbVictoryPointsToAdd = nbVictoryPointsToAdd;
        }

        public override void EndNotif(World world)
        {
            
        }

        public override void StartNotif(World world)
        {
            this.NodeLevel.BoardBannerLayer.AddVictoryPointsTo(this.OwnerCardEntity.Card.CurrentOwner.PlayerName, this.NbVictoryPointsToAdd);
        }

        public override bool UpdateNotif(World world)
        {
            return false;
        }
    }
}
