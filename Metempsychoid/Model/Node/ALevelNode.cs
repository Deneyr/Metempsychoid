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
        public ALevelNode(World world) : 
            base(world)
        {
        }

        public override void VisitEnd(World world)
        {
            base.VisitEnd(world);

            world.EndLevel();
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
            foreach(ALayer layer in world.CurrentLayers)
            {
                layer.OnGameEvent(world, gameEvent);
            }
        }
    }
}
