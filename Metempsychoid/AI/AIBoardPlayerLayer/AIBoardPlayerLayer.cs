using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.AI.AICard;
using Astrategia.Model;
using Astrategia.Model.Card;
using Astrategia.Model.Layer.BoardPlayerLayer;

namespace Astrategia.AI.AIBoardPlayerLayer
{
    public class AIBoardPlayerLayer : AAILayer
    {
        private List<AICardEntity> cardsDeck;
        private List<AICardEntity> cardsCemetery;
        private List<AICardEntity> cardsHand;

        private List<AICardEntity> sourceCardEntities;

        private AICardEntity cardDrawn;
        private AICardEntity cardFocused;

        private BoardPlayerLayer.PileFocused pileFocused;

        private int nbCardsToDraw;

        public BoardPlayerLayer.PileFocused PileFocused
        {
            get
            {
                return this.pileFocused;
            }

            set
            {
                if (this.pileFocused != value)
                {
                    this.pileFocused = value;
                }
            }
        }

        public AIBoardPlayerLayer(AIWorld world2D, IAIObjectFactory layerFactory, BoardPlayerLayer layer) 
            : base(world2D, layerFactory, layer)
        {
            this.cardsDeck = new List<AICardEntity>();
            this.cardsCemetery = new List<AICardEntity>();
            this.cardsHand = new List<AICardEntity>();

            layer.CardDrawn += OnCardDrawn;
            layer.NbCardsToDrawChanged += OnNbCardToDrawsChanged;

            layer.CardFocused += OnCardFocused;

            layer.CardPicked += OnCardPicked;
            layer.CardUnpicked += OnCardUnpicked;

            layer.PileFocusedChanged += OnPileFocusedChanged;

            layer.CardDestroyed += OnCardDestroyed;
            // layer.CardResurrected += OnCardResurrected;

            layer.SourceCardEntitiesSet += OnSourceCardEntitiesSet;
        }

        public override void SendInfluence(string influence, AAIEntity entityConcernedAI)
        {
            lock (this.objectLock)
            {
                if (this.objectAIToObjects.TryGetValue(entityConcernedAI, out AEntity entityConcerned))
                {
                    switch (influence)
                    {
                        case "pickCard":
                            this.pendingGameEvent.Enqueue(new GameEventContainer(Model.Event.EventType.PICK_CARD, entityConcerned, 0 + ":" + 0));
                            break;
                    }
                }
            }
        }

        public override void InitializeLayer(IAIObjectFactory factory)
        {
            this.cardsDeck.Clear();
            this.cardsCemetery.Clear();
            this.cardsHand.Clear();

            this.pileFocused = (this.parentLayer as BoardPlayerLayer).CardPileFocused;

            base.InitializeLayer(factory);

            foreach (AAIEntity entity in this.objectToObjectAIs.Values)
            {
                if (entity is AICardEntity)
                {
                    AICardEntity cardEntity2D = entity as AICardEntity;

                    this.cardsDeck.Add(cardEntity2D);
                }
            }
        }

        private void OnSourceCardEntitiesSet(List<CardEntity> obj)
        {
            lock (this.objectLock)
            {
                if (obj != null && obj.Count > 0)
                {
                    this.sourceCardEntities = obj.Select(pElem => this.objectToObjectAIs[pElem] as AICardEntity).ToList();
                }
                else
                {
                    this.sourceCardEntities = null;
                }
            }
        }

        private void OnNbCardToDrawsChanged(int obj)
        {
            lock (this.objectLock)
            {
                this.nbCardsToDraw = obj;
            }
        }

        private void OnCardFocused(CardEntity obj)
        {
            lock (this.objectLock)
            {
                if (obj != null)
                {
                    AICardEntity cardFocused = this.objectToObjectAIs[obj] as AICardEntity;

                    this.cardFocused = cardFocused;
                    //this.cardToolTip.DisplayToolTip(obj.Card, cardFocused);
                }
                else
                {
                    this.cardFocused = null;
                    //this.cardToolTip.HideToolTip();
                }
            }
        }

