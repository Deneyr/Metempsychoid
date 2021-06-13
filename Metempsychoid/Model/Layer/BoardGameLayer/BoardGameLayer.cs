using Metempsychoid.Animation;
using Metempsychoid.Model.Animation;
using Metempsychoid.Model.Card;
using Metempsychoid.Model.Layer.BoardGameLayer.Actions;
using Metempsychoid.Model.Layer.BoardNotifLayer;
using Metempsychoid.Model.Layer.BoardNotifLayer.Behavior;
using Metempsychoid.Model.Node;
using Metempsychoid.Model.Node.TestWorld;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardGameLayer
{
    public class BoardGameLayer: EntityLayer.EntityLayer
    {
        private CardEntity cardFocused;

        private CardEntity cardPicked;

        private Player.Player playerTurn;

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

        public int NbCardsAbleToBeSocketed
        {
            get;
            private set;
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

                    this.NbCardsAbleToBeSocketed = 100;
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

                    this.NotifyCardPicked(this.cardPicked);
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

                        this.NotifyCardFocused(this.cardFocused);
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

        public event Action<List<StarEntity>> SourceStarEntitiesSet;
        public event Action<List<StarEntity>> TargetStarEntitiesSet;

        public BoardGameLayer()
        {
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

        protected void AddStar(StarEntity starEntity)
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

        protected StarLinkEntity AddStarLink(StarEntity starEntityFrom, StarEntity starEntityTo)
        {
            StarLinkEntity link = new StarLinkEntity(this, starEntityFrom, starEntityTo);

            this.StarLinkSystem.Add(link);
            this.AddEntityToLayer(link);

            this.StarToLinks[starEntityFrom].Add(link);
            this.StarToLinks[starEntityTo].Add(link);

            return link;
        }

        protected StarLinkEntity AddCurvedStarLink(StarEntity starEntityFrom, StarEntity starEntityTo, float radius)
        {
            StarLinkEntity link = new CurvedStarLinkEntity(this, starEntityFrom, starEntityTo, radius);

            this.StarLinkSystem.Add(link);
            this.AddEntityToLayer(link);

            this.StarToLinks[starEntityFrom].Add(link);
            this.StarToLinks[starEntityTo].Add(link);

            return link;
        }

        protected void AddStarDomain(CJStarDomain domainToAdd)
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

        public void SetBehaviorTargetStarEntities(List<StarEntity> sourceStarEntities)
        {
            this.BehaviorTargetStarEntities = new List<StarEntity>(sourceStarEntities);

            this.TargetStarEntitiesSet?.Invoke(this.BehaviorTargetStarEntities);
        }

        public bool PickCard(Card.Card card)
        {
            if (this.CardEntityPicked == null)
            {
                CardEntity cardEntity = new CardEntity(this, card, true);

                this.AddEntityToLayer(cardEntity);

                this.CardEntityPicked = cardEntity;

                return true;
            }
            return false;
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
                    IAnimation positionAnimation;
                    positionAnimation = new PositionAnimation(cardEntity.Position, cardEntity.ParentStar.Position, Time.FromSeconds(1f), AnimationType.ONETIME, InterpolationMethod.SQUARE_DEC);

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
            if(this.CardEntityPicked.ParentStar != starEntity)
            {
                this.PendingActions.Add(new UnsocketCardAction(this.CardEntityPicked, false));
                this.PendingActions.Add(new SocketCardAction(this.CardEntityPicked, starEntity));

                this.CardEntityPicked = null;
            }
        }

        public void SocketCard(StarEntity starEntity) //, Vector2f positionInNotifBoard)
        {
            if(this.CardEntityPicked != null && this.NbCardsAbleToBeSocketed > 0)
            {
                this.CardEntityPicked.IsFliped = true;

                this.NbCardsAbleToBeSocketed--;

                this.PendingActions.Add(new SocketCardAction(this.CardEntityPicked, starEntity));
            }
        }

        public void GetCardFromBoard(CardEntity cardEntity)
        {
            //cardEntity.IsActive = false;
        }

        public void ReturnCardToBoard(CardEntityDecorator cardToReturn)
        {
            cardToReturn.CardEntityDecorated.IsActive = true;
        }

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
            BoardPlayerLayer.BoardPlayerLayer playerLayer = (this.ownerLevelNode as TestLevel).BoardplayersList.FirstOrDefault(pElem => pElem.SupportedPlayer != player);

            return playerLayer.CardsCemetery.Count;
        }

        public int GetNbDeadCard(Player.Player player)
        {
            BoardPlayerLayer.BoardPlayerLayer playerLayer = (this.ownerLevelNode as TestLevel).BoardplayersList.FirstOrDefault(pElem => pElem.SupportedPlayer == player);

            return playerLayer.CardsCemetery.Count;
        }

        private void UpdateBoard()
        {
            if (this.PendingActions.Count > 0)
            {
                // Clear the actions pending list to allow it to be fill by the next generation of events raised by card awakening ...
                List<IBoardGameAction> currentPendingActions = this.PendingActions.ToList();
                this.PendingActions.Clear();

                foreach (IBoardGameAction actionToResolve in currentPendingActions)
                {
                    actionToResolve.ExecuteAction(this);
                }

                HashSet<StarEntity> starModifiedActions = new HashSet<StarEntity>(currentPendingActions.Where(pElem => pElem is IModifyStarEntityAction).Select(pElem => (pElem as IModifyStarEntityAction).OwnerStar));
                // Update awaken states
                if (this.CardsOffBoard.Count > 0)
                {
                    foreach (CardEntity cardEntity in this.CardsOffBoard)
                    {
                        cardEntity.Card.ResetConstellations();
                    }
                    this.CardsOffBoard.Clear();
                }

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
                        this.RegisterNotifBehavior(new CardAwakenedNotifBehavior(cardEntity));
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
            TestLevel ownerLevel = this.ownerLevelNode as TestLevel;
            notifBehavior.NodeLevel = ownerLevel;
            ownerLevel.BoardNotifLayer.NotifBehaviorsList.Add(notifBehavior);
        }

        public void UnregisterNotifBehavior(CardEntity behaviorOwner)
        {
            TestLevel ownerLevel = this.ownerLevelNode as TestLevel;
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

            this.playerTurn = null;

            this.StarSystem.Clear();
            this.StarLinkSystem.Clear();
            this.StarToLinks.Clear();
            this.StarDomains.Clear();

            //this.DeactivatedCardEntities.Clear();
            this.CardsOffBoard.Clear();
            this.PendingActions.Clear();
            this.NameToOnBoardCardEntities.Clear();

            float cosPi4 = (float) Math.Cos(Math.PI / 4);

            // Inner circle
            StarEntity star = new StarEntity(this);
            this.AddStar(star);


            StarEntity star1 = new StarEntity(this);
            star1.Position = new Vector2f(-400 * cosPi4, -400 * cosPi4);
            this.AddStar(star1);

            StarEntity star2 = new StarEntity(this);
            star2.Position = new Vector2f(0, -400);
            this.AddStar(star2);

            StarEntity star3 = new StarEntity(this);
            star3.Position = new Vector2f(400 * cosPi4, -400 * cosPi4);
            this.AddStar(star3);


            StarEntity star4 = new StarEntity(this);
            star4.Position = new Vector2f(400 * cosPi4, 400 * cosPi4);
            this.AddStar(star4);

            StarEntity star5 = new StarEntity(this);
            star5.Position = new Vector2f(0, 400);
            this.AddStar(star5);

            StarEntity star6 = new StarEntity(this);
            star6.Position = new Vector2f(-400 * cosPi4, 400 * cosPi4);
            this.AddStar(star6);

            this.AddStarLink(star, star2);
            this.AddStarLink(star, star5);
            this.AddCurvedStarLink(star1, star2, 400);
            this.AddCurvedStarLink(star2, star3, 400);
            this.AddCurvedStarLink(star4, star5, 400);
            this.AddCurvedStarLink(star5, star6, 400);

            // Circle
            StarEntity star7 = new StarEntity(this);
            star7.Position = new Vector2f(-800, 0);
            this.AddStar(star7);

            StarEntity star8 = new StarEntity(this);
            star8.Position = new Vector2f(0, -800);
            this.AddStar(star8);

            StarEntity star9 = new StarEntity(this);
            star9.Name = "in_star_right";
            star9.Position = new Vector2f(800, 0);
            this.AddStar(star9);

            StarEntity star10 = new StarEntity(this);
            star10.Position = new Vector2f(0, 800);
            this.AddStar(star10);

            this.AddStarLink(star, star7);
            this.AddStarLink(star, star9);
            this.AddStarLink(star1, star7);
            this.AddStarLink(star6, star7);
            this.AddStarLink(star3, star9);
            this.AddStarLink(star4, star9);
            this.AddStarLink(star1, star8);
            this.AddStarLink(star3, star8);
            this.AddStarLink(star4, star10);
            this.AddStarLink(star6, star10);
            this.AddCurvedStarLink(star7, star8, 800);
            this.AddCurvedStarLink(star8, star9, 800);
            this.AddCurvedStarLink(star9, star10, 800);
            this.AddCurvedStarLink(star10, star7, 800);

            // out circle
            StarEntity star11 = new StarEntity(this);
            star11.Name = "out_star_left";
            star11.Position = new Vector2f(-1200, 0);
            this.AddStar(star11);

            StarEntity star12 = new StarEntity(this);
            star12.Position = new Vector2f(0, -1200);
            this.AddStar(star12);

            StarEntity star13 = new StarEntity(this);
            star13.Name = "out_star_right";
            star13.Position = new Vector2f(1200, 0);
            this.AddStar(star13);

            StarEntity star14 = new StarEntity(this);
            star14.Name = "out_star_bot";
            star14.Position = new Vector2f(0, 1200);
            this.AddStar(star14);

            this.AddStarLink(star7, star11);
            this.AddStarLink(star9, star13);
            this.AddCurvedStarLink(star11, star12, 1200);
            this.AddCurvedStarLink(star12, star13, 1200);
            this.AddCurvedStarLink(star13, star14, 1200);
            this.AddCurvedStarLink(star14, star11, 1200);

            // Star Domains
            CJStarDomain domain1 = new CJStarDomain(this, new HashSet<StarEntity>()
            {
                star1,
                star2,
                star3,
                star
            }, -1);
            this.AddStarDomain(domain1);

            CJStarDomain domain2 = new CJStarDomain(this, new HashSet<StarEntity>()
            {
                star7,
                star8,
                star9,
                star10
            }, -2, false);
            this.AddStarDomain(domain2);
        }

        protected void NotifyCardPicked(CardEntity cardPicked)
        {
            this.CardPicked?.Invoke(cardPicked);
        }

        protected void NotifyCardFocused(CardEntity cardFocused)
        {
            this.CardFocused?.Invoke(cardFocused);
        }
    }
}
