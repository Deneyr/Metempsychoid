using Metempsychoid.Model.Event;
using Metempsychoid.Model.Layer.BoardPlayerLayer;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Node.TestWorld
{
    public class TestLevel: ALevelNode
    {
        private static int NB_CARDS_HAND = 5;

        public TestLevel(World world) :
            base(world)
        {
            this.CurrentTurnPhase = TurnPhase.VOID;
        }

        public TurnPhase CurrentTurnPhase
        {
            get;
            private set;
        }

        public override void VisitStart(World world)
        {
            base.VisitStart(world);

            world.InitializeLevel(new List<string>()
            {
                "VsO7nJK",
                "gameLayer",
                "playerLayer",
                "bannerLayer"
            });

            this.InitializeStartLevelPhase(world);
        }

        protected override void InternalUpdateLogic(World world, Time timeElapsed)
        {
            switch (this.CurrentTurnPhase)
            {
                case TurnPhase.START_LEVEL:
                    this.UpdateStartLevelPhase(world);
                    break;
                case TurnPhase.CREATE_HAND:
                    this.UpdateCreateHandPhase(world);
                    break;
                case TurnPhase.START_TURN:

                    break;
                case TurnPhase.DRAW:

                    break;
                case TurnPhase.MAIN:

                    break;
                case TurnPhase.END_TURN:

                    break;
                case TurnPhase.END_LEVEL:

                    break;
            }
        }

        private void InitializeStartLevelPhase(World world)
        {
            this.SetCurrentTurnPhase(world, TurnPhase.START_LEVEL);
        }

        private void InitializeCreateHandPhase(World world)
        {
            this.SetCurrentTurnPhase(world, TurnPhase.CREATE_HAND);

            BoardPlayerLayer boardPlayerLayer = world.LoadedLayers["playerLayer"] as BoardPlayerLayer;

            boardPlayerLayer.NbCardsToDraw = NB_CARDS_HAND;
        }

        private void InitializeStartTurnPhase(World world)
        {
            this.SetCurrentTurnPhase(world, TurnPhase.START_TURN);
        }

        private void UpdateStartLevelPhase(World world)
        {
            if (this.CheckNextTurnPhaseEvent(TurnPhase.CREATE_HAND))
            {
                this.InitializeCreateHandPhase(world);
            }
        }

        private void UpdateCreateHandPhase(World world)
        {
            BoardPlayerLayer boardPlayerLayer = world.LoadedLayers["playerLayer"] as BoardPlayerLayer;

            if (this.CheckDrawCardEvent(world))
            {
                boardPlayerLayer.DrawCard();
            }

            if (this.CheckNextTurnPhaseEvent(TurnPhase.START_TURN))
            {
                this.InitializeStartTurnPhase(world);
            }
        }

        private bool CheckDrawCardEvent(World world)
        {
            BoardPlayerLayer boardPlayerLayer = world.LoadedLayers["playerLayer"] as BoardPlayerLayer;
            foreach (GameEvent gameEvent in this.pendingGameEvents)
            {
                if (gameEvent.Type == EventType.DRAW_CARD)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckNextTurnPhaseEvent(TurnPhase nextTurnPhase)
        {
            string nextTurnPhaseString = Enum.GetName(typeof(TurnPhase), nextTurnPhase);

            GameEvent nextTurnGameEvent = this.pendingGameEvents.FirstOrDefault(pElem => pElem.Type == EventType.LEVEL_PHASE_CHANGE && pElem.Details == nextTurnPhaseString);

            return nextTurnGameEvent != null;
        }

        private void SetCurrentTurnPhase(World world, TurnPhase newPhase)
        {
            if(this.CurrentTurnPhase != newPhase)
            {
                this.CurrentTurnPhase = newPhase;

                this.NotifyLevelStateChanged(world, Enum.GetName(typeof(TurnPhase), this.CurrentTurnPhase));
            }
        }

        public override void VisitEnd(World world)
        {
            this.SetCurrentTurnPhase(world, TurnPhase.VOID);

            base.VisitEnd(world);
        }
    }

    public enum TurnPhase
    {
        VOID,
        START_LEVEL,
        CREATE_HAND,
        START_TURN,
        DRAW,
        MAIN,
        END_TURN,
        END_LEVEL
    }
}
