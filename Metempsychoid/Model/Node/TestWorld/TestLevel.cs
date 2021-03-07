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
        private string nextTurnPhase;

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

            world.InitializeLevel(new List<string>() { "VsO7nJK", "gameLayer", "playerLayer" });

            this.InitializeStartLevelPhase(world);
        }

        public override void UpdateLogic(World world, Time timeElapsed)
        {
            switch (this.CurrentTurnPhase)
            {
                case TurnPhase.START_LEVEL:
                    this.UpdateStartLevelPhase(world);
                    break;
                case TurnPhase.CREATE_HAND:

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
            boardPlayerLayer.DrawCard();
            boardPlayerLayer.DrawCard();
            boardPlayerLayer.DrawCard();
            boardPlayerLayer.DrawCard();
        }

        private void UpdateStartLevelPhase(World world)
        {
            if(string.IsNullOrEmpty(this.nextTurnPhase) == false
                && this.nextTurnPhase == Enum.GetName(typeof(TurnPhase), TurnPhase.CREATE_HAND))
            {
                this.InitializeCreateHandPhase(world);
            }
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

        public override void OnGameEvent(World world, GameEvent gameEvent)
        {
            base.OnGameEvent(world, gameEvent);

            switch (gameEvent.Type)
            {
                case EventType.LEVEL_PHASE_CHANGE:
                    this.nextTurnPhase = gameEvent.Details;
                    break;
            }
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
