using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model;
using Metempsychoid.Model.Card;
using Metempsychoid.Model.Layer.BoardBannerLayer;
using Metempsychoid.Model.Node.TestWorld;
using Metempsychoid.View.Card2D;
using Metempsychoid.View.Layer2D.BoardGameLayer2D;
using SFML.Graphics;
using SFML.System;

namespace Metempsychoid.View.Layer2D.BoardBannerLayer2D
{
    public class BoardBannerLayer2D : ALayer2D
    {
        private BannerEntity2D bannerEntity2D;

        private HeaderEntity2D headerEntity2D;

        private CardToolTip2D cardToolTip;

        private TurnPhase levelTurnPhase;

        private HashSet<ICardFocusedLayer> cardFocusedLayers;

        private CardEntity2D cardFocused;   

        public TurnPhase LevelTurnPhase
        {
            get
            {
                return this.levelTurnPhase;
            }
            private set
            {
                if (this.levelTurnPhase != value)
                {
                    this.levelTurnPhase = value;

                    switch (this.levelTurnPhase)
                    {
                        case TurnPhase.START_TURN:
                            this.InitializeStartTurnPhase();
                            break;
                        case TurnPhase.END_TURN:
                            this.InitializeEndTurnPhase();
                            break;
                    }
                }
            }
        }

        public BoardBannerLayer2D(World2D world2D, IObject2DFactory factory, BoardBannerLayer layer) :
            base(world2D, layer)
        {
            this.Area = new Vector2i(int.MaxValue, int.MaxValue);

            this.bannerEntity2D = new BannerEntity2D(this);

            this.cardFocusedLayers = new HashSet<ICardFocusedLayer>();
        }

        public override void InitializeLayer(IObject2DFactory factory)
        {
            base.InitializeLayer(factory);

            this.LevelTurnPhase = TurnPhase.VOID;         

            BoardBannerLayer boardBannerLayer = this.parentLayer as BoardBannerLayer;
            this.headerEntity2D = new HeaderEntity2D(this, boardBannerLayer.Player.PlayerName, boardBannerLayer.Opponent.PlayerName);

            this.cardFocused = null;
            this.cardFocusedLayers.Clear();
            if (this.world2D.TryGetTarget(out World2D world2D))
            {
                foreach(ALayer2D layer in world2D.LayersList)
                {
                    ICardFocusedLayer cardFocusedLayer = layer as ICardFocusedLayer;

                    if(cardFocusedLayer != null)
                    {
                        //cardFocusedLayer.CardFocusedChanged += OnCardFocusedChanged;

                        this.cardFocusedLayers.Add(cardFocusedLayer);
                    }
                }
            }
        }

        //private void OnCardFocusedChanged(ICardFocusedLayer cardFocusedLayer)
        //{
        //    this.layerToCardFocused[cardFocusedLayer] = cardFocusedLayer.CardFocused;
        //}

        protected override AEntity2D AddEntity(AEntity obj)
        {
            AEntity2D entityToAdd = base.AddEntity(obj);

            if(entityToAdd is CardToolTip2D)
            {
                this.cardToolTip = entityToAdd as CardToolTip2D;
            }

            return entityToAdd;
        }

        protected override void OnLevelStateChanged(string obj)
        {
            this.LevelTurnPhase = (TurnPhase)Enum.Parse(typeof(TurnPhase), obj.ToString());
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            this.UpdateFocusedEntity();

            switch (this.LevelTurnPhase)
            {
                case TurnPhase.START_TURN:
                    this.UpdateStartTurnPhase(deltaTime);
                    break;
                case TurnPhase.END_TURN:
                    this.UpdateEndTurnPhase(deltaTime);
                    break;
            }
        }

