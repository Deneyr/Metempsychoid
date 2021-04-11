using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model;
using Metempsychoid.Model.Layer.BoardBannerLayer;
using Metempsychoid.Model.Node.TestWorld;
using SFML.Graphics;
using SFML.System;

namespace Metempsychoid.View.Layer2D.BoardBannerLayer2D
{
    public class BoardBannerLayer2D : ALayer2D
    {
        private BannerEntity2D bannerEntity2D;

        private HeaderEntity2D headerEntity2D;

        private TurnPhase levelTurnPhase;

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
                    }
                }
            }
        }

        public BoardBannerLayer2D(World2D world2D, IObject2DFactory factory, BoardBannerLayer layer) :
            base(world2D, layer)
        {
            this.Area = new Vector2i(int.MaxValue, int.MaxValue);
        }

        public override void InitializeLayer(IObject2DFactory factory)
        {
            base.InitializeLayer(factory);

            this.LevelTurnPhase = TurnPhase.VOID;

            this.bannerEntity2D = new BannerEntity2D(this);

            BoardBannerLayer boardBannerLayer = this.parentLayer as BoardBannerLayer;
            this.headerEntity2D = new HeaderEntity2D(this, boardBannerLayer.Player.PlayerName, boardBannerLayer.Opponent.PlayerName);
        }

        protected override void OnLevelStateChanged(string obj)
        {
            this.LevelTurnPhase = (TurnPhase)Enum.Parse(typeof(TurnPhase), obj.ToString());
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            switch (this.LevelTurnPhase)
            {
                case TurnPhase.START_TURN:
                    this.UpdateStartTurnPhase(deltaTime);
                    break;
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
            this.bannerEntity2D.SpriteColor = new Color(0, 0, 0, 128);
            this.bannerEntity2D.Position = new Vector2f(-this.bannerEntity2D.ObjectSprite.TextureRect.Width, 0);
            this.bannerEntity2D.PlayAnimation(0);

            this.headerEntity2D.DisplayHeader("start_player_turn");
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

        private void GoOnTurnPhase(TurnPhase nextTurnPhase)
        {
            if (this.world2D.TryGetTarget(out World2D world))
            {
                world.SendEventToWorld(new Model.Event.GameEvent(Model.Event.EventType.LEVEL_PHASE_CHANGE, null, Enum.GetName(typeof(TurnPhase), nextTurnPhase)));
            }
        }

        public override void FlushEntities()
        {
            base.FlushEntities();

            this.LevelTurnPhase = TurnPhase.VOID;

            this.bannerEntity2D.IsActive = false;
            this.headerEntity2D.IsActive = false;
        }
    }
}
