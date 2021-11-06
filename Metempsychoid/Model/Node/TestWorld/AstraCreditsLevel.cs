using Astrategia.Model.Event;
using Astrategia.Model.Layer.BackgroundLayer;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Node.TestWorld
{
    public class AstraCreditsLevel : AstraRulesLevel
    {
        public AstraCreditsLevel(World world) 
            : base(world)
        {

        }

        public override void VisitStart(World world)
        {
            this.NodeState = NodeState.ACTIVE;
            this.pendingGameEvents = new Dictionary<EventType, List<GameEvent>>();

            this.backgroundLayer = world.LoadedLayers["slidesLayer"] as ImageBackgroundLayer;

            this.currentRulesPhase = RulesPhase.SLIDE0;
            this.backgroundLayer.PreInitImageId = this.phaseToImageIds[this.currentRulesPhase];

            world.InitializeLevel(new List<string>()
            {
                "slidesLayer"
            }, this);
        }

        protected override void InternalUpdateLogic(World world, Time timeElapsed)
        {
            //int indexNextRulePhase = ((int)this.CurrentRulesPhase) + 1;

            //if (indexNextRulePhase < 1)
            //{
            //    RulesPhase nextRulePhase = (RulesPhase)indexNextRulePhase;

            //    if (this.CheckNextRulesPhaseEvent())
            //    {
            //        this.CurrentRulesPhase = nextRulePhase;
            //    }
            //}
            if(this.CheckNextRulesPhaseEvent())
            {
                world.NotifyInternalGameEvent(new InternalGameEvent(InternalEventType.GO_TO_LEVEL, "StartPageLevel"));
            }
        }
    }
}
