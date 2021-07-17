using Metempsychoid.Model.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardNotifLayer.Behavior
{
    public class SecureDomainNotifBehavior : ABoardNotifBehavior
    {
        public override bool IsThereEndButton
        {
            get
            {
                return true;
            }
        }

        public SecureDomainNotifBehavior(CardEntity ownerCardEntity) : base(ownerCardEntity)
        {

        }

        public override void EndNotif(World world)
        {

        }

        public override void StartNotif(World world)
        {
            this.NodeLevel.BoardGameLayer.SecureDomains(this.OwnerCardEntity.ParentStar);
        }

        public override bool UpdateNotif(World world)
        {
            return false;
        }
    }
}
