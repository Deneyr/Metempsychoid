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

        private int maxPriority;

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

            this.cardsDeck = new List<CardEntity2D>();

            this.cardsCemetery = new List<CardEntity2D>();

            this.cardsHand = new List<CardEntity2D>();

            this.maxPriority = 0;

            (this.parentLayer as BoardPlayerLayer).CardDrew += OnCardDrew;

            this.LevelTurnPhase = TurnPhase.VOID;
            this.cardDrew = null;
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
            switch (this.LevelTurnPhase)
            {
                case TurnPhase.CREATE_HAND:
                    this.UpdateCreateHandPhase(deltaTime);
                    break;
            }
        }

        private void UpdateCreateHandPhase(Time deltaTime)
        {
            if(this.cardDrew != null
                && this.cardDrew.IsFliped)
            {
                this.cardDrew = null;
            }

            if(this.cardDrew == null)
            {
                if (this.world2D.TryGetTarget(out World2D world))
                {
                    world.SendEventToWorld(new Model.Event.GameEvent(Model.Event.EventType.DRAW_CARD, null, string.Empty));
                }
            }
        }

        private void GoOnTurnPhase(TurnPhase nextTurnPhase)
        {
            if (this.world2D.TryGetTarget(out World2D world))
            {
                world.SendEventToWorld(new Model.Event.GameEvent(Model.Event.EventType.LEVEL_PHASE_CHANGE, null, Enum.GetName(typeof(TurnPhase), nextTurnPhase)));
            }
        }

        protected override void OnEntityAdded(AEntity obj)
        {
            CardEntity2D entity2D = this.AddEntity(obj) as CardEntity2D;

            entity2D.Priority = this.maxPriority++;

            this.cardsDeck.Add(entity2D);
        }

        protected override void OnEntityPropertyChanged(AEntity obj, string propertyName)
        {
            base.OnEntityPropertyChanged(obj, propertyName);

            switch (propertyName)
            {
                case "IsFliped":
                    (this.objectToObject2Ds[obj] as CardEntity2D).IsFliped = (obj as CardEntity).IsFliped;
                    break;
                //case "Position":
                //    AEntity2D entity2D = this.objectToObject2Ds[obj];
                //    IAnimation positionAnimation = new PositionAnimation(entity2D.Position, obj.Position, Time.FromSeconds(20f), AnimationType.ONETIME, InterpolationMethod.SIGMOID);

                //    this.objectToObject2Ds[obj].PlayAnimation(positionAnimation);
                //    break;
                //case "IsActive":
                //    this.objectToObject2Ds[obj].IsActive = obj.IsActive;
                //    break;
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

            this.LevelTurnPhase = TurnPhase.VOID;
            this.cardDrew = null;
        }

        public override void Dispose()
        {
            (this.parentLayer as BoardPlayerLayer).CardDrew -= OnCardDrew;

            base.Dispose();
        }
    }
}
