using Astrategia.Model.Event;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Node.TestWorld
{
    public class CJMenuLevel : ALevelNode
    {
        public CJMenuLevel(World world) : base(world)
        {
        }

        public override void VisitStart(World world)
        {
            base.VisitStart(world);

            world.InitializeLevel(new List<string>()
            {
                "startPage"
            }, this);
        }

        protected override void InternalUpdateLogic(World world, Time timeElapsed)
        {
            if (this.CheckLevelChangeEvent(world, out string detailsBoardFocused))
            {
                world.NotifyInternalGameEvent(new InternalGameEvent(InternalEventType.GO_TO_LEVEL, detailsBoardFocused));
            }
        }

        private bool CheckLevelChangeEvent(World world, out string details)
        {
            details = null;

            if (this.pendingGameEvents.TryGetValue(EventType.LEVEL_CHANGE, out List<GameEvent> gameEventsList))
            {
                foreach (GameEvent gameEvent in gameEventsList)
                {
                    details = gameEvent.Details;

                    return true;
                }
            }
            return false;
        }

    }
}