        private void OnCardDrawn(CardEntity obj)
        {
            lock (this.objectLock)
            {
                this.cardDrawn = this.objectToObjectAIs[obj] as AICardEntity;

                this.cardsDeck.Remove(this.cardDrawn);
                this.cardsHand.Add(this.cardDrawn);
            }
        }

        private void OnCardPicked(CardEntity obj, BoardPlayerLayer.PileFocused pilePicked)
        {
            lock (this.objectLock)
            {
                AICardEntity cardPicked = this.objectToObjectAIs[obj] as AICardEntity;

                switch (pilePicked)
                {
                    case BoardPlayerLayer.PileFocused.HAND:
                        this.cardsHand.Remove(cardPicked);
                        break;
                    case BoardPlayerLayer.PileFocused.CEMETERY:
                        this.cardsCemetery.Remove(cardPicked);
                        break;
                }
            }
        }

        private void OnCardUnpicked(CardEntity obj, BoardPlayerLayer.PileFocused pilePicked)
        {
            lock (this.objectLock)
            {
                AICardEntity cardUnpicked = this.objectToObjectAIs[obj] as AICardEntity;

                switch (pilePicked)
                {
                    case BoardPlayerLayer.PileFocused.HAND:
                        this.cardsHand.Add(cardUnpicked);
                        break;
                    case BoardPlayerLayer.PileFocused.CEMETERY:
                        this.cardsCemetery.Add(cardUnpicked);
                        break;
                }
            }
        }

        //private void OnCardResurrected(CardEntity obj)
        //{
        //    this.cardsCemetery.Remove(this.GetEntity2DFromEntity(obj) as CardEntity2D);

        //    this.UpdateCardCimeteryPriority();
        //}

        private void OnCardDestroyed(CardEntity obj)
        {
            lock (this.objectLock)
            {
                AICardEntity cardDestroyed = this.objectToObjectAIs[obj] as AICardEntity;

                this.cardsCemetery.Add(cardDestroyed);
            }
        }

        private void OnPileFocusedChanged(BoardPlayerLayer.PileFocused newPileFocused)
        {
            lock (this.objectLock)
            {
                this.PileFocused = newPileFocused;
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
                    case "IsFliped":
                        if (this.objectToObjectAIs.TryGetValue(obj, out entityAI))
                        {
                            lock (entityAI.objectLock)
                            {
                                (entityAI as AICardEntity).IsFliped = (obj as CardEntity).IsFliped;
                            }
                        }
                        break;
                }
            }
        }

        public override void FlushEntities()
        {
            base.FlushEntities();

            this.cardsDeck.Clear();
            this.cardsCemetery.Clear();
            this.cardsHand.Clear();

            this.nbCardsToDraw = 0;

            //this.LevelTurnPhase = TurnPhase.VOID;
            this.cardDrawn = null;
        }

        public override void Dispose()
        {
            (this.parentLayer as BoardPlayerLayer).CardDrawn -= OnCardDrawn;
            (this.parentLayer as BoardPlayerLayer).NbCardsToDrawChanged -= OnNbCardToDrawsChanged;

            (this.parentLayer as BoardPlayerLayer).CardFocused -= OnCardFocused;

            (this.parentLayer as BoardPlayerLayer).CardPicked -= OnCardPicked;
            (this.parentLayer as BoardPlayerLayer).CardUnpicked -= OnCardUnpicked;

            (this.parentLayer as BoardPlayerLayer).PileFocusedChanged -= OnPileFocusedChanged;

            (this.parentLayer as BoardPlayerLayer).CardDestroyed -= OnCardDestroyed;
            // (this.parentLayer as BoardPlayerLayer).CardResurrected -= OnCardResurrected;

            base.Dispose();
        }
    }
}
