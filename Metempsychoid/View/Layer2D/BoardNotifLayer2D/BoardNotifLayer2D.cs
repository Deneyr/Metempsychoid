using Metempsychoid.Animation;
using Metempsychoid.Model;
using Metempsychoid.Model.Animation;
using Metempsychoid.Model.Layer.BoardNotifLayer;
using Metempsychoid.View.Card2D;
using Metempsychoid.View.Layer2D.BoardBannerLayer2D;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardNotifLayer2D
{
    public class BoardNotifLayer2D : ALayer2D //, ICardFocusedLayer
    {
        private AwakenedBannerLabel2D awakenedBannerLabel2D;
        private EffectBanner2D effectBanner2D;
        private EffectLabel2D effectLabel2D;

        private CardEntityDecorator2D cardAwakened;

        //private BoardGameLayer2D.BoardGameLayer2D boardGameLayer2D;

        //private static Vector2f HAND_POSITION = new Vector2f(0, -350);
        //private static int HAND_CARD_SPACE = 100;

        //public event Action<ICardFocusedLayer> CardFocusedChanged;

        //protected Vector2f HandPosition
        //{
        //    get
        //    {
        //        Vector2f result = HAND_POSITION;

        //        return result;
        //    }
        //}

        //public List<CardEntity2D> CardsHand
        //{
        //    get;
        //    private set;
        //}

        //public override IHitRect FocusedGraphicEntity2D
        //{
        //    protected set
        //    {
        //        IHitRect previousEntityFocused = base.FocusedGraphicEntity2D;

        //        base.FocusedGraphicEntity2D = value;

        //        if (previousEntityFocused != value && value is CardEntity2D)
        //        {
        //            this.UpdateCardsHandPosition();
        //        }
        //    }
        //}

        //public CardEntity2D CardFocused
        //{
        //    get
        //    {
        //        return this.FocusedGraphicEntity2D as CardEntity2D;
        //    }
        //}

        public CardEntityDecorator2D CardAwakened
        {
            get
            {
                return this.cardAwakened;
            }
            private set
            {
                if(value != this.cardAwakened)
                {
                    this.cardAwakened = value;

                    if (this.cardAwakened != null)
                    {
                        this.cardAwakened.DisplayAwakened();
                        this.awakenedBannerLabel2D.DisplayBanner();
                    }
                }
            }
        }

        public BoardNotifLayer2D(World2D world2D, IObject2DFactory factory, BoardNotifLayer layer) :
            base(world2D, layer)
        {
            this.Area = new Vector2i(int.MaxValue, int.MaxValue);

            this.awakenedBannerLabel2D = new AwakenedBannerLabel2D(this);
            this.effectBanner2D = new EffectBanner2D(this);

            this.effectLabel2D = new EffectLabel2D(this);

            layer.CardAwakened += OnCardAwakened;
        }

        public override void InitializeLayer(IObject2DFactory factory)
        {
            base.InitializeLayer(factory);

            this.cardAwakened = null;

            //if (this.world2D.TryGetTarget(out World2D world2D))
            //{
            //    this.boardGameLayer2D = world2D.LayersList.First(pElem => pElem is BoardGameLayer2D.BoardGameLayer2D) as BoardGameLayer2D.BoardGameLayer2D;
            //}
        }

        private void OnCardAwakened(CardEntityDecorator obj)
        {
            this.CardAwakened = this.objectToObject2Ds[obj] as CardEntityDecorator2D;
        }

        public Vector2f GetPositionFrom(ALayer layerFrom, Vector2f position)
        {
            Vector2f originPosition = new Vector2f();
            if (this.world2D.TryGetTarget(out World2D world2D))
            {
                originPosition = this.GetPositionInScene(world2D.LayersDictionary[layerFrom].GetPositionInWindow(position));
            }

            return originPosition;
        }

        //private void UpdateCardsHandPosition()
        //{
        //    float startWidth = this.HandPosition.X + HAND_CARD_SPACE * this.CardsHand.Count / 2f;

        //    int i = 0;
        //    bool cardFocusedEncountered = false;

        //    foreach (CardEntity2D cardEntity2D in this.CardsHand)
        //    {
        //        Vector2f newPosition;
        //        cardFocusedEncountered |= this.CardFocused == cardEntity2D;

        //        if (this.CardFocused != null)
        //        {
        //            if (this.CardFocused == cardEntity2D)
        //            {
        //                newPosition = new Vector2f(startWidth - i * HAND_CARD_SPACE, this.HandPosition.Y);
        //            }
        //            else if (cardFocusedEncountered)
        //            {
        //                newPosition = new Vector2f(startWidth - (i + 1) * HAND_CARD_SPACE, this.HandPosition.Y);
        //            }
        //            else
        //            {
        //                newPosition = new Vector2f(startWidth - (i - 1) * HAND_CARD_SPACE, this.HandPosition.Y);
        //            }
        //        }
        //        else
        //        {
        //            newPosition = new Vector2f(startWidth - i * HAND_CARD_SPACE, this.HandPosition.Y);
        //        }

        //        IAnimation positionAnimation;
        //        if (this.CardFocused != null)
        //        {
        //            positionAnimation = new PositionAnimation(cardEntity2D.Position, newPosition, Time.FromSeconds(1f), AnimationType.ONETIME, InterpolationMethod.SQUARE_DEC);
        //        }
        //        else
        //        {
        //            positionAnimation = new PositionAnimation(cardEntity2D.Position, newPosition, Time.FromSeconds(2f), AnimationType.ONETIME, InterpolationMethod.SIGMOID);
        //        }

        //        cardEntity2D.PlayAnimation(positionAnimation);
        //        i++;
        //    }
        //}

        protected override AEntity2D AddEntity(AEntity obj)
        {
            AEntity2D entity2D = base.AddEntity(obj);

            //if (entity2D is CardEntity2D)
            //{
            //    CardEntityDecorator cardDecorator = obj as CardEntityDecorator;

            //    if (this.world2D.TryGetTarget(out World2D world2D))
            //    {
            //        ALayer2D cardDecoratedParentLayer2D = world2D.LayersDictionary[cardDecorator.ParentLayer];

            //        if (cardDecorator != null)
            //        {
            //            entity2D.Position = this.GetPositionInScene(cardDecoratedParentLayer2D.GetPositionInWindow(cardDecorator.CardDecoratedPosition));
            //        }
            //    }

            //    this.CardsHand.Add(entity2D as CardEntity2D);

            //    this.UpdateCardsHandPosition();
            //}

            return entity2D;
        }

        protected override void OnEntityPropertyChanged(AEntity obj, string propertyName)
        {
            base.OnEntityPropertyChanged(obj, propertyName);

            switch (propertyName)
            {
                case "Value":
                    CardEntityDecorator2D cardConcerned2D = this.objectToObject2Ds[obj] as CardEntityDecorator2D;
                    CardEntityDecorator cardConcerned = (obj as CardEntityDecorator);

                    cardConcerned2D.CardValue = cardConcerned.Card.Value;
                    cardConcerned2D.CardValueModifier = cardConcerned.Card.ValueModifier;
                    break;
               
            }
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            base.UpdateGraphics(deltaTime);

            if(this.CardAwakened != null)
            {
                switch (this.CardAwakened.DecoratorState)
                {
                    case CardEntityDecorator2D.CardDecoratorState.PENDING:

                        if (this.effectBanner2D.IsActive == false)
                        {
                            this.effectBanner2D.DisplayEffectBanner();

                            this.effectLabel2D.DisplayEffectLabel((this.object2DToObjects[this.CardAwakened] as CardEntityDecorator).Card.EffectIdLoc);
                        }
                        else if (this.effectBanner2D.IsAnimationRunning() == false)
                        {
                            this.CardAwakened.HideAwakened();

                            this.effectBanner2D.IsActive = false;
                            this.effectLabel2D.IsActive = false;
                        }

                        break;
                    case CardEntityDecorator2D.CardDecoratorState.FINISHED:
                        AEntity cardEntityDecorator = this.object2DToObjects[this.CardAwakened];

                        this.CardAwakened = null;

                        this.SendEventToWorld(Model.Event.EventType.NEXT_BEHAVIOR, cardEntityDecorator, string.Empty);
                        break;
                }
            }
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            base.DrawIn(window, deltaTime);

            SFML.Graphics.View defaultView = window.DefaultView;
            window.SetView(this.view);

            this.awakenedBannerLabel2D.DrawIn(window, deltaTime);

            this.effectBanner2D.DrawIn(window, deltaTime);
            this.effectLabel2D.DrawIn(window, deltaTime);

            window.SetView(defaultView);
        }

        public override void Dispose()
        {
            if(this.effectBanner2D != null)
            {
                this.effectBanner2D.Dispose();
            }

            if (this.effectLabel2D != null)
            {
                this.effectLabel2D.Dispose();
            }

            (this.parentLayer as BoardNotifLayer).CardAwakened += OnCardAwakened;

            base.Dispose();
        }
    }
}
