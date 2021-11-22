using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.AI.AICard;
using Astrategia.Model;
using Astrategia.Model.Card;
using Astrategia.Model.Layer.BoardGameLayer;
using SFML.System;

namespace Astrategia.AI.AIBoardGameLayer
{
    public class AIBoardGameLayer : AAILayer
    {
        private List<AICardEntity> sourceCardEntities;
        private List<AIStarEntity> sourceStarEntities;
        private List<AICardEntity> targetCardEntities;
        private List<AIStarEntity> targetStarEntities;

        private AICardEntity cardEntityPicked;

        private int nbCardsToPlace;

        public Dictionary<AIStarEntity, HashSet<AIStarLinkEntity>> StarToLinks
        {
            get;
            private set;
        }

        public HashSet<AIStarEntity> StarSystem
        {
            get;
            protected set;
        }

        public HashSet<AIStarLinkEntity> StarLinkSystem
        {
            get;
            protected set;
        }

        public HashSet<AICJStarDomain> StarDomainSystem
        {
            get;
            protected set;
        }

        public AIBoardGameLayer(AIWorld world2D, IAIObjectFactory layerFactory, BoardGameLayer layer) 
            : base(world2D, layerFactory, layer)
        {
            this.StarToLinks = new Dictionary<AIStarEntity, HashSet<AIStarLinkEntity>>();

            this.StarSystem = new HashSet<AIStarEntity>();

            this.StarLinkSystem = new HashSet<AIStarLinkEntity>();

            this.StarDomainSystem = new HashSet<AICJStarDomain>();

            layer.CardPicked += this.OnCardPicked;
            //layer.CardUnpicked += this.OnCardUnPicked;

            layer.SourceStarEntitiesSet += OnSourceStarEntitiesSet;
            layer.TargetStarEntitiesSet += OnTargetStarEntitiesSet;

            layer.NbCardsToPlaceChanged += OnNbCardsToPlaceChanged;
        }

        public override void InitializeLayer(IAIObjectFactory factory)
        {
            this.StarToLinks.Clear();
            this.StarSystem.Clear();
            this.StarLinkSystem.Clear();
            this.StarDomainSystem.Clear();

            this.sourceCardEntities = null;
            this.sourceStarEntities = null;
            this.targetCardEntities = null;
            this.targetStarEntities = null;

            this.cardEntityPicked = null;

            this.nbCardsToPlace = 0;

            base.InitializeLayer(factory);

            foreach(AIStarLinkEntity starLinkEntity in this.StarLinkSystem)
            {
                starLinkEntity.UpdateReference();

                HashSet<AIStarLinkEntity> linksConnectedTo = null;
                if (this.StarToLinks.TryGetValue(starLinkEntity.StarFrom, out linksConnectedTo) == false)
                {
                    linksConnectedTo = new HashSet<AIStarLinkEntity>();

                    this.StarToLinks.Add(starLinkEntity.StarFrom, linksConnectedTo);
                }
                linksConnectedTo.Add(starLinkEntity);

                linksConnectedTo = null;
                if (this.StarToLinks.TryGetValue(starLinkEntity.StarTo, out linksConnectedTo) == false)
                {
                    linksConnectedTo = new HashSet<AIStarLinkEntity>();

                    this.StarToLinks.Add(starLinkEntity.StarTo, linksConnectedTo);
                }
                linksConnectedTo.Add(starLinkEntity);
            }

            foreach(AICJStarDomain starDomainAI in this.StarDomainSystem)
            {
                starDomainAI.UpdateReference();
            }
        }

        public override void SendInfluence(string influence, AAIEntity entityConcernedAI)
        {
            lock (this.objectLock)
            {
                if (this.objectAIToObjects.TryGetValue(entityConcernedAI, out AEntity entityConcerned))
                {
                    switch (influence)
                    {
                        case "socketCard":
                            this.SendEventToWorld(Model.Event.EventType.SOCKET_CARD, entityConcerned, null);
                            break;
                    }
                }
            }
        }

        private void OnNbCardsToPlaceChanged(int obj)
        {
            lock (this.objectLock)
            {
                this.nbCardsToPlace = obj;
            }
        }

        private void OnCardPicked(CardEntity obj)
        {
            lock (this.objectLock)
            {
                if (obj != null)
                {
                    this.cardEntityPicked = this.objectToObjectAIs[obj] as AICardEntity;
                }
                else
                {
                    this.cardEntityPicked = null;
                }
            }
        }

        private void OnSourceStarEntitiesSet(List<StarEntity> obj)
        {
            lock (this.objectLock)
            {
                this.targetStarEntities = null;
                this.targetCardEntities = null;

                if (obj != null && obj.Count > 0)
                {
                    this.sourceStarEntities = obj.Select(pElem => this.objectToObjectAIs[pElem] as AIStarEntity).ToList();
                    this.sourceCardEntities = obj.Select(pElem => this.objectToObjectAIs[pElem.CardSocketed] as AICardEntity).ToList();
                }
                else
                {
                    this.sourceStarEntities = null;
                    this.sourceCardEntities = null;
                }
            }
        }

