using Metempsychoid.Animation;
using Metempsychoid.Model;
using Metempsychoid.Model.Animation;
using Metempsychoid.Model.Card;
using Metempsychoid.Model.Layer.BoardPlayerLayer;
using Metempsychoid.Model.Node.TestWorld;
using Metempsychoid.View.Card2D;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardPlayerLayer2D
{
    public class BoardPlayerLayer2D: ALayer2D
    {
        private List<CardEntity2D> cardsDeck;

        private List<CardEntity2D> cardsCemetery;

        private List<CardEntity2D> cardsHand;

        private CardEntity2D cardDrew;

        private CardEntity2D cardFocused;

        private int maxPriority;

        private int nbCardsToDraw;

        private TurnPhase levelTurnPhase;

        public TurnPhase LevelTurnPhase
        {
            get
            {
                return this.levelTurnPhase;
            }
            private set
            {
                if(this.levelTurnPhase != value)
                {
                    this.levelTurnPhase = value;

                    switch (this.levelTurnPhase)
                    {
                        case TurnPhase.CREATE_HAND:
                            this.cardDrew = null;
                            break;
                    }
                }
            }
        }

        public BoardPlayerLayer2D(World2D world2D, IObject2DFactory factory, BoardPlayerLayer layer) :
            base(world2D, layer)
        {
            this.Area = new Vector2i(int.MaxValue, int.MaxValue);

            (this.parentLayer as BoardPlayerLayer).CardDrew += OnCardDrew;
            (this.parentLayer as BoardPlayerLayer).NbCardsToDrawChanged += OnNbCardToDrawsChanged;
        }

        public override void InitializeLayer(IObject2DFactory factory)
        {
            this.cardsDeck = new List<CardEntity2D>();

            this.cardsCemetery = new List<CardEntity2D>();

            this.cardsHand = new List<CardEntity2D>();

            this.maxPriority = 0;

            this.nbCardsToDraw = 0;

            this.LevelTurnPhase = TurnPhase.VOID;
            this.cardDrew = null;
            this.cardFocused = null;

            base.InitializeLayer(factory);
        }

        private void OnNbCardToDrawsChanged(int obj)
        {
            this.nbCardsToDraw = obj;
        }

        private void OnCardDrew(AEntity obj)
        {
            this.cardDrew = this.GetEntity2DFromEntity(obj) as CardEntity2D;

            this.cardsDeck.Remove(this.cardDrew);
            this.cardsHand.Add(this.cardDrew);

            this.UpdateCardEntitiesPriority();
        }

        private void UpdateCardEntitiesPriority()
        {
            int i = 0;
            foreach (CardEntity2D cardEntity2D in this.cardsHand)
            {
                cardEntity2D.Priority = 1000 + i;
                i++;
            }
        }

        protected override void OnLevelStateChanged(string obj)
        {
            this.LevelTurnPhase = (TurnPhase)Enum.Parse(typeof(TurnPhase), obj.ToString());
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            this.UpdateCardsToDraw();

            switch (this.LevelTurnPhase)
            {
                case TurnPhase.CREATE_HAND:
                    this.UpdateCreateHandPhase(deltaTime);
                    break;
                case TurnPhase.DRAW:
                    this.UpdateDrawPhase(deltaTime);
                    break;
                case TurnPhase.MAIN:
                    this.UpdateMainPhase(deltaTime);
                    break;
            }
        }

        private void UpdateCardsToDraw()
        {
            if (this.cardDrew != null
                && this.cardDrew.IsFliped)
            {
                this.cardDrew = null;
            }

            if (this.nbCardsToDraw > 0
                && this.cardDrew == null)
            {
                if (this.world2D.TryGetTarget(out World2D world))
                {
                    world.SendEventToWorld(new Model.Event.GameEvent(Model.Event.EventType.DRAW_CARD, null, string.Empty));
                }
            }
        }

        private void UpdateCreateHandPhase(Time deltaTime)
        {
            if(this.nbCardsToDraw == 0
                && this.cardDrew == null)
            {
                this.GoOnTurnPhase(TurnPhase.START_TURN);
            }
        }

        private void UpdateDrawPhase(Time deltaTime)
        {
            if (this.nbCardsToDraw == 0
                && this.cardDrew == null)
            {
                this.GoOnTurnPhase(TurnPhase.MAIN);
            }
        }

        private void UpdateMainPhase(Time deltaTime)
        {
            CardEntity2D cardFocused = this.GetCardFocused();

            if (cardFocused != this.cardFocused)
            {
                this.cardFocused = cardFocused;

                AEntity associatedCardFocused = this.objectToObject2Ds.FirstOrDefault(pElem => pElem.Value == cardFocused).Key;

                if (this.world2D.TryGetTarget(out World2D world))
                {
                    world.SendEventToWorld(new Model.Event.GameEvent(Model.Event.EventType.FOCUS_CARD, associatedCardFocused, null));
                }
            }     
        }

        private CardEntity2D GetCardFocused()
        {
            CardEntity2D cardFocused = null;

            Vector2i mousePosition = this.MousePosition;

            foreach (CardEntity2D cardDeck in this.cardsHand)
            {
                if(cardDeck is IHitRect
                    && (cardDeck as IHitRect).HitZone.Contains(mousePosition.X, mousePosition.Y))
                {
                    if(cardFocused == null
                        || Math.Abs(mousePosition.X - cardDeck.Position.X) + Math.Abs(mousePosition.Y - cardDeck.Position.Y) 
                        < Math.Abs(mousePosition.X - cardFocused.Position.X) + Math.Abs(mousePosition.Y - cardFocused.Position.Y))
                    {
                        cardFocused = cardDeck;
                    }
                }
            }

            return cardFocused;
        }

        private void GoOnTurnPhase(TurnPhase nextTurnPhase)
        {
            if (this.world2D.TryGetTarget(out World2D world))
            {
                world.SendEventToWorld(new Model.Event.GameEvent(Model.Event.EventType.LEVEL_PHASE_CHANGE, null, Enum.GetName(typeof(TurnPhase), nextTurnPhase)));
            }
        }

        protected override AEntity2D AddEntity(AEntity obj)
        {
            CardEntity2D entity2D = base.AddEntity(obj) as CardEntity2D;

            if (entity2D != null)
            {
                entity2D.Priority = this.maxPriority++;

                this.cardsDeck.Add(entity2D);
            }

            return entity2D;
        }

        protected override void OnEntityPropertyChanged(AEntity obj, string propertyName)
        {
            switch (propertyName)
            {
                case "IsFliped":
                    (this.objectToObject2Ds[obj] as CardEntity2D).IsFliped = (obj as CardEntity).IsFliped;
                    break;
                case "Position":
                    AEntity2D entity2D = this.objectToObject2Ds[obj];
                    entity2D.Position = new Vector2f(obj.Position.X, entity2D.Position.Y);
                    break;
                case "IsActive":
                    this.objectToObject2Ds[obj].IsActive = obj.IsActive;
                    break;
            }
        }

        protected override void UpdateViewSize(Vector2f viewSize, Time deltaTime)
        {
            this.DefaultViewSize = viewSize;
            this.view.Size = viewSize;

            foreach(KeyValuePair<AEntity, AEntity2D> pairEntity in this.objectToObject2Ds)
            {
                pairEntity.Value.Position = new Vector2f(pairEntity.Key.Position.X, this.view.Size.Y / 2 + pairEntity.Key.Position.Y);
            }
        }

        public override void FlushEntities()
        {
            base.FlushEntities();

            this.cardsDeck.Clear();

            this.cardsCemetery.Clear();

            this.cardsHand.Clear();

            this.maxPriority = 0;

            this.nbCardsToDraw = 0;

            this.LevelTurnPhase = TurnPhase.VOID;
            this.cardDrew = null;
        }

        public override void Dispose()
        {
            (this.parentLayer as BoardPlayerLayer).CardDrew -= OnCardDrew;
            (this.parentLayer as BoardPlayerLayer).NbCardsToDrawChanged -= OnNbCardToDrawsChanged;

            base.Dispose();
        }
    }
}
