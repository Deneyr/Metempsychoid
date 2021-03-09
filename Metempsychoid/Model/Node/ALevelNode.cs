using Metempsychoid.Model.Event;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Node
{
    public abstract class ALevelNode: ANode
    {
        protected List<GameEvent> pendingGameEvents;

        public ALevelNode(World world) : 
            base(world)
        {
        }

        public override void VisitStart(World world)
        {
            base.VisitStart(world);

            this.pendingGameEvents = new List<GameEvent>();
        }

        public override void VisitEnd(World world)
        {
            base.VisitEnd(world);

            world.EndLevel();
        }

        public override void UpdateLogic(World world, Time timeElapsed)
        {
            base.UpdateLogic(world, timeElapsed);

            this.InternalUpdateLogic(world, timeElapsed);

            this.pendingGameEvents.Clear();
        }

        protected virtual void InternalUpdateLogic(World world, Time timeElapsed)
        {
            // To Override.
        }

        protected void NotifyLevelStateChanged(World world, string state)
        {
            foreach(ALayer layer in world.CurrentLayers)
            {
                layer.NotifyLevelStateChanged(state);
            }
        }

        public override void OnInternalGameEvent(World world, InternalGameEvent internalGameEvent)
        {

        }

        public override void OnGameEvent(World world, GameEvent gameEvent)
        {
            this.pendingGameEvents.Add(gameEvent);
        }
    }
}
