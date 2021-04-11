using Metempsychoid.Model.Card;
using Metempsychoid.Model.Event;
using Metempsychoid.Model.Layer.BoardGameLayer;
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
        private static int NB_CARDS_HAND = 4;

        protected Player.Player opponent;

        public TestLevel(World world) :
            base(world)
        {
            this.CurrentTurnPhase = TurnPhase.VOID;

            this.opponent = new Player.Player(SFML.Graphics.Color.Green, "Seth");
            for (int i = 0; i < 30; i++)
            {
                this.opponent.Deck.Cards.Add(world.CardLibrary.CreateCard("wheel", this.opponent));
            }
        }

        public TurnPhase CurrentTurnPhase
        {
            get;
            private set;
        }

        public override void VisitStart(World world)
        {
            base.VisitStart(world);

            world.Opponent = this.opponent;

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
                    this.UpdateStartTurnPhase(world);
                    break;
                case TurnPhase.DRAW:
                    this.UpdateDrawPhase(world);
                    break;
                case TurnPhase.MAIN:
                    this.UpdateMainPhase(world);
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

        private void InitializeDrawPhase(World world)
        {
            this.SetCurrentTurnPhase(world, TurnPhase.DRAW);

            BoardPlayerLayer boardPlayerLayer = world.LoadedLayers["playerLayer"] as BoardPlayerLayer;

            boardPlayerLayer.NbCardsToDraw = 1;
        }

        private void InitializeMainPhase(World world)
        {
            this.SetCurrentTurnPhase(world, TurnPhase.MAIN);
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

        private void UpdateStartTurnPhase(World world)
        {
            if (this.CheckNextTurnPhaseEvent(TurnPhase.DRAW))
            {
                this.InitializeDrawPhase(world);
            }
        }

        private void UpdateDrawPhase(World world)
        {
            BoardPlayerLayer boardPlayerLayer = world.LoadedLayers["playerLayer"] as BoardPlayerLayer;

            if (this.CheckDrawCardEvent(world))
            {
                boardPlayerLayer.DrawCard();
            }

            if (this.CheckNextTurnPhaseEvent(TurnPhase.MAIN))
            {
                this.InitializeMainPhase(world);
            }
        }

        private void UpdateMainPhase(World world)
        {
            BoardPlayerLayer boardPlayerLayer = world.LoadedLayers["playerLayer"] as BoardPlayerLayer;
            BoardGameLayer boardGameLayer = world.LoadedLayers["gameLayer"] as BoardGameLayer;

            if (this.CheckMoveCardOverboardEvent(world, out CardEntity cardToMove, out string detailsMove))
            {
                Vector2f startPosition = GetPositionFrom(detailsMove);
                boardGameLayer.MoveCardOverBoard(cardToMove, startPosition);
            }

            if (this.CheckFocusCardEvent(world, out CardEntity cardFocused))
            {
                if (boardGameLayer.CardEntityPicked == null)
                {
                    boardPlayerLayer.CardEntityFocused = cardFocused;
                }
            }

            if(this.CheckPickCardEvent(world, out CardEntity cardPicked, out string detailsPicked))
            {
                if (cardPicked != null)
                {
                    if (boardGameLayer.CardEntityPicked == null)
                    {
                        if (boardPlayerLayer.PickCard(cardPicked))
                        {
                            boardGameLayer.PickCard(cardPicked.Card);
                        }
                        else
                        {
                            boardGameLayer.PickCard(cardPicked);
                        }
                    }
                }
                else
                {
                    if (boardGameLayer.CardEntityPicked != null)
                    {
                        Card.Card cardToUnpick = boardGameLayer.CardEntityPicked.Card;
                        Vector2f startPosition = GetPositionFrom(detailsPicked);

                        if (boardGameLayer.UnPickCard())
                        {
                            boardPlayerLayer.UnpickCard(cardToUnpick, startPosition);
                        }
                    }
                }
            }

            if (this.CheckSocketCardEvent(world, out StarEntity starEntity))
            {
                if(starEntity != null)
                {
                    boardGameLayer.SocketCard(starEntity);
                }
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

        private bool CheckPickCardEvent(World world, out CardEntity cardEntity, out string details)
        {
            cardEntity = null;
            details = null;

            foreach (GameEvent gameEvent in this.pendingGameEvents)
            {
                if (gameEvent.Type == EventType.PICK_CARD)
                {
                    cardEntity = gameEvent.Entity as CardEntity;
                    details = gameEvent.Details;

                    return true;
                }
            }
            return false;
        }

        private bool CheckSocketCardEvent(World world, out StarEntity starEntity)
        {
            starEntity = null;

            foreach (GameEvent gameEvent in this.pendingGameEvents)
            {
                if (gameEvent.Type == EventType.SOCKET_CARD)
                {
                    starEntity = gameEvent.Entity as StarEntity;

                    return true;
                }
            }
            return false;
        }

        private bool CheckFocusCardEvent(World world, out CardEntity cardEntity)
        {
            cardEntity = null;

            foreach (GameEvent gameEvent in this.pendingGameEvents)
            {
                if (gameEvent.Type == EventType.FOCUS_CARD)
                {
                    cardEntity = gameEvent.Entity as CardEntity;

                    return true;
                }
            }
            return false;
        }

        private bool CheckMoveCardOverboardEvent(World world, out CardEntity cardEntity, out string details)
        {
            cardEntity = null;
            details = null;

            foreach (GameEvent gameEvent in this.pendingGameEvents)
            {
                if (gameEvent.Type == EventType.MOVE_CARD_OVERBOARD)
                {
                    cardEntity = gameEvent.Entity as CardEntity;
                    details = gameEvent.Details;

                    return true;
                }
            }
            return false;
        }

        private static Vector2f GetPositionFrom(string position)
        {
            string[] token = position.Split(':');

            return new Vector2f(float.Parse(token[0]), float.Parse(token[1]));
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

            world.Opponent = null;

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
