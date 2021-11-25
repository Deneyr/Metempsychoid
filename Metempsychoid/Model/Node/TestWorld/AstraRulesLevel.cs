using Astrategia.Model.Event;
using Astrategia.Model.Layer.BackgroundLayer;
using Astrategia.Model.Layer.BoardPlayerLayer;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Node.TestWorld
{
    public class AstraRulesLevel : ALevelNode
    {
        protected ImageBackgroundLayer backgroundLayer;

        protected Dictionary<RulesPhase, string> phaseToImageIds;

        protected RulesPhase currentRulesPhase;

        public RulesPhase CurrentRulesPhase
        {
            get
            {
                return this.currentRulesPhase;
            }
            protected set
            {
                if(this.currentRulesPhase != value)
                {
                    this.currentRulesPhase = value;

                    this.backgroundLayer.CurrentImageId = this.phaseToImageIds[this.currentRulesPhase];
                }
            }
        }

        public AstraRulesLevel(World world) 
            : base(world)
        {
            this.currentRulesPhase = RulesPhase.VOID;

            this.phaseToImageIds = new Dictionary<RulesPhase, string>()
            {
                {RulesPhase.SLIDE0, "slide_credits1" },
                {RulesPhase.SLIDE1, "slide_rules1" },
                {RulesPhase.SLIDE2, "slide_rules2" },
                {RulesPhase.SLIDE3, "slide_rules3" },
                {RulesPhase.SLIDE4, "slide_rules4" }
            };
        }

        public override void VisitStart(World world)
        {
            base.VisitStart(world);

            this.backgroundLayer = world.LoadedLayers["slidesLayer"] as ImageBackgroundLayer;

            this.currentRulesPhase = RulesPhase.SLIDE1;
            this.backgroundLayer.PreInitImageId = this.phaseToImageIds[this.currentRulesPhase];

            world.InitializeLevel(new List<string>()
            {
                "slidesLayer"
            }, this);
        }

        protected override void InternalUpdateLogic(World world, Time timeElapsed)
        {
            if (this.CheckNextRulesPhaseEvent())
            {
                int indexNextRulePhase = ((int)this.CurrentRulesPhase) + 1;

                if (indexNextRulePhase < 5)
                {
                    RulesPhase nextRulePhase = (RulesPhase)indexNextRulePhase;

                    this.CurrentRulesPhase = nextRulePhase;
                }
                else
                {
                    world.NotifyInternalGameEvent(new InternalGameEvent(InternalEventType.GO_TO_LEVEL, "StartPageLevel"));
                }
            }
        }

        protected bool CheckNextRulesPhaseEvent()
        {
            //if (this.pendingGameEvents.TryGetValue(EventType.LEVEL_PHASE_CHANGE, out List<GameEvent> gameEventsList))
            //{
            //    return true;
            //}
            //return false;

            return this.pendingGameEvents.ContainsKey(EventType.LEVEL_PHASE_CHANGE);
        }

        public enum RulesPhase
        {
            VOID = -1,
            SLIDE0 = 0,
            SLIDE1 = 1,
            SLIDE2 = 2,
            SLIDE3 = 3,
            SLIDE4 = 4
        }
    }
}