        private void UpdateFocusedEntity()
        {
            ICardFocusedLayer firstCardLayerFocused = this.cardFocusedLayers.FirstOrDefault(pElem => pElem.CardFocused != null);
            ALayer2D layer2D = firstCardLayerFocused as ALayer2D;

            if (firstCardLayerFocused != null)
            {
                CardEntity2D cardFocused = firstCardLayerFocused.CardFocused;
                if (this.cardFocused != cardFocused)
                {
                    CardEntity cardEntity = layer2D.GetEntityFromEntity2D(cardFocused) as CardEntity;

                    this.cardToolTip.DisplayToolTip(cardEntity.Card, cardFocused);

                    this.cardFocused = cardFocused;
                }

                if (this.cardToolTip.IsActive)
                {
                    Vector2f cardFocusedPosition = this.cardFocused.Position;

                    Vector2f cardPositionInWindow = layer2D.GetPositionInWindow(cardFocusedPosition);

                    if (cardPositionInWindow.Y > this.defaultViewSize.Y / 2)
                    {
                        cardFocusedPosition.Y -= this.cardFocused.Canevas.Height / 2;

                        cardFocusedPosition = this.GetPositionInScene(layer2D.GetPositionInWindow(cardFocusedPosition));

                        cardFocusedPosition.X -= this.cardToolTip.Canevas.Width / 2;
                        cardFocusedPosition.Y -= this.cardToolTip.Canevas.Height;
                    }
                    else
                    {
                        cardFocusedPosition.Y += this.cardFocused.Canevas.Height / 2;

                        cardFocusedPosition = this.GetPositionInScene(layer2D.GetPositionInWindow(cardFocusedPosition));

                        cardFocusedPosition.X -= this.cardToolTip.Canevas.Width / 2;
                    }

                    this.cardToolTip.Position = cardFocusedPosition;
                }
            }
            else
            {
                this.cardToolTip.HideToolTip();

                this.cardFocused = null;
            }
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            base.DrawIn(window, deltaTime);

            SFML.Graphics.View defaultView = window.DefaultView;
            window.SetView(this.view);

            this.bannerEntity2D.DrawIn(window, deltaTime);
            this.headerEntity2D.DrawIn(window, deltaTime);

            window.SetView(defaultView);
        }

        private void InitializeStartTurnPhase()
        {
            this.bannerEntity2D.IsActive = true;
            this.headerEntity2D.IsActive = true;
            this.bannerEntity2D.SpriteColor = new Color(0, 0, 0, 128);
            this.bannerEntity2D.Position = new Vector2f(-this.bannerEntity2D.ObjectSprite.TextureRect.Width, 0);
            this.bannerEntity2D.PlayAnimation(0);

            this.headerEntity2D.DisplayHeader((this.parentLayer as BoardBannerLayer).PlayerTurn.PlayerName, 0, 0);
        }

        private void InitializeEndTurnPhase()
        {
            this.bannerEntity2D.IsActive = true;
            this.headerEntity2D.IsActive = true;
            this.bannerEntity2D.PlayAnimation(1);

            this.headerEntity2D.DisplayHeader((this.parentLayer as BoardBannerLayer).PlayerTurn.PlayerName, 1, 1);
        }

        private void UpdateStartTurnPhase(Time deltaTime)
        {
            if (this.bannerEntity2D.IsAnimationRunning() == false)
            {
                this.bannerEntity2D.IsActive = false;
                this.headerEntity2D.IsActive = false;

                this.GoOnTurnPhase(TurnPhase.DRAW);
            }
        }

        private void UpdateEndTurnPhase(Time deltaTime)
        {
            if (this.bannerEntity2D.IsAnimationRunning() == false)
            {
                this.bannerEntity2D.IsActive = false;
                this.headerEntity2D.IsActive = false;

                this.GoOnTurnPhase(TurnPhase.START_TURN);
            }
        }

        private void GoOnTurnPhase(TurnPhase nextTurnPhase)
        {
            this.SendEventToWorld(Model.Event.EventType.LEVEL_PHASE_CHANGE, null, Enum.GetName(typeof(TurnPhase), nextTurnPhase));
        }

        public override void FlushEntities()
        {
            base.FlushEntities();

            this.LevelTurnPhase = TurnPhase.VOID;

            if (this.bannerEntity2D != null)
            {
                this.bannerEntity2D.IsActive = false;
                this.bannerEntity2D.Dispose();
            }

            if (this.headerEntity2D != null)
            {
                this.headerEntity2D.IsActive = false;
                this.headerEntity2D.Dispose();
            }

            //foreach(ICardFocusedLayer cardFocusedLayer in this.layerToCardFocused.Keys)
            //{
            //    cardFocusedLayer.CardFocusedChanged -= this.OnCardFocusedChanged;
            //}
            this.cardFocusedLayers.Clear();
        }
    }
}
