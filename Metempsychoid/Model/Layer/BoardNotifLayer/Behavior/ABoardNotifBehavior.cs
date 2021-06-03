using Metempsychoid.Model.Node.TestWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardNotifLayer.Behavior
{
    public abstract class ABoardNotifBehavior : IBoardNotifBehavior
    {
        protected TestLevel level;

        public ABoardNotifBehavior(TestLevel level)
        {
            this.level = level;
        }

        public abstract void EndNotif(World world);

        public abstract void StartNotif(World world);

        public abstract bool UpdateNotif(World world);
    }
}
