using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Node
{
    public class ANode
    {
        public ANode NextNode
        {
            get;
            set;
        }

        public NodeState NodeState
        {
            get;
            set;
        }

        public ANode()
        {
            this.NextNode = null;

            this.NodeState = NodeState.NOT_ACTIVE;
        }

        public virtual void VisitStart(World world)
        {
            this.NodeState = NodeState.ACTIVE;

            //world.InternalGameEvent += this.OnInternalGameEvent;
        }

        public virtual void VisitEnd(World world)
        {
            //world.InternalGameEvent -= this.OnInternalGameEvent;
        }

        public virtual void UpdateLogic(World world, Time timeElapsed)
        {

        }

        protected virtual void OnInternalGameEvent(World world, AObject lObject, AObject lObjectTo, string details)
        {
            
        }

    }

    public enum NodeState
    {
        NOT_ACTIVE,
        ACTIVE
    }
}
