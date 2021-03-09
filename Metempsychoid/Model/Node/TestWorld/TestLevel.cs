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

            world.InitializeLevel(new List<string>() { "VsO7nJK", "gameLayer", "playerLayer" });

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
        }

        private void InitializeStartTurnPhase(World world)
        {
            this.SetCurrentTurnPhase(world, TurnPhase.START_TURN);
        }

        private void UpdateStartLevelPhase(World world)
        {
            string nextTurnPhase = Enum.GetName(typeof(TurnPhase), TurnPhase.CREATE_HAND);

            GameEvent nextTurnGameEvent = this.pendingGameEvents.FirstOrDefault(pElem => pElem.Type == EventType.LEVEL_PHASE_CHANGE && pElem.Details == nextTurnPhase);
            
            if (nextTurnGameEvent != null)
            {
                this.InitializeCreateHandPhase(world);
            }
        }

        private void UpdateCreateHandPhase(World world)
        {
            BoardPlayerLayer boardPlayerLayer = world.LoadedLayers["playerLayer"] as BoardPlayerLayer;
            foreach (GameEvent gameEvent in this.pendingGameEvents)
            {
                if(gameEvent.Type == EventType.DRAW_CARD)
                {
                    if (boardPlayerLayer.CardsHand.Count < NB_CARDS_HAND)
                    {
                        boardPlayerLayer.DrawCard();
                    }
                }
            }

            if(boardPlayerLayer.CardsHand.Count >= NB_CARDS_HAND)
            {
                this.InitializeStartTurnPhase(world);
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
