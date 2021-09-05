using Metempsychoid.Model.Card;
using Metempsychoid.Model.Event;
using Metempsychoid.Model.Layer.BoardBannerLayer;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardNotifLayer;
using Metempsychoid.Model.Layer.BoardPlayerLayer;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Node.TestWorld
{
    public class CardBoardLevel: ALevelNode
    {
        private static int NB_CARDS_HAND = 4;

        private int playerIndex;

        public List<BoardPlayerLayer> BoardplayersList
        {
            get;
            private set;
        }

        public BoardNotifLayer BoardNotifLayer
        {
            get;
            private set;
        }

        public BoardGameLayer BoardGameLayer
        {
            get;
            private set;
        }

        public BoardBannerLayer BoardBannerLayer
        {
            get;
            private set;
        }

        public int TurnIndex
        {
            get
            {
                return this.BoardBannerLayer.TurnIndex;
            }
            set
            {
                this.BoardBannerLayer.TurnIndex = value;
            }
        }

        public Player.Player MainPlayer
        {
            get;
            private set;
        }

        public Player.Player Opponent
        {
            get;
            private set;
        }

        public CardBoardLevel(World world) :
            base(world)
        {
            this.CurrentTurnPhase = TurnPhase.VOID;
            this.BoardplayersList = new List<BoardPlayerLayer>();
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
                return this.BoardplayersList[this.TurnIndex % this.BoardplayersList.Count];
            }
        }

        public override void VisitStart(World world)
        {
            base.VisitStart(world);

            this.MainPlayer = world.Player;
            this.Opponent = world.Opponent;

            this.playerIndex = 0;

            this.BoardBannerLayer = world.LoadedLayers["bannerLayer"] as BoardBannerLayer;
            this.BoardBannerLayer.PreInitMaxTurnCount = 1;

            world.InitializeLevel(new List<string>()
            {
                "VsO7nJK",
                "gameLayer",
                "playerLayer",
                "opponentLayer",
                "notifLayer",
                "bannerLayer"
            }, this);

            this.BoardplayersList.Clear();
            this.BoardplayersList.Add(world.LoadedLayers["playerLayer"] as BoardPlayerLayer);
            this.BoardplayersList.Add(world.LoadedLayers["opponentLayer"] as BoardPlayerLayer);

            this.BoardNotifLayer = world.LoadedLayers["notifLayer"] as BoardNotifLayer;
            this.BoardGameLayer = world.LoadedLayers["gameLayer"] as BoardGameLayer;

            this.InitializeStartLevelPhase(world);
        }

        public Player.Player GetPlayerFromIndex(World world, out int currentPlayerIndex)
        {
            Player.Player result = null;
            if (this.playerIndex == 0)
            {
                result = this.MainPlayer;
            }
            else
            {
                result = this.Opponent;
            }

            currentPlayerIndex = this.playerIndex++;
            return result;
        }

        public BoardPlayerLayer GetLayerFromPlayer(Player.Player player)
        {
            return this.BoardplayersList.FirstOrDefault(pElem => pElem.SupportedPlayer == player);
        }

        //public override void UpdateLogic(World world, Time timeElapsed)
        //{
        //    base.UpdateLogic(world, timeElapsed);

        //    BoardGameLayer boardGameLayer = world.LoadedLayers["gameLayer"] as BoardGameLayer;
        //    boardGameLayer.UpdateLogic(world, timeElapsed);

        //    BoardGameLayer notifGameLayer = world.LoadedLayers["notifLayer"] as BoardGameLayer;
        //    boardGameLayer.UpdateLogic(world, timeElapsed);
        //}

        protected override void InternalUpdateLogic(World world, Time timeElapsed)
        {
            //this.UpdateCanevas();

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
                case TurnPhase.COUNT_POINTS:
                    this.UpdateCountPointsLevelPhase(world);
                    break;
                case TurnPhase.END_LEVEL:
                    this.UpdateEndLevelPhase(world);
                    break;
            }
        }

        //private void UpdateCanevas()
        //{
        //    if (this.pendingGameEvents.TryGetValue(EventType.CANEVAS_CHANGED, out List<GameEvent> gameEventsList))
        //    {
        //        GameEvent canevasChangedEvent = gameEventsList.FirstOrDefault();

        //        string[] canevasTokens = canevasChangedEvent.Details.Split(':');

        //        this.SceneCanevas = new FloatRect(float.Parse(canevasTokens[0]), float.Parse(canevasTokens[1]), float.Parse(canevasTokens[2]), float.Parse(canevasTokens[3]));
        //    }
        //}

        private void InitializeStartLevelPhase(World world)
        {
            this.SetCurrentTurnPhase(world, TurnPhase.START_LEVEL);
        }

        private void InitializeCreateHandPhase(World world)
        {
            this.SetCurrentTurnPhase(world, TurnPhase.CREATE_HAND);

            BoardPlayerLayer boardPlayerLayer = world.LoadedLayers["playerLayer"] as BoardPlayerLayer;
            boardPlayerLayer.NbCardsToDraw = this.MainPlayer.Deck.CardIds.Count;

            boardPlayerLayer = world.LoadedLayers["opponentLayer"] as BoardPlayerLayer;
            boardPlayerLayer.NbCardsToDraw = this.Opponent.Deck.CardIds.Count;
        }

        private void InitializeStartTurnPhase(World world)
        {
            BoardBannerLayer bannerLayer = this.BoardBannerLayer;
            BoardGameLayer boardGameLayer = this.BoardGameLayer;

            this.TurnIndex++;

            BoardPlayerLayer boardPlayerLayer = this.CurrentBoardPlayer;

            bannerLayer.PlayerTurn = boardPlayerLayer.SupportedPlayer;
            boardGameLayer.PlayerTurn = boardPlayerLayer.SupportedPlayer;

            this.SetCurrentTurnPhase(world, TurnPhase.START_TURN);

            boardPlayerLayer.OnStartTurn();
        }

        private void InitializeDrawPhase(World world)
        {
            this.SetCurrentTurnPhase(world, TurnPhase.DRAW);

            BoardPlayerLayer boardPlayerLayer = this.CurrentBoardPlayer; // world.LoadedLayers["playerLayer"] as BoardPlayerLayer;

            boardPlayerLayer.NbCardsToDraw = 0;
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

        private void InitializeCountPointsPhase(World world)
        {
            BoardGameLayer boardGameLayer = world.LoadedLayers["gameLayer"] as BoardGameLayer;
            BoardBannerLayer bannerLayer = world.LoadedLayers["bannerLayer"] as BoardBannerLayer;

            int playerScore = 0;
            int opponentScore = 0;
            foreach(CJStarDomain domain in boardGameLayer.StarDomains)
            {
                if(domain.IsThereAtLeastOneCard)
                {
                    if(domain.DomainOwner == null)
                    {
                        playerScore++;
                        opponentScore++;
                    }
                    else if(domain.DomainOwner == this.MainPlayer)
                    {
                        playerScore++;
                    }
                    else
                    {
                        opponentScore++;
                    }
                }
            }

            List<int> scoresPlayer = bannerLayer.PlayerNameToTotalScores[this.MainPlayer.PlayerName];
            List<int> scoresOpponent = bannerLayer.PlayerNameToTotalScores[this.Opponent.PlayerName];
            if (scoresPlayer.Count > 0)
            {
                playerScore += scoresPlayer.Last();
                opponentScore += scoresOpponent.Last();
            }

            playerScore += bannerLayer.PlayerNameToModifier[this.MainPlayer.PlayerName];
            opponentScore += bannerLayer.PlayerNameToModifier[this.Opponent.PlayerName];

            bannerLayer.ClearModifiers();

            scoresPlayer.Add(playerScore);
            scoresOpponent.Add(opponentScore);

            this.SetCurrentTurnPhase(world, TurnPhase.COUNT_POINTS);
        }

        private void InitializeEndLevelPhase(World world)
        {
            this.SetCurrentTurnPhase(world, TurnPhase.END_LEVEL);
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
            BoardNotifLayer boardNotifLayer = world.LoadedLayers["notifLayer"] as BoardNotifLayer;

            if (boardNotifLayer.CurrentNotifBehavior != null)
            {
                boardNotifLayer.ForwardGameEventsToBehavior(this.pendingGameEvents);
            }
            else
            {

                if (this.CheckMoveCardOverboardEvent(world, boardPlayerLayer, out CardEntity cardToMove, out string detailsMove))
                {
                    this.MoveCardOverBoard(detailsMove, cardToMove);
                }

                if (this.CheckFocusCardHandEvent(world, boardPlayerLayer, out CardEntity cardHandFocused, out string detailsHandFocused))
                {
                    //if (boardGameLayer.CardEntityPicked == null)
                    //{
                    boardPlayerLayer.CardEntityFocused = cardHandFocused;

                    boardGameLayer.CardEntityFocused = null;
                    //}
                }


                if (this.CheckFocusCardBoardEvent(world, null, out CardEntity cardBoardFocused, out string detailsBoardFocused))
                {
                    if (/*boardGameLayer.CardEntityPicked == null && */boardPlayerLayer.CardEntityFocused == null)
                    {
                        boardGameLayer.CardEntityFocused = cardBoardFocused;
                    }
                }

                bool isThereSocketCardEvent = this.CheckSocketCardEvent(world, null, out StarEntity starEntity);
                if (isThereSocketCardEvent)
                {
                    if (starEntity != null)
                    {
                        boardGameLayer.SocketCard(starEntity);
                    }
                }
                else
                {
                    bool IsTherePickCardEvent = this.CheckPickCardEvent(world, boardPlayerLayer, out CardEntity cardPicked, out string detailsPicked);
                    bool IsThereUnpickCardEvent = this.CheckUnPickCardEvent(world, boardPlayerLayer, out CardEntity cardUnpicked, out string detailsUnpicked);

                    if (boardGameLayer.CardEntityPicked == null)
                    {
                        if (IsTherePickCardEvent)
                        {
                            this.PickCard(boardPlayerLayer, cardPicked);
                        }
                    }
                    else
                    {
                        if (IsThereUnpickCardEvent)
                        {
                            this.UnpickCard(boardPlayerLayer, detailsUnpicked);
                        }

                        if (IsTherePickCardEvent)
                        {
                            this.PickCard(boardPlayerLayer, cardPicked);
                        }
                    }
                }

                if (this.CheckNextTurnPhaseEvent(TurnPhase.END_TURN, null))
                {
                    boardPlayerLayer.CardEntityFocused = null;
                    boardGameLayer.CardEntityFocused = null;

                    if (boardGameLayer.CardEntityPicked == null
                        && boardGameLayer.PendingActions.Count == 0)
                    {
                        this.InitializeEndTurnPhase(world);
                    }
                }
            }
        }

        public void UpdateEndLevelPhase(World world)
        {
            if (this.CheckLevelChangeEvent(world, out string detailsEvent))
            {
                world.NotifyInternalGameEvent(new InternalGameEvent(InternalEventType.GO_TO_LEVEL, detailsEvent));
            }
        }

        public CardEntity PickCard(BoardPlayerLayer boardPlayerLayer, CardEntity cardPicked)
        {
            if (this.BoardGameLayer.CardEntityPicked == null)
            {
                if (boardPlayerLayer.PickCard(cardPicked))
                {
                    return this.BoardGameLayer.PickCard(cardPicked.Card);
                }
                this.BoardGameLayer.PickCard(cardPicked);
            }

            return null;
        }

        public CardEntity UnpickCard(BoardPlayerLayer boardPlayerLayer, string detailUnpicked)
        {
            if (this.BoardGameLayer.CardEntityPicked != null)
            {
                Card.Card cardToUnpick = this.BoardGameLayer.CardEntityPicked.Card;
                Vector2f startPosition = GetPositionFrom(detailUnpicked);

                if (this.BoardGameLayer.UnPickCard() && boardPlayerLayer != null)
                {
                    return boardPlayerLayer.UnpickCard(cardToUnpick, startPosition);
                }
            }

            return null;
        }

        public CardEntity PickCard(BoardNotifLayer boardNotifLayer, CardEntity cardPicked)
        {
            if (this.BoardGameLayer.CardEntityPicked == null)
            {
                if (boardNotifLayer.PickCard(cardPicked))
                {
                    return this.BoardGameLayer.PickCard(cardPicked.Card);
                }
                this.BoardGameLayer.PickCard(cardPicked);
            }

            return null;
        }

        public CardEntity UnpickCard(BoardNotifLayer boardNotifLayer, string detailUnpicked)
        {
            if (this.BoardGameLayer.CardEntityPicked != null)
            {
                Card.Card cardToUnpick = this.BoardGameLayer.CardEntityPicked.Card;
                Vector2f startPosition = GetPositionFrom(detailUnpicked);

                if (this.BoardGameLayer.UnPickCard() && boardNotifLayer != null)
                {
                    return boardNotifLayer.UnpickCard(cardToUnpick, startPosition);
                }
            }

            return null;
        }

        public void MoveCardOverBoard(string detailsMove, CardEntity cardToMove)
        {
            Vector2f startPosition = GetPositionFrom(detailsMove);
            this.BoardGameLayer.MoveCardOverBoard(cardToMove, startPosition);
        }

        //public void AddCardToNotifBoard(World world, CardEntity cardEntityToAdd)
        //{
        //    BoardGameLayer boardGameLayer = world.LoadedLayers["gameLayer"] as BoardGameLayer;
        //    BoardNotifLayer boardNotifLayer = world.LoadedLayers["notifLayer"] as BoardNotifLayer;

        //    boardGameLayer.GetCardFromBoard(cardEntityToAdd);

        //    boardNotifLayer.AddCardToBoard(cardEntityToAdd);
        //}

        //public void RemoveCardFromNotifBoard(World world, CardEntityDecorator cardEntityToRemove)
        //{
        //    BoardGameLayer boardGameLayer = world.LoadedLayers["gameLayer"] as BoardGameLayer;
        //    BoardNotifLayer boardNotifLayer = world.LoadedLayers["notifLayer"] as BoardNotifLayer;

        //    boardNotifLayer.RemoveCardFromBoard(cardEntityToRemove);

        //    boardGameLayer.ReturnCardToBoard(cardEntityToRemove);
        //}

        public void ClearLayersSelection()
        {
            this.BoardGameLayer.CardEntityFocused = null;

            this.CurrentBoardPlayer.CardEntityFocused = null;
        }

        private void UpdateEndTurnPhase(World world)
        {
            if (this.CheckNextTurnPhaseEvent(TurnPhase.START_TURN, null))
            {
                if (this.TurnIndex % 2 == 0)
                {
                    this.InitializeStartTurnPhase(world);
                }
                else
                {
                    this.InitializeCountPointsPhase(world);
                }
            }
        }

        private void UpdateCountPointsLevelPhase(World world)
        {
            if (this.CheckNextTurnPhaseEvent(TurnPhase.START_TURN, null))
            {
                foreach (CJStarDomain domain in this.BoardGameLayer.StarDomains)
                {
                    domain.TemporaryDomainOwner = null;
                }

                if (this.BoardBannerLayer.CurrentTurnCount < this.BoardBannerLayer.MaxTurnCount)
                {
                    this.BoardBannerLayer.CurrentTurnCount++;

                    this.InitializeStartTurnPhase(world);
                }
                else
                {
                    this.InitializeEndLevelPhase(world);
                }
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
                        if (cardEntity != null)
                        {
                            details = gameEvent.Details;

                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private bool CheckUnPickCardEvent(World world, BoardPlayerLayer boardPlayerLayer, out CardEntity cardEntity, out string details)
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
                        if (cardEntity == null)
                        {
                            details = gameEvent.Details;

                            return true;
                        }
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

        public static Vector2f GetPositionFrom(string position)
        {
            string[] token = position.Split(':');

            return new Vector2f(float.Parse(token[0]), float.Parse(token[1]));
        }

        private bool CheckNextTurnPhaseEvent(TurnPhase nextTurnPhase, BoardPlayerLayer boardPlayerLayer)
        {
            string nextTurnPhaseString = Enum.GetName(typeof(TurnPhase), nextTurnPhase);

            if (this.pendingGameEvents.TryGetValue(EventType.LEVEL_PHASE_CHANGE, out List<GameEvent> gameEventsList))
            {
                if(boardPlayerLayer == null)
                {
                    return true;
                }

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
        COUNT_POINTS,
        END_LEVEL
    }
}
