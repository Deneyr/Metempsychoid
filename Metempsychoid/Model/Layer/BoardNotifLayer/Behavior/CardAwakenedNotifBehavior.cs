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
        public CardEntity CardEntityAwakened
        {
            get;
            private set;
        }

        public CardAwakenedNotifBehavior(TestLevel level, CardEntity cardEntityAwakened):
            base(level)
        {
            this.CardEntityAwakened = cardEntityAwakened;
        }

        public override void EndNotif(World world)
        {
            
        }

        public override void StartNotif(World world)
        {
            this.level.BoardNotifLayer.NotifyCardAwakened(this.CardEntityAwakened);
        }

        public override bool UpdateNotif(World world)
        {
            return this.IsActive;
        }
    }
}
