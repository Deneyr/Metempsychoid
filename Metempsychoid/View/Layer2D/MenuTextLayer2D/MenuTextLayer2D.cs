using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.Model;
using Astrategia.Model.Node.TestWorld;
using Astrategia.View.Text2D;
using SFML.Graphics;
using SFML.System;

namespace Astrategia.View.Layer2D.MenuTextLayer2D
{
    public class MenuTextLayer2D : ALayer2D
    {
        private TextCanevas2D titleText2D;

        protected TurnPhase levelTurnPhase;

        public virtual TurnPhase LevelTurnPhase
        {
            get
            {
                return this.levelTurnPhase;
            }
            protected set
            {
                if (this.levelTurnPhase != value)
                {
                    this.levelTurnPhase = value;

                    switch (this.levelTurnPhase)
                    {
                        case TurnPhase.CREATE_HAND:
                            this.titleText2D.IsActive = true;

                            this.titleText2D.LaunchTextOfParagraphScrolling(0, 8);
                            break;
                    }
                }
            }
        }

        public override Vector2f Position
        {
            set
            {
                base.Position = value * 0.75f;
            }
        }

        public MenuTextLayer2D(World2D world2D, IObject2DFactory layerFactory, ALayer layer) : base(world2D, layerFactory, layer)
        {
            this.Area = new Vector2i(int.MaxValue, int.MaxValue);

            this.LevelTurnPhase = TurnPhase.VOID;

            this.titleText2D = new TextCanevas2D(this);
            this.titleText2D.Position = new Vector2f(-5000, -800);
            this.titleText2D.Canevas = new IntRect(0, 0, 10000, 1000);

            this.titleText2D.CreateTextParagraph2D(new Vector2f(0, 0), new Vector2f(0, 0), TextParagraph2D.Alignment.CENTER, 1000);
            this.titleText2D.UpdateTextOfParagraph(0, "menu_title");
        }

        public override void InitializeLayer(IObject2DFactory factory)
        {
            base.InitializeLayer(factory);

            this.titleText2D.IsActive = false;
        }

        protected override void OnLevelStateChanged(string obj)
        {
            this.LevelTurnPhase = (TurnPhase)Enum.Parse(typeof(TurnPhase), obj);
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            base.DrawIn(window, deltaTime);

            SFML.Graphics.View defaultView = window.DefaultView;
            window.SetView(this.view);

            this.titleText2D.DrawIn(window, deltaTime);

            window.SetView(defaultView);
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            base.UpdateGraphics(deltaTime);

            this.titleText2D.UpdateGraphics(deltaTime);
        }
    }
}
