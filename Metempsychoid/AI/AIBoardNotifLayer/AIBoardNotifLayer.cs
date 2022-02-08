using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.AI.AICard;
using Astrategia.Model;
using Astrategia.Model.Layer.BoardNotifLayer;
using Astrategia.Model.Layer.BoardNotifLayer.Behavior;

namespace Astrategia.AI.AIBoardNotifLayer
{
    public class AIBoardNotifLayer : AAILayer
    {
        public List<AICardEntity> CardsHand
        {
            get;
            private set;
        }

        public AICardEntity CardFocused
        {
            get;
            private set;
        }

        public AICardEntity CardAwakened
        {
            get;
            private set;
        }

        public ActiveCardBehavior CurrentActiveBehavior
        {
            get;
            private set;
        }

        public AIBoardNotifLayer(AIWorld world2D, IAIObjectFactory layerFactory, BoardNotifLayer layer) 
            : base(world2D, layerFactory, layer)
        {
            this.CardsHand = new List<AICardEntity>();

            layer.CardCreated += OnCardCreated;
            layer.CardRemoved += OnCardRemoved;

            layer.CardPicked += OnCardPicked;
            layer.CardUnpicked += OnCardUnpicked;

            layer.CardFocused += OnCardFocused;

            layer.CardAwakened += OnCardAwakened;

            layer.NotifBehaviorStarted += OnNotifBehaviorStarted;
            layer.NotifBehaviorPhaseChanged += OnNotifBehaviorPhaseChanged;
            layer.NotifBehaviorUseChanged += OnNotifBehaviorUseChanged;
            layer.NotifBehaviorEnded += OnNotifBehaviorEnded;
        }

        public override void InitializeLayer(IAIObjectFactory factory)
        {
            base.InitializeLayer(factory);

            this.CardFocused = null;
            this.CardAwakened = null;

            this.CurrentActiveBehavior = null;
        }

        private void OnCardFocused(Model.Card.CardEntity obj)
        {
            lock (this.objectLock)
            {
                if (obj != null)
                {
                    AICardEntity cardFocused = this.GetAIEntityFromEntity(obj) as AICardEntity;

                    this.CardFocused = cardFocused;
                    //this.cardToolTip.DisplayToolTip(obj.Card, cardFocused);
                }
                else
                {
                    this.CardFocused = null;
                    //this.cardToolTip.HideToolTip();
                }
            }
        }

        private void OnCardAwakened(CardEntityAwakenedDecorator obj)
        {
            lock (this.objectLock)
            {
                if (obj != null)
                {
                    this.CardAwakened = this.objectToObjectAIs[obj] as AICardEntity;
                }
                else
                {
                    this.CardAwakened = null;
                }
            }
        }

        private void OnCardPicked(Model.Card.CardEntity obj)
        {
            lock (this.objectLock)
            {
                this.CardsHand.Remove(this.GetAIEntityFromEntity(obj) as AICardEntity);
            }
        }

        private void OnCardUnpicked(Model.Card.CardEntity obj)
        {
            lock (this.objectLock)
            {
                AICardEntity cardPicked = this.GetAIEntityFromEntity(obj) as AICardEntity;

                this.CardsHand.Add(cardPicked);
            }
        }

        private void OnCardCreated(Model.Card.CardEntity obj)
        {
            lock (this.objectLock)
            {
                AICardEntity cardEntity = this.GetAIEntityFromEntity(obj) as AICardEntity;

                this.CardsHand.Add(cardEntity);
            }
        }

        private void OnCardRemoved(Model.Card.CardEntity obj)
        {
            lock (this.objectLock)
            {
                AICardEntity cardEntity2DToRemove = this.GetAIEntityFromEntity(obj) as AICardEntity;

                this.CardsHand.Remove(cardEntity2DToRemove);
            }
        }

        protected virtual void OnNotifBehaviorStarted(IBoardNotifBehavior obj)
        {
            lock (this.objectLock)
            {
                if (obj.IsThereBehaviorLabel)
                {
                    ACardNotifBehavior cardNotifBehavior = obj as ACardNotifBehavior;

                    this.CurrentActiveBehavior = new ActiveCardBehavior(cardNotifBehavior.GetType(), cardNotifBehavior.NbBehaviorUse, cardNotifBehavior.StateValue, cardNotifBehavior.OwnerCardEntity.Card.Name);
                }
            }
        }

        private void OnNotifBehaviorUseChanged(int obj)
        {
            lock (this.objectLock)
            {
                if (this.CurrentActiveBehavior != null)
                {
                    this.CurrentActiveBehavior.NbUses = obj;
                }
            }
        }

        private void OnNotifBehaviorPhaseChanged(string obj)
        {
            lock (this.objectLock)
            {
                if (this.CurrentActiveBehavior != null)
                {
                    this.CurrentActiveBehavior.CurrentPhase = obj;
                }
            }
        }

        protected virtual void OnNotifBehaviorEnded(IBoardNotifBehavior obj)
        {
            lock (this.objectLock)
            {
                this.CurrentActiveBehavior = null;
            }
        }

        public override void Dispose()
        {
            (this.parentLayer as BoardNotifLayer).CardCreated -= OnCardCreated;
            (this.parentLayer as BoardNotifLayer).CardRemoved -= OnCardRemoved;

            (this.parentLayer as BoardNotifLayer).CardPicked -= OnCardPicked;
            (this.parentLayer as BoardNotifLayer).CardUnpicked -= OnCardUnpicked;

            (this.parentLayer as BoardNotifLayer).CardFocused -= OnCardFocused;

            (this.parentLayer as BoardNotifLayer).CardAwakened -= OnCardAwakened;

            (this.parentLayer as BoardNotifLayer).NotifBehaviorStarted -= OnNotifBehaviorStarted;
            (this.parentLayer as BoardNotifLayer).NotifBehaviorPhaseChanged -= OnNotifBehaviorPhaseChanged;
            (this.parentLayer as BoardNotifLayer).NotifBehaviorEnded -= OnNotifBehaviorEnded;

            base.Dispose();
        }
    }
}
