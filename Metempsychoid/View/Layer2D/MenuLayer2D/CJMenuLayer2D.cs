using Metempsychoid.Model.MenuLayer;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.MenuLayer2D
{
    public class CJMenuLayer2D: ALayer2D
    {
        private CJStartButton2D startButton;

        private CJDeckBuildingButton2D player1DeckBuildingButton;
        private CJDeckBuildingButton2D player2DeckBuildingButton;

        private List<AEntity2D> focusableEntities;

        public CJMenuLayer2D(World2D world2D, IObject2DFactory factory, CJMenuLayer layer) :
            base(world2D, factory, layer)
        {
            this.Area = new Vector2i(int.MaxValue, int.MaxValue);

            this.startButton = new CJStartButton2D(this);

            this.player1DeckBuildingButton = new CJDeckBuildingButton2D(this, "start_deck_building1");
            this.player2DeckBuildingButton = new CJDeckBuildingButton2D(this, "start_deck_building2");

            this.focusableEntities = new List<AEntity2D>()
            {
                this.startButton,

                this.player1DeckBuildingButton,
                this.player2DeckBuildingButton
            };
        }

        public override void InitializeLayer(IObject2DFactory factory)
        {
            base.InitializeLayer(factory);

            this.startButton.Position = new Vector2f(0, -100);

            this.player1DeckBuildingButton.Position = new Vector2f(0, 0);
            this.player2DeckBuildingButton.Position = new Vector2f(0, 100);
        }

        protected override IEnumerable<AEntity2D> GetEntities2DFocusable()
        {
            return this.focusableEntities;
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            base.DrawIn(window, deltaTime);

            SFML.Graphics.View defaultView = window.DefaultView;
            window.SetView(this.view);

            this.startButton.DrawIn(window, deltaTime);

            this.player1DeckBuildingButton.DrawIn(window, deltaTime);
            this.player2DeckBuildingButton.DrawIn(window, deltaTime);

            window.SetView(defaultView);
        }

        public override void Dispose()
        {
            if (this.startButton != null)
            {
                this.startButton.Dispose();
            }

            if (this.player1DeckBuildingButton != null)
            {
                this.player1DeckBuildingButton.Dispose();
            }

            if (this.player2DeckBuildingButton != null)
            {
                this.player2DeckBuildingButton.Dispose();
            }

            base.Dispose();
        }
    }
}
