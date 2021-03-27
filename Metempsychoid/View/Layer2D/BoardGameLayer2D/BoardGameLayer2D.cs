using Metempsychoid.Model;
using Metempsychoid.Model.Card;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Node.TestWorld;
using Metempsychoid.Model.Player;
using Metempsychoid.View.Card2D;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardGameLayer2D
{
    public class BoardGameLayer2D : ALayer2D
    {
        private CardEntity2D cardPicked;

        public TurnPhase LevelTurnPhase
        {
            get;
            private set;
        }

        public override Vector2f Position
        {
            set
            {
                base.Position = value * 0.75f;
            }
        }

        public BoardGameLayer2D(World2D world2D, IObject2DFactory factory, BoardGameLayer layer) :
            base(world2D, layer)
        {
            this.Area = new Vector2i(int.MaxValue, int.MaxValue);

            layer.CardPicked += this.OnCardPicked;
            layer.CardUnpicked += this.OnCardUnPicked;
        }

        public override void InitializeLayer(IObject2DFactory factory)
        {
            base.InitializeLayer(factory);

            this.LevelTurnPhase = TurnPhase.VOID;

            this.cardPicked = null;
        }

        private void OnCardPicked(CardEntity obj)
        {
            this.cardPicked = this.objectToObject2Ds[obj] as CardEntity2D;
            this.cardPicked.Priority = 1000;
        }

        private void OnCardUnPicked(CardEntity obj)
        {
            this.cardPicked = null;
        }

        protected override void OnEntityPropertyChanged(AEntity obj, string propertyName)
        {
            base.OnEntityPropertyChanged(obj, propertyName);

            switch (propertyName)
            {
                case "CardSocketed":
                    StarEntity starEntity = obj as StarEntity;
                    StarEntity2D starEntity2D = this.objectToObject2Ds[obj] as StarEntity2D;

                    starEntity2D.SetCardSocketed(starEntity.CardSocketed);

                    this.cardPicked = null;
                    break;
                case "IsSocketed":
                    (this.objectToObject2Ds[obj] as CardEntity2D).IsSocketed = (obj as CardEntity).IsSocketed;
                    break;
                case "IsFliped":
                    (this.objectToObject2Ds[obj] as CardEntity2D).IsFliped = (obj as CardEntity).IsFliped;
                    break;
            }
        }

        protected override void OnLevelStateChanged(string obj)
        {
            this.LevelTurnPhase = (TurnPhase) Enum.Parse(typeof(TurnPhase), obj.ToString());
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            switch (this.LevelTurnPhase)
            {
                case TurnPhase.START_LEVEL:
                    this.UpdateStartLevelPhase(deltaTime);
                    break;
                case TurnPhase.MAIN:
                    this.UpdateMainPhase(deltaTime);
                    break;
            }
        }

        private void UpdateStartLevelPhase(Time deltaTime)
        {
            bool areAllLinksActive = true;
            foreach (KeyValuePair<AEntity, AEntity2D> pair in this.objectToObject2Ds)
            {
                if (pair.Value is StarLinkEntity2D)
                {
                    if (pair.Key.IsActive)
                    {
                        areAllLinksActive &= pair.Value.IsActive;
                    }
                }
            }

            if (areAllLinksActive)
            {
                this.GoOnTurnPhase(TurnPhase.CREATE_HAND);
            }
        }

        private void UpdateMainPhase(Time deltaTime)
        {

        }

        public override void UpdateAfterViewUpdated(Time deltaTime)
        {
            this.UpdateCardPickedPosition();
        }

        private void UpdateCardPickedPosition()
        {
            if (this.cardPicked != null)
            {
                Vector2i mousePosition = this.MousePosition;

                StarEntity2D starEntity2D = this.GetStarEntity2DOn(mousePosition);

                Vector2f cardPosition = new Vector2f(mousePosition.X, mousePosition.Y);

                if (starEntity2D != null)
                {
                    StarEntity starEntity = this.object2DToObjects[starEntity2D] as StarEntity;
                    CardEntity cardEntity = this.object2DToObjects[this.cardPicked] as CardEntity;

                    if (starEntity.CanSocketCard(cardEntity))
                    {
                        cardPosition = new Vector2f(starEntity2D.Position.X, starEntity2D.Position.Y);
                    }
                }

                this.cardPicked.Position = cardPosition;
            }
        }

        private StarEntity2D GetStarEntity2DOn(Vector2i mousePosition)
        {
            StarEntity2D starEntityResult = null;

            foreach (AObject2D object2D in this.objectToObject2Ds.Values)
            {
                StarEntity2D starEntity2D = object2D as StarEntity2D;

                if (starEntity2D != null
                    && starEntity2D.HitZone.Contains(mousePosition.X, mousePosition.Y))
                {
                    if (starEntityResult == null
                        || Math.Abs(mousePosition.X - starEntity2D.Position.X) + Math.Abs(mousePosition.Y - starEntity2D.Position.Y)
                        < Math.Abs(mousePosition.X - starEntity2D.Position.X) + Math.Abs(mousePosition.Y - starEntity2D.Position.Y))
                    {
                        starEntityResult = starEntity2D;
                    }
                }
            }

            return starEntityResult;
        }

        private void GoOnTurnPhase(TurnPhase nextTurnPhase)
        {
            if (this.world2D.TryGetTarget(out World2D world))
            {
                world.SendEventToWorld(new Model.Event.GameEvent(Model.Event.EventType.LEVEL_PHASE_CHANGE, null, Enum.GetName(typeof(TurnPhase), nextTurnPhase)));
            }
        }

        public override bool OnControlActivated(Controls.ControlEventType eventType, string details)
        {

            switch (this.LevelTurnPhase)
            {
                case TurnPhase.MAIN:

                    StarEntity2D starEntity2D = this.GetStarEntity2DOn(this.MousePosition);

                    if (eventType == Controls.ControlEventType.MOUSE_LEFT_CLICK && details == "pressed"
                        && this.cardPicked != null
                        && starEntity2D != null)
                    {
                        StarEntity starEntity = this.object2DToObjects[starEntity2D] as StarEntity;
                        CardEntity cardEntity = this.object2DToObjects[this.cardPicked] as CardEntity;

                        if (starEntity.CanSocketCard(cardEntity))
                        {
                            if (this.world2D.TryGetTarget(out World2D world))
                            {
                                world.SendEventToWorld(new Model.Event.GameEvent(Model.Event.EventType.SOCKET_CARD, this.object2DToObjects[starEntity2D], null));
                            }
                        }
                    }
                    break;
            }
            //switch (this.LevelTurnPhase)
            //{
            //    case TurnPhase.MAIN:
            //        if (eventType == Controls.ControlEventType.MOUSE_RIGHT_CLICK && details == "pressed"
            //            && this.cardPicked != null)
            //        {
            //            if (this.world2D.TryGetTarget(out World2D world))
            //            {
            //                world.SendEventToWorld(new Model.Event.GameEvent(Model.Event.EventType.PICK_CARD, null, null));
            //            }
            //        }
            //        break;
            //}

            //base.OnControlActivated(eventType, details);

            //Random rand = new Random();

            //Player player;
            //if (rand.NextDouble() < 0.5)
            //{
            //    player = new Player(Color.Green);
            //}
            //else
            //{
            //    player = new Player(Color.Red);
            //}

            //switch (eventType)
            //{
            //    case Controls.ControlEventType.UP:
            //        foreach (AEntity2D entity in this.objectToObject2Ds.Values)
            //        {
            //            if (entity is CardEntity2D)
            //            {
            //                (entity as CardEntity2D).IsSocketed = !(entity as CardEntity2D).IsSocketed;
            //                (entity as CardEntity2D).IsFliped = !(entity as CardEntity2D).IsFliped;
            //            }

            //            if(entity is StarEntity2D && rand.NextDouble() > 0.5)
            //            {
            //                (entity as StarEntity2D).SetCardSocketed(new CardEntity(null, new Card(new CardTemplate("wheel", 0), player), false));
            //            }
            //        }

            //        break;
            //}

            return true;
        }

        public override void Dispose()
        {
            this.LevelTurnPhase = TurnPhase.VOID;

            (this.parentLayer as BoardGameLayer).CardPicked -= this.OnCardPicked;
            (this.parentLayer as BoardGameLayer).CardUnpicked -= this.OnCardUnPicked;

            base.Dispose();
        }
    }
}
