using Metempsychoid.Model.Event;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Node
{
    public abstract class AWorldNode: ANode
    {
        protected Dictionary<string, ALevelNode> nameTolevelNodes;

        protected ALevelNode currentLevelNode;

        protected string nextLevelNodeName;

        public AWorldNode()
        {
            this.nameTolevelNodes = new Dictionary<string, ALevelNode>();

            this.currentLevelNode = null;
            this.nextLevelNodeName = null;
        }

        public override void UpdateLogic(World world, Time timeElapsed)
        {
            if(this.currentLevelNode != null)
            {
                this.currentLevelNode.UpdateLogic(world, timeElapsed);
            }

            this.UpdateCurrentLevelNode(world);
        }

        public void UpdateCurrentLevelNode(World world)
        {
            if (string.IsNullOrEmpty(this.nextLevelNodeName) == false)
            {
                if (this.currentLevelNode != null)
                {
                    this.currentLevelNode.VisitEnd(world);
                }

                if (this.nameTolevelNodes.ContainsKey(this.nextLevelNodeName))
                {
                    this.currentLevelNode = this.nameTolevelNodes[this.nextLevelNodeName];
                    this.currentLevelNode.VisitStart(world);
                }

                this.nextLevelNodeName = null;
            }
        }

        public override void VisitEnd(World world)
        {
            base.VisitEnd(world);

            world.EndWorld();
        }

        public override void OnInternalGameEvent(World world, InternalGameEvent internalGameEvent)
        {
            if(internalGameEvent.Type == InternalEventType.GO_TO_LEVEL)
            {
                this.nextLevelNodeName = internalGameEvent.Details;
            }
        }
    }
}
