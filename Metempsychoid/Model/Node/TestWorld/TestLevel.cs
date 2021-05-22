using Metempsychoid.Model.Card;
using Metempsychoid.Model.Event;
using Metempsychoid.Model.Layer.BoardBannerLayer;
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

        private int playerIndex;

        private List<BoardPlayerLayer> boardplayersList;

        public int TurnIndex
        {
            get;
            private set;
        }

        public Player.Player Opponent
        {
            get;
            private set;
        }

        public TestLevel(World world) :
            base(world)
        {
            this.CurrentTurnPhase = TurnPhase.VOID;
            this.boardplayersList = new List<BoardPlayerLayer>();
        }

        public TurnPhase CurrentTurnPhase
        {
            get;
            private set;
        }

        protected BoardPlayerLayer CurrentBoardPlayer
        {
            get
            {
                return this.boardplayersList[this.TurnIndex % this.boardplayersList.Count];
            }
        }

        public override void VisitStart(World world)
        {
            base.VisitStart(world);

            this.Opponent = world.Opponent;

            this.playerIndex = 0;
            world.InitializeLevel(new List<string>()
            {
                "VsO7nJK",
                "gameLayer",
                "playerLayer",
                "opponentLayer",
                "bannerLayer"
            }, this);

            this.boardplayersList.Clear();
            this.boardplayersList.Add(world.LoadedLayers["playerLayer"] as BoardPlayerLayer);
            this.boardplayersList.Add(world.LoadedLayers["opponentLayer"] as BoardPlayerLayer);
            this.TurnIndex = -1;

            this.InitializeStartLevelPhase(world);
        }

        public Player.Player GetPlayerFromIndex(World world, out int currentPlayerIndex)
        {
            Player.Player result = null;
            if (this.playerIndex == 0)
            {
                result = world.Player;
            }
            else
            {
                result = this.Opponent;
            }

            currentPlayerIndex = this.playerIndex++;
            return result;
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
                    this.UpdateEndTurnPhase(world);
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

            boardPlayerLayer = world.LoadedLayers["opponentLayer"] as BoardPlayerLayer;
            boardPlayerLayer.NbCardsToDraw = NB_CARDS_HAND;
        }

        private void InitializeStartTurnPhase(World world)
        {
            this.TurnIndex++;

            BoardPlayerLayer boardPlayerLayer = this.CurrentBoardPlayer;

            BoardBannerLayer bannerLayer = world.LoadedLayers["bannerLayer"] as BoardBannerLayer;
            bannerLayer.PlayerTurn = boardPlayerLayer.SupportedPlayer;
            bannerLayer.TurnIndex = this.TurnIndex;

            this.SetCurrentTurnPhase(world, TurnPhase.START_TURN);

            boardPlayerLayer.OnStartTurn();
        }

        private void InitializeDrawPhase(World world)
        {
            this.SetCurrentTurnPhase(world, TurnPhase.DRAW);

            BoardPlayerLayer boardPlayerLayer = this.CurrentBoardPlayer; // world.LoadedLayers["playerLayer"] as BoardPlayerLayer;

            boardPlayerLayer.NbCardsToDraw = 1;
        }

        private void InitializeMainPhase(World world)
        {
            this.SetCurrentTurnPhase(world, TurnPhase.MAIN);
        }

        private void InitializeEndTurnPhase(World world)
        {
            BoardPlayerLayer boardPlayerLayer = this.CurrentBoardPlayer;

            this.SetCurrentTurnPhase(world, TurnPhase.END_TURN);

            boardPlayerLayer.OnEndTurn();
        }

        private void UpdateStartLevelPhase(World world)
        {
            if (this.CheckNextTurnPhaseEvent(TurnPhase.CREATE_HAND, null))
            {
                this.InitializeCreateHandPhase(world);
            }
        }

        private void UpdateCreateHandPhase(World world)
        {
            BoardPlayerLayer boardPlayerLayer = world.LoadedLayers["playerLayer"] as BoardPlayerLayer;
            BoardPlayerLayer boardOpponentLayer = world.LoadedLayers["opponentLayer"] as BoardPlayerLayer;

            if (this.CheckDrawCardEvent(world, boardPlayerLayer))
            {
                boardPlayerLayer.DrawCard();
            }

            if (this.CheckDrawCardEvent(world, boardOpponentLayer))
            {
                boardOpponentLayer.DrawCard(false);
            }

            if (this.CheckNextTurnPhaseEvent(TurnPhase.START_TURN, null))
            {
                this.InitializeStartTurnPhase(world);
            }
        }

        private void UpdateStartTurnPhase(World world)
        {
            //BoardPlayerLayer boardPlayerLayer = this.CurrentBoardPlayer;

            if (this.CheckNextTurnPhaseEvent(TurnPhase.DRAW, null))
            {
                this.InitializeDrawPhase(world);
            }
        }

        private void UpdateDrawPhase(World world)
        {
            BoardPlayerLayer boardPlayerLayer = this.CurrentBoardPlayer; // world.LoadedLayers["playerLayer"] as BoardPlayerLayer;

            if (this.CheckDrawCardEvent(world, boardPlayerLayer))
            {
                boardPlayerLayer.DrawCard();
            }

            if (this.CheckNextTurnPhaseEvent(TurnPhase.MAIN, boardPlayerLayer))
            {
                this.InitializeMainPhase(world);
            }
        }

        private void UpdateMainPhase(World world)
        {
            BoardPlayerLayer boardPlayerLayer = this.CurrentBoardPlayer; // world.LoadedLayers["playerLayer"] as BoardPlayerLayer;
            BoardGameLayer boardGameLayer = world.LoadedLayers["gameLayer"] as BoardGameLayer;

            if (this.CheckMoveCardOverboardEvent(world, boardPlayerLayer, out CardEntity cardToMove, out string detailsMove))
            {
                Vector2f startPosition = GetPositionFrom(detailsMove);
                boardGameLayer.MoveCardOverBoard(cardToMove, startPosition);
            }

            if (this.CheckFocusCardHandEvent(world, boardPlayerLayer, out CardEntity cardHandFocused, out string detailsHandFocused))
            {
                if (boardGameLayer.CardEntityPicked == null)
                {
                    boardPlayerLayer.CardEntityFocused = cardHandFocused;

                    boardGameLayer.CardEntityFocused = null;
                }
            }


            if (this.CheckFocusCardBoardEvent(world, null, out CardEntity cardBoardFocused, out string detailsBoardFocused))
            {
                if (boardGameLayer.CardEntityPicked == null && boardPlayerLayer.CardEntityFocused == null)
                {
                    boardGameLayer.CardEntityFocused = cardBoardFocused;
                }
            }

            if (this.CheckPickCardEvent(world, boardPlayerLayer, out CardEntity cardPicked, out string detailsPicked))
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

            if (this.CheckSocketCardEvent(world, null, out StarEntity starEntity))
            {
                if(starEntity != null)
                {
                    boardGameLayer.SocketCard(starEntity);
                }
            }

            if (this.CheckNextTurnPhaseEvent(TurnPhase.END_TURN, boardPlayerLayer))
            {
                this.InitializeEndTurnPhase(world);
            }
        }

        private void UpdateEndTurnPhase(World world)
        {
            if (this.CheckNextTurnPhaseEvent(TurnPhase.START_TURN, null))
            {
                this.InitializeStartTurnPhase(world);
            }
        }

        private bool CheckDrawCardEvent(World world, BoardPlayerLayer boardPlayerLayer)
        {
            if (this.pendingGameEvents.TryGetValue(EventType.DRAW_CARD, out List<GameEvent> gameEventsList))
            {
                foreach (GameEvent gameEvent in gameEventsList)
                {
                    if (boardPlayerLayer == null || gameEvent.Layer == boardPlayerLayer)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckPickCardEvent(World world, BoardPlayerLayer boardPlayerLayer, out CardEntity cardEntity, out string details)
        {
            cardEntity = null;
            details = null;

            if (this.pendingGameEvents.TryGetValue(EventType.PICK_CARD, out List<GameEvent> gameEventsList))
            {
                foreach (GameEvent gameEvent in gameEventsList)
                {
                    if (boardPlayerLayer == null || gameEvent.Layer == boardPlayerLayer)
                    {
                        cardEntity = gameEvent.Entity as CardEntity;
                        details = gameEvent.Details;

                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckSocketCardEvent(World world, BoardPlayerLayer boardPlayerLayer, out StarEntity starEntity)
        {
            starEntity = null;
            if (this.pendingGameEvents.TryGetValue(EventType.SOCKET_CARD, out List<GameEvent> gameEventsList))
            {
                foreach (GameEvent gameEvent in gameEventsList)
                {
                    if (boardPlayerLayer == null || gameEvent.Layer == boardPlayerLayer)
                    {
                        starEntity = gameEvent.Entity as StarEntity;

                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckFocusCardHandEvent(World world, BoardPlayerLayer boardPlayerLayer, out CardEntity cardEntity, out string detailsFocused)
        {
            cardEntity = null;
            detailsFocused = null;
            bool encounterGameEvent = false;

            if (this.pendingGameEvents.TryGetValue(EventType.FOCUS_CARD_HAND, out List<GameEvent> gameEventsList))
            {
                foreach (GameEvent gameEvent in gameEventsList)
                {
                    if (boardPlayerLayer == null || gameEvent.Layer == boardPlayerLayer)
                    {
                        if (cardEntity == null
                            || gameEvent.Entity != null)
                        {
                            cardEntity = gameEvent.Entity as CardEntity;

                            detailsFocused = gameEvent.Details;

                            encounterGameEvent = true;
                        }
                    }
                }
            }

            return encounterGameEvent;
        }

        private bool CheckFocusCardBoardEvent(World world, BoardPlayerLayer boardPlayerLayer, out CardEntity cardEntity, out string detailsFocused)
        {
            cardEntity = null;
            detailsFocused = null;
            bool encounterGameEvent = false;

            if (this.pendingGameEvents.TryGetValue(EventType.FOCUS_CARD_BOARD, out List<GameEvent> gameEventsList))
            {
                foreach (GameEvent gameEvent in gameEventsList)
                {
                    if (boardPlayerLayer == null || gameEvent.Layer == boardPlayerLayer)
                    {
                        if (cardEntity == null
                            || gameEvent.Entity != null)
                        {
                            cardEntity = gameEvent.Entity as CardEntity;

                            detailsFocused = gameEvent.Details;

                            encounterGameEvent = true;
                        }
                    }
                }
            }

            return encounterGameEvent;
        }

        private bool CheckMoveCardOverboardEvent(World world, BoardPlayerLayer boardPlayerLayer, out CardEntity cardEntity, out string details)
        {
            cardEntity = null;
            details = null;

            if (this.pendingGameEvents.TryGetValue(EventType.MOVE_CARD_OVERBOARD, out List<GameEvent> gameEventsList))
            {
                foreach (GameEvent gameEvent in gameEventsList)
                {
                    if (boardPlayerLayer == null || gameEvent.Layer == boardPlayerLayer)
                    {
                        cardEntity = gameEvent.Entity as CardEntity;
                        details = gameEvent.Details;

                        return true;
                    }
                }
            }
            return false;
        }

        private static Vector2f GetPositionFrom(string position)
        {
            string[] token = position.Split(':');

            return new Vector2f(float.Parse(token[0]), float.Parse(token[1]));
        }

        private bool CheckNextTurnPhaseEvent(TurnPhase nextTurnPhase, BoardPlayerLayer boardPlayerLayer)
        {
            string nextTurnPhaseString = Enum.GetName(typeof(TurnPhase), nextTurnPhase);

            if (this.pendingGameEvents.TryGetValue(EventType.LEVEL_PHASE_CHANGE, out List<GameEvent> gameEventsList))
            {
                foreach (GameEvent gameEvent in gameEventsList)
                {
                    if ((boardPlayerLayer == null || gameEvent.Layer == boardPlayerLayer) && gameEvent.Details == nextTurnPhaseString)
                    {
                        return true;
                    }
                }
            }
            return false;
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
