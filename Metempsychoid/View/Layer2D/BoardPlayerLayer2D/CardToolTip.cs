using Metempsychoid.View.Text2D;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardPlayerLayer2D
{
    public class CardToolTip : TextCanevas2D
    {
        private RectangleShape bannerShape;

        public override Vector2f Position
        {
            get
            {
                return this.bannerShape.Position;
            }
            set
            {
                base.Position = value;

                this.bannerShape.Position = value * MainWindow.MODEL_TO_VIEW;
            }
        }

        public override Color SpriteColor
        {
            get
            {
                return this.bannerShape.FillColor;
            }
            set
            {
                this.bannerShape.FillColor = value;
            }
        }

        public override IntRect Canevas
        {
            get
            {
                return new IntRect(0, 0, (int) this.bannerShape.Size.X, (int) this.bannerShape.Size.Y);
            }

            set
            {
                Vector2f newSize = new Vector2f(value.Width, value.Height);

                if(this.bannerShape.Size != newSize)
                {
                    this.bannerShape.Size = newSize;

                    IntRect newCanevas = this.Canevas;
                    foreach (TextParagraph2D textParagraph2D in this.textParagraph2Ds)
                    {
                        textParagraph2D.Canevas = newCanevas;
                    }
                }
            }
        }

        public CardToolTip(ALayer2D parentLayer) 
            : base(parentLayer)
        {
            this.bannerShape = new RectangleShape(new Vector2f(200, 400));
            //this.bannerShape.Origin = new Vector2f(this.bannerShape.Size.X / 2, this.bannerShape.Size.Y / 2);

            this.bannerShape.FillColor = new Color(0, 0, 0, 128);

            this.Position = new Vector2f(0, 0);

            //SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(6), AnimationType.ONETIME);

            //IAnimation anim = new PositionAnimation(new Vector2f(-this.bannerShape.Size.X, 0), new Vector2f(0, 0), Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.SQUARE_DEC);
            //sequence.AddAnimation(0, anim);

            //anim = new ColorAnimation(new Color(0, 0, 0, 128), new Color(0, 0, 0, 0), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            //sequence.AddAnimation(5, anim);

            //this.animationsList.Add(sequence);

            //this.IsActive = false;
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            if (this.IsActive)
            {
                window.Draw(this.bannerShape);
            }

            base.DrawIn(window, deltaTime);
        }
    }
}
