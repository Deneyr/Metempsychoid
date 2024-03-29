﻿using Astrategia.Animation;
using Astrategia.Model.Animation;
using Astrategia.Model.Card;
using Astrategia.Model.Layer.BoardGameLayer.Actions;
using Astrategia.Model.Layer.BoardNotifLayer;
using Astrategia.Model.Layer.BoardNotifLayer.Behavior;
using Astrategia.Model.Node;
using Astrategia.Model.Node.TestWorld;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Layer.BoardGameLayer
{
    public class BoardGameLayer: EntityLayer.EntityLayer
    {
        private CardEntity cardFocused;

        private CJStarDomain domainFocused;

        private CardEntity cardPicked;

        private Player.Player playerTurn;

        private int nbCardsToPlace;

        public string PreInitGalaxyName
        {
            get;
            set;
        }

        public List<StarEntity> BehaviorSourceStarEntities
        {
            get;
            private set;
        }

        public List<StarEntity> BehaviorTargetStarEntities
        {
            get;
            private set;
        }

        public Dictionary<string, HashSet<CardEntity>> NameToOnBoardCardEntities
        {
            get;
            private set;
        }

        public List<CardEntity> CardsOffBoard
        {
            get;
            private set;
        }

        public List<IBoardGameAction> PendingActions
        {
            get;
            private set;
        }

        public int NbCardsToPlace
        {
            get
            {
                return this.nbCardsToPlace;
            }
            private set
            {
                if(this.nbCardsToPlace != value)
                {
                    this.nbCardsToPlace = value;

                    this.NbCardsToPlaceChanged?.Invoke(this.nbCardsToPlace);
                }
            }
        }

        public Player.Player PlayerTurn
        {
            get
            {
                return this.playerTurn;
            }
            set
            {
                if (this.playerTurn != value)
                {
                    this.playerTurn = value;

                    this.NbCardsToPlace = 1;
                }
            }
        }

        public Dictionary<StarEntity, HashSet<StarLinkEntity>> StarToLinks
        {
            get;
            private set;
        }

        public HashSet<StarEntity> StarSystem
        {
            get;
            protected set;
        }

        public HashSet<StarLinkEntity> StarLinkSystem
        {
            get;
            protected set;
        }

        public CardEntity CardEntityPicked
        {
            get
            {
                return this.cardPicked;
            }
            set
            {
                if (value != this.cardPicked)
                {
                    this.cardPicked = value;

                    this.CardPicked?.Invoke(this.cardPicked);
                }
            }
        }

        public CardEntity CardEntityFocused
        {
            get
            {
                return this.cardFocused;
            }
            set
            {
                if (value != this.cardFocused)
                {
                    if (value == null || this.Entities.Contains(value))
                    {
                        this.cardFocused = value;

                        this.CardFocused?.Invoke(this.cardFocused);
                    }
                }
            }
        }

        public CJStarDomain DomainEntityFocused
        {
            get
            {
                return this.domainFocused;
            }
            set
            {
                if (value != this.domainFocused)
                {
                    if (value == null || this.Entities.Contains(value))
                    {
                        this.domainFocused = value;

                        this.DomainFocused?.Invoke(this.domainFocused);
                    }
                }
            }
        }

        public List<CJStarDomain> StarDomains
        {
            get;
            protected set;
        }

        public event Action<CardEntity> CardPicked;
        public event Action<CardEntity> CardFocused;

        public event Action<CJStarDomain> DomainFocused;

        public event Action<List<StarEntity>> SourceStarEntitiesSet;
        public event Action<List<StarEntity>> TargetStarEntitiesSet;

        public event Action<int> NbCardsToPlaceChanged;

        public BoardGameLayer()
        {
            this.PreInitGalaxyName = "Default";

            this.StarSystem = new HashSet<StarEntity>();
            this.StarLinkSystem = new HashSet<StarLinkEntity>();

            this.StarToLinks = new Dictionary<StarEntity, HashSet<StarLinkEntity>>();

            this.StarDomains = new List<CJStarDomain>();

            //this.DeactivatedCardEntities = new Dictionary<Card.Card, CardEntity>();
            this.CardsOffBoard = new List<CardEntity>();
            this.PendingActions = new List<IBoardGameAction>();
            this.NameToOnBoardCardEntities = new Dictionary<string, HashSet<CardEntity>>();

            this.BehaviorSourceStarEntities = null;
            this.BehaviorTargetStarEntities = null;

            this.TypesInChunk.Add(typeof(StarEntity));
            this.TypesInChunk.Add(typeof(StarLinkEntity));
            this.TypesInChunk.Add(typeof(CardEntity));
        }

        internal void AddStar(StarEntity starEntity)
        {
            this.StarSystem.Add(starEntity);
            this.AddEntityToLayer(starEntity);

            this.StarToLinks.Add(starEntity, new HashSet<StarLinkEntity>());
        }

        protected void RemoveStar(StarEntity starEntity)
        {
            this.StarSystem.Remove(starEntity);
            this.RemoveEntityFromLayer(starEntity);

            HashSet<StarLinkEntity> linksList = this.StarToLinks[starEntity];
            this.StarToLinks.Remove(starEntity);

            foreach(StarLinkEntity link in linksList)
            {
                this.RemoveStarLink(link);
            }

            starEntity.Dispose();
        }

        internal StarLinkEntity AddStarLink(StarEntity starEntityFrom, StarEntity starEntityTo)
        {
            StarLinkEntity link = new StarLinkEntity(this, starEntityFrom, starEntityTo);

            this.StarLinkSystem.Add(link);
            this.AddEntityToLayer(link);

            this.StarToLinks[starEntityFrom].Add(link);
            this.StarToLinks[starEntityTo].Add(link);

            return link;
        }

        internal StarLinkEntity AddCurvedStarLink(StarEntity starEntityFrom, StarEntity starEntityTo, float radius)
        {
            StarLinkEntity link = new CurvedStarLinkEntity(this, starEntityFrom, starEntityTo, radius);

            this.StarLinkSystem.Add(link);
            this.AddEntityToLayer(link);

            this.StarToLinks[starEntityFrom].Add(link);
            this.StarToLinks[starEntityTo].Add(link);

            return link;
        }

        internal void AddStarDomain(CJStarDomain domainToAdd)
        {
            this.AddEntityToLayer(domainToAdd);
            this.StarDomains.Add(domainToAdd);
        }

        protected void RemoveStarLink(StarLinkEntity starLinkEntity)
        {
            this.StarLinkSystem.Remove(starLinkEntity);
            this.RemoveEntityFromLayer(starLinkEntity);

            if (this.StarToLinks.ContainsKey(starLinkEntity.StarFrom))
            {
                this.StarToLinks[starLinkEntity.StarFrom].Remove(starLinkEntity);
            }

            if (this.StarToLinks.ContainsKey(starLinkEntity.StarTo))
            {
                this.StarToLinks[starLinkEntity.StarTo].Remove(starLinkEntity);
            }
        }

        public void SetBehaviorSourceStarEntities(List<StarEntity> sourceStarEntities)
        {
            this.UnPickCard();

            this.CardEntityFocused = null;
            this.DomainEntityFocused = null;

            this.BehaviorTargetStarEntities = null;
            if (sourceStarEntities != null && sourceStarEntities.Count > 0)
            {
                this.BehaviorSourceStarEntities = new List<StarEntity>(sourceStarEntities);
            }
            else
            {
                this.BehaviorSourceStarEntities = null;
            }

            this.SourceStarEntitiesSet?.Invoke(this.BehaviorSourceStarEntities);
        }

        public void SetBehaviorTargetStarEntities(List<StarEntity> targetStarEntities)
        {
            if (targetStarEntities != null && targetStarEntities.Count > 0)
            {
                this.BehaviorTargetStarEntities = new List<StarEntity>(targetStarEntities);
            }
            else
            {
                this.BehaviorTargetStarEntities = null;
            }

            this.TargetStarEntitiesSet?.Invoke(this.BehaviorTargetStarEntities);
        }

        public CardEntity PickCard(Card.Card card, bool isFliped = true)
        {
            if (this.CardEntityPicked == null)
            {
                CardEntity cardEntity = new CardEntity(this, card, isFliped);

                this.AddEntityToLayer(cardEntity);

                this.CardEntityPicked = cardEntity;

                return cardEntity;
            }
            return null;
        }

        public bool PickCard(CardEntity cardEntity)
        {
            if (this.CardEntityPicked == null)
            {
                this.CardEntityPicked = cardEntity;

                return true;
            }
            return false;
        }

        public bool UnPickCard()
        {
            if (this.CardEntityPicked != null)
            {
                CardEntity cardEntity = this.CardEntityPicked;

                if (cardEntity.ParentStar != null)
                {
                    IAnimation positionAnimation = new PositionAnimation(cardEntity.Position, cardEntity.ParentStar.Position, Time.FromSeconds(1f), AnimationType.ONETIME, InterpolationMethod.SQUARE_DEC);

                    cardEntity.PlayAnimation(positionAnimation);

                    this.CardEntityPicked = null;

                    return false;
                }
                else
                {
                    this.RemoveEntityFromLayer(cardEntity);

                    this.CardEntityPicked = null;

                    return true;
                }
            }

            return false;
        }

        public void MoveCard(StarEntity starEntity)
        {
            if(this.CardEntityPicked.Card.CanBeMoved 
                && this.CardEntityPicked.ParentStar != starEntity)
            {
                this.PendingActions.Add(new UnsocketCardAction(this.CardEntityPicked, false));
                this.PendingActions.Add(new SocketCardAction(this.CardEntityPicked, starEntity));

                this.CardEntityPicked = null;
            }
        }

        public void DeleteCard(StarEntity starEntity)
        {
            if (starEntity.CardSocketed != null)
            {
                this.PendingActions.Add(new UnsocketCardAction(starEntity.CardSocketed, true));

                this.CardEntityPicked = null;
            }
        }

        public void ConvertCard(StarEntity starEntity, Player.Player newCardOwner)
        {
            if (starEntity.CardSocketed != null)
            {
                this.PendingActions.Add(new ConvertCardAction(starEntity.CardSocketed, newCardOwner));

                this.CardEntityPicked = null;
            }
        }

        public void SwapCard(StarEntity starEntity)
        {
            if (this.CardEntityPicked.Card.CanBeMoved 
                && this.CardEntityPicked.ParentStar != starEntity)
            {
                CardEntity toCardEntity = starEntity.CardSocketed;
                StarEntity toStarEntity = this.CardEntityPicked.ParentStar;

                this.PendingActions.Add(new UnsocketCardAction(this.CardEntityPicked, false));
                this.PendingActions.Add(new UnsocketCardAction(toCardEntity, false));

                this.PendingActions.Add(new SocketCardAction(this.CardEntityPicked, starEntity));
                this.PendingActions.Add(new SocketCardAction(toCardEntity, toStarEntity, true));

                this.CardEntityPicked = null;
            }
        }

        public void SocketCard(StarEntity starEntity, bool isTurnLimited) //, Vector2f positionInNotifBoard)
        {
            if(this.CardEntityPicked != null)
            {
                if (isTurnLimited == false || this.NbCardsToPlace > 0)
                {
                    this.CardEntityPicked.IsFliped = true;

                    this.PendingActions.Add(new SocketCardAction(this.CardEntityPicked, starEntity));

                    this.CardEntityPicked = null;

                    if (isTurnLimited)
                    {
                        this.NbCardsToPlace--;
                    }
                }
            }
        }

        public void SecureDomains(StarEntity starEntity)
        {
            if(starEntity.CardSocketed != null)
            {
                IEnumerable<CJStarDomain> domainConcerned = this.StarDomains.Where(pElem => pElem.Domain.Contains(starEntity));

                foreach(CJStarDomain domain in domainConcerned)
                {
                    this.PendingActions.Add(new SecureDomainAction(domain, starEntity.CardSocketed.Card.CurrentOwner));
                }
            }
        }

        //public void GetCardFromBoard(CardEntity cardEntity)
        //{
        //    //cardEntity.IsActive = false;
        //}

        //public void ReturnCardToBoard(CardEntityDecorator cardToReturn)
        //{
        //    cardToReturn.CardEntityDecorated.IsActive = true;
        //}

        //private void MoveCard(StarEntity fromStarEntity, StarEntity toStarEntity, CardEntity cardConcerned)
        //{
        //    if (fromStarEntity != null)
        //    {
        //        cardConcerned.Card.CardUnsocketed(this, toStarEntity);
        //        fromStarEntity.CardSocketed = null;

        //        this.starsChanged.Add(fromStarEntity);
        //    }

        //    if (toStarEntity != null)
        //    {
        //        cardConcerned.Card.CardSocketed(this, toStarEntity);
        //        toStarEntity.CardSocketed = cardConcerned;

        //        this.starsChanged.Add(toStarEntity);
        //    }
        //}

        public override void UpdateLogic(World world, Time deltaTime)
        {
            base.UpdateLogic(world, deltaTime);

            // Apply the actions.
            this.UpdateBoard();
        }

        public int GetNbOpponentDeadCard(Player.Player player)
        {
            BoardPlayerLayer.BoardPlayerLayer playerLayer = (this.ownerLevelNode as CardBoardLevel).BoardPlayersList.FirstOrDefault(pElem => pElem.SupportedPlayer != player);

            return playerLayer.CardsCemetery.Count;
        }

        public int GetNbDeadCard(Player.Player player)
        {
            BoardPlayerLayer.BoardPlayerLayer playerLayer = (this.ownerLevelNode as CardBoardLevel).BoardPlayersList.FirstOrDefault(pElem => pElem.SupportedPlayer == player);

            return playerLayer.CardsCemetery.Count;
        }

        private void UpdateBoard()
        {
            if (this.PendingActions.Count > 0)
            {
                // Clear the actions pending list to allow it to be fill by the next generation of events raised by card awakening ...
                List<IBoardGameAction> currentPendingActions = this.PendingActions.ToList();//Where(pElem => pElem.IsStillValid(this)).ToList();
                this.PendingActions.Clear();

                foreach (IBoardGameAction actionToResolve in currentPendingActions)
                {
                    actionToResolve.ExecuteAction(this);
                }

                // Update off board cards
                if (this.CardsOffBoard.Count > 0)
                {
                    CardBoardLevel ownerLevel = this.ownerLevelNode as CardBoardLevel;
                    foreach (CardEntity cardEntity in this.CardsOffBoard)
                    {
                        cardEntity.Card.ResetConstellations(this, cardEntity);

                        this.RemoveEntityFromLayer(cardEntity);
                        ownerLevel.GetLayerFromPlayer(cardEntity.Card.CurrentOwner).AddCardToCemetery(cardEntity.Card, cardEntity.Position);
                    }
                    this.CardsOffBoard.Clear();
                }

                // Update awaken states
                HashSet<StarEntity> starModifiedActions = new HashSet<StarEntity>(currentPendingActions.Where(pElem => pElem is IModifyStarEntityAction).Select(pElem => (pElem as IModifyStarEntityAction).OwnerStar));
                if (starModifiedActions.Count > 0)
                {
                    foreach (StarEntity star in this.StarSystem)
                    {
                        if (star.CardSocketed != null)
                        {
                            star.CardSocketed.Card.ReevaluateAwakening(this, star, starModifiedActions);
                        }
                    }
                }

                // Notify all cards of actions been executed.
                if (currentPendingActions.Count > 0)
                {
                    foreach (StarEntity star in this.StarSystem)
                    {
                        if (star.CardSocketed != null)
                        {
                            star.CardSocketed.Card.NotifyActionsOccured(this, star, currentPendingActions);
                        }
                    }
                }

                // Reevaluate domains
                foreach (CJStarDomain domain in this.StarDomains)
                {
                    domain.EvaluateDomainOwner();
                }
            }
        }

        public override void NotifyObjectBeforePropertyChanged(AEntity obj, string propertyName)
        {
            switch (propertyName)
            {
                case "IsAwakened":
                    CardEntity cardEntity = obj as CardEntity;
                    if (cardEntity.Card.IsAwakened)
                    {
                        if (cardEntity.AreCardBehaviorsActive)
                        {
                            this.RegisterNotifBehavior(new CardAwakenedNotifBehavior(cardEntity));
                        }
                    }
                    else
                    {
                        this.UnregisterNotifBehavior(cardEntity);
                    }
                    break;
            }
        }

        public void RegisterNotifBehavior(IBoardNotifBehavior notifBehavior)
        {
            CardBoardLevel ownerLevel = this.ownerLevelNode as CardBoardLevel;
            notifBehavior.NodeLevel = ownerLevel;
            ownerLevel.BoardNotifLayer.NotifBehaviorsList.Add(notifBehavior);
        }

        public void UnregisterNotifBehavior(CardEntity behaviorOwner)
        {
            CardBoardLevel ownerLevel = this.ownerLevelNode as CardBoardLevel;
            ownerLevel.BoardNotifLayer.NotifBehaviorsList.RemoveAll(pElem => pElem.OwnerCardEntity == behaviorOwner);
        }

        public void MoveCardOverBoard(CardEntity cardEntity, Vector2f positionToMove)
        {
            if(this.CardEntityPicked != null
                && this.CardEntityPicked == cardEntity)
            {
                this.CardEntityPicked.Position = positionToMove;
            }
        }

        protected override void InternalInitializeLayer(World world, ALevelNode levelNode)
        {
            this.cardPicked = null;
            this.cardFocused = null;
            this.domainFocused = null;

            this.playerTurn = null;

            this.nbCardsToPlace = 0;

            this.StarSystem.Clear();
            this.StarLinkSystem.Clear();
            this.StarToLinks.Clear();
            this.StarDomains.Clear();

            //this.DeactivatedCardEntities.Clear();
            this.CardsOffBoard.Clear();
            this.PendingActions.Clear();
            this.NameToOnBoardCardEntities.Clear();

            GalaxyFactory.NameToGalaxyCreators[this.PreInitGalaxyName](this);

            FloatRect galaxyArea = this.GetGalaxyArea();
            (levelNode as CardBoardLevel).BoardBackgroundLayer.Position = new Vector2f(galaxyArea.Left + galaxyArea.Width / 2f, galaxyArea.Top + galaxyArea.Height / 2f) * (1 / 0.75f);
            (levelNode as CardBoardLevel).BoardBackgroundLayer.StartingArea = new Vector2f(galaxyArea.Width + 1000, galaxyArea.Height + 1000);
        }

        protected FloatRect GetGalaxyArea()
        {
            FloatRect galaxyArea = new FloatRect(float.MaxValue, float.MaxValue, float.MinValue, float.MinValue);
            foreach (StarEntity starEntity in this.StarSystem)
            {
                if(starEntity.Position.X < galaxyArea.Left)
                {
                    galaxyArea.Left = starEntity.Position.X;
                }
                if (starEntity.Position.X > galaxyArea.Width)
                {
                    galaxyArea.Width = starEntity.Position.X;
                }

                if (starEntity.Position.Y < galaxyArea.Top)
                {
                    galaxyArea.Top = starEntity.Position.Y;
                }
                if (starEntity.Position.Y > galaxyArea.Height)
                {
                    galaxyArea.Height = starEntity.Position.Y;
                }
            }

            galaxyArea.Width -= galaxyArea.Left;
            galaxyArea.Height -= galaxyArea.Top;

            return galaxyArea;
        }
    }
}
