using Metempsychoid.Model.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Node
{
    public abstract class ALevelNode: ANode
    {
        protected List<ALayer> layers;

        public ALevelNode()
        {
            this.layers = new List<ALayer>();
        }


        public override void VisitEnd(World world)
        {
            base.VisitEnd(world);

            world.EndLevel();
        }

        public override void OnInternalGameEvent(World world, InternalGameEvent internalGameEvent)
        {

        }
    }
}