        private void OnTargetStarEntitiesSet(List<StarEntity> obj)
        {
            lock (this.objectLock)
            {
                this.sourceCardEntities = null;

                if (obj != null && obj.Count > 0)
                {
                    this.sourceStarEntities = obj.Select(pElem => this.objectToObjectAIs[pElem] as AIStarEntity).ToList();

                    if (obj.First().CardSocketed != null)
                    {
                        this.targetStarEntities = null;
                        this.targetCardEntities = obj.Select(pElem => this.objectToObjectAIs[pElem.CardSocketed] as AICardEntity).ToList();
                    }
                    else
                    {
                        this.targetCardEntities = null;
                        this.targetStarEntities = this.sourceStarEntities;
                    }

                }
                else
                {
                    this.sourceStarEntities = null;
                    this.targetStarEntities = null;
                }
            }
        }

        protected override void OnEntityPropertyChanged(AEntity obj, string propertyName)
        {
            base.OnEntityPropertyChanged(obj, propertyName);

            AAIEntity entityAI;

            lock (this.objectLock)
            {
                switch (propertyName)
                {
                    case "CardSocketed":
                        StarEntity starEntity = obj as StarEntity;
                        AIStarEntity starEntityAI = this.objectToObjectAIs[obj] as AIStarEntity;

                        lock (starEntityAI.objectLock)
                        {
                            if (starEntity.CardSocketed != null)
                            {
                                starEntityAI.CardEntitySocketed = this.objectToObjectAIs[starEntity.CardSocketed] as AICardEntity;
                            }
                            else
                            {
                                starEntityAI.CardEntitySocketed = null;
                            }
                        }
                        break;
                    case "IsSocketed":
                        if (this.objectToObjectAIs.TryGetValue(obj, out entityAI))
                        {
                            lock (entityAI.objectLock)
                            {
                                (entityAI as AICardEntity).IsSocketed = (obj as CardEntity).ParentStar != null;
                            }
                        }
                        break;
                    case "IsFliped":
                        if (this.objectToObjectAIs.TryGetValue(obj, out entityAI))
                        {
                            lock (entityAI.objectLock)
                            {
                                (entityAI as AICardEntity).IsFliped = (obj as CardEntity).IsFliped;
                            }
                        }
                        break;
                    case "IsSelected":
                        if (this.objectToObjectAIs.TryGetValue(obj, out entityAI))
                        {
                            lock (entityAI.objectLock)
                            {
                                (entityAI as AICardEntity).IsSelected = (obj as CardEntity).IsSelected;
                            }
                        }
                        break;
                    case "IsAwakened":
                        if (this.objectToObjectAIs.TryGetValue(obj, out entityAI))
                        {
                            lock (entityAI.objectLock)
                            {
                                (entityAI as AICardEntity).IsAwakened = (obj as CardEntity).Card.IsAwakened;
                            }
                        }
                        break;
                    case "CurrentOwner":
                        if (this.objectToObjectAIs.TryGetValue(obj, out entityAI))
                        {
                            lock (entityAI.objectLock)
                            {
                                (entityAI as AICardEntity).CardOwnerName = (obj as CardEntity).Card.CurrentOwner.PlayerName;
                            }
                        }
                        break;
                    case "Value":
                        if (this.objectToObjectAIs.TryGetValue(obj, out entityAI))
                        {
                            lock (entityAI.objectLock)
                            {
                                (entityAI as AICardEntity).Value = (obj as CardEntity).Card.Value;
                            }
                        }
                        break;
                    case "DomainOwner":
                        CJStarDomain starDomain = obj as CJStarDomain;
                        if (this.objectToObjectAIs.TryGetValue(obj, out entityAI))
                        {
                            lock (entityAI.objectLock)
                            {
                                (entityAI as AICJStarDomain).DomainOwnerName = starDomain.DomainOwner != null ? starDomain.DomainOwner.PlayerName : null;
                            }
                        }
                        break;

                }
            }
        }

        protected override AAIEntity AddEntity(AEntity obj)
        {
            AAIEntity entityAdded = base.AddEntity(obj);

            if(entityAdded != null)
            {
                if(entityAdded is AIStarEntity)
                {
                    this.StarSystem.Add(entityAdded as AIStarEntity);
                }
                else if(entityAdded is AIStarLinkEntity)
                {
                    this.StarLinkSystem.Add(entityAdded as AIStarLinkEntity);
                }
                else if(entityAdded is AICJStarDomain)
                {
                    this.StarDomainSystem.Add(entityAdded as AICJStarDomain);
                }
            }

            return entityAdded;
        }

        public override void UpdateAI(Time deltaTime)
        {
            
        }

        public override void FlushEntities()
        {
            base.FlushEntities();

            //this.cardsOnBoard.Clear();
            //this.domainsOwnedByPlayers.Clear();

            //this.LevelTurnPhase = TurnPhase.VOID;
        }

        public override void Dispose()
        {
            (this.parentLayer as BoardGameLayer).CardPicked -= this.OnCardPicked;
            //(this.parentLayer as BoardGameLayer).CardUnpicked -= this.OnCardUnPicked;

            (this.parentLayer as BoardGameLayer).SourceStarEntitiesSet -= OnSourceStarEntitiesSet;
            (this.parentLayer as BoardGameLayer).TargetStarEntitiesSet -= OnTargetStarEntitiesSet;

            (this.parentLayer as BoardGameLayer).NbCardsToPlaceChanged -= OnNbCardsToPlaceChanged;

            base.Dispose();
        }
    }
}
