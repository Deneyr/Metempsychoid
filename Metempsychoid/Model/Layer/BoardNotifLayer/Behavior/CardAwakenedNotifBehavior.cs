using Metempsychoid.Model.Card;
using Metempsychoid.Model.Node.TestWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardNotifLayer.Behavior
{
    public class CardAwakenedNotifBehavior : ABoardNotifBehavior
    {
        public int ValueBeforeAwakened
        {
            get;
            private set;
        }

        public CardAwakenedNotifBehavior(CardEntity cardEntityAwakened)
            : base(cardEntityAwakened)
        {
            this.ValueBeforeAwakened = cardEntityAwakened.CardValue;
        }

        public override void EndNotif(World world)
        {
            
        }

        public override void StartNotif(World world)
        {
            this.NodeLevel.BoardNotifLayer.NotifyCardAwakened(this.OwnerCardEntity, this.ValueBeforeAwakened);
        }

        public override bool UpdateNotif(World world)
        {
            return this.IsActive;
        }
    }
}
