using Metempsychoid.Animation;
using Metempsychoid.View.Animation;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardBannerLayer2D
{
    public class HeaderEntity2D : AEntity2D
    {
        private Text text;

        public override Vector2f Position
        {
            get
            {
                return this.text.Position;
            }
            set
            {
                base.Position = value;

                this.text.Position = value * MainWindow.MODEL_TO_VIEW;
            }
        }

        public override Color SpriteColor
        {
            get
            {
                return this.text.FillColor;
            }
            set
            {
                this.text.FillColor = new Color(this.text.FillColor.R, this.text.FillColor.G, this.text.FillColor.B, value.A);
                this.text.OutlineColor = new Color(this.text.OutlineColor.R, this.text.OutlineColor.G, this.text.OutlineColor.B, value.A);
            }
        }

        public override float Zoom
        {
            get
            {
                return this.text.Scale.X;
            }
            set
            {
                this.text.Scale = new Vector2f(value, value);
            }
        }

        public uint CharacterSize
        {
            get
            {
                return this.text.CharacterSize;
            }
            set
            {
                this.text.CharacterSize = value;

                this.UpdateCanevas();
            }
        }

        public string Text
        {
            get
            {
                return this.text.DisplayedString;
            }
            set
            {
                this.text.DisplayedString = value;

                this.UpdateCanevas();
            }
        }

        public HeaderEntity2D(ALayer2D parentLayer) : base(parentLayer)
        {
            this.text = new Text();
            this.text.Font = AObject2DFactory.GetFontByName("Protector");

            this.Position = new Vector2f(0, 0);
            this.text.FillColor = Color.White;
            this.CharacterSize = 80;
            this.text.OutlineThickness = 2;
            this.text.OutlineColor = Color.Black;

            SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(6), AnimationType.ONETIME);

            IAnimation anim = new ColorAnimation(new Color(0, 0, 0, 0), new Color(0, 0, 0, 0), Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.SQUARE_DEC);
            sequence.AddAnimation(0, anim);

            anim = new ColorAnimation(new Color(0, 0, 0, 0), new Color(0, 0, 0, 255), Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.SQUARE_DEC);
            sequence.AddAnimation(2, anim);

            anim = new ColorAnimation(new Color(0, 0, 0, 255), new Color(0, 0, 0, 0), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            sequence.AddAnimation(5, anim);

            this.animationsList.Add(sequence);

            this.IsActive = false;
        }

        private void UpdateCanevas()
        {
            this.text.Origin = new Vector2f(this.text.GetGlobalBounds().Width / 2, this.text.GetGlobalBounds().Height / 2);
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            if (this.IsActive)
            {
                window.Draw(this.text);
            }
        }
    }
}
