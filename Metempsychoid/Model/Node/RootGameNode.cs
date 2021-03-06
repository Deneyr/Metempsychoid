using Metempsychoid.Model.Event;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Node
{
    public class RootGameNode : ANode
    {
        private Dictionary<string, AWorldNode> nameToWorldNodes;
        private string nextWorldNodeName;
        private AWorldNode currentWorldNode;

        public RootGameNode(World world) :
            base(world)
        {
            this.nameToWorldNodes = new Dictionary<string, AWorldNode>();
            this.nextWorldNodeName = null;
            this.currentWorldNode = null;

            this.nameToWorldNodes.Add("TestWorld", new TestWorld.TestWorld(world));
        }

        public override void VisitStart(World world)
        {
            base.VisitStart(world);

            this.nextWorldNodeName = "TestWorld";
            this.UpdateCurrentWorldNode(world);
        }

        public override void UpdateLogic(World world, Time timeElapsed)
        {
            if (this.currentWorldNode != null)
            {
                this.currentWorldNode.UpdateLogic(world, timeElapsed);
            }

            this.UpdateCurrentWorldNode(world);
        }

        private void UpdateCurrentWorldNode(World world)
        {
            if (string.IsNullOrEmpty(this.nextWorldNodeName) == false)
            {
                if (this.currentWorldNode != null)
                {
                    this.currentWorldNode.VisitEnd(world);
                }

                if (this.nameToWorldNodes.ContainsKey(this.nextWorldNodeName))
                {
                    this.currentWorldNode = this.nameToWorldNodes[this.nextWorldNodeName];
                    this.currentWorldNode.VisitStart(world);
                }

                this.nextWorldNodeName = null;
            }
        }

        public override void OnInternalGameEvent(World world, InternalGameEvent internalGameEvent)
        {
            if (internalGameEvent.Type == InternalEventType.GO_TO_WORLD)
            {
                this.nextWorldNodeName = internalGameEvent.Details;
            }
        }

        public override void OnGameEvent(World world, GameEvent gameEvent)
        {
            if (this.currentWorldNode != null)
            {
                this.currentWorldNode.OnGameEvent(world, gameEvent);
            }
        }
    }
}
