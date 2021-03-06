using Metempsychoid.Model.Event;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Node
{
    public class ANode: IGameEventListener
    {
        protected WeakReference<World> world;

        public virtual ANode NextNode
        {
            get;
            set;
        }

        public NodeState NodeState
        {
            get;
            set;
        }

        public ANode(World world)
        {
            this.NextNode = null;

            this.NodeState = NodeState.NOT_ACTIVE;

            this.world = new WeakReference<World>(world);
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

        public virtual void OnInternalGameEvent(World world, InternalGameEvent internalGameEvent)
        {

        }

        public virtual void OnGameEvent(World world, GameEvent gameEvent)
        {

        }
    }

    public enum NodeState
    {
        NOT_ACTIVE,
        ACTIVE
    }
}
