using Astrategia.Animation;
using Astrategia.View.Animation;
using Astrategia.View.Controls;
using Astrategia.View.Text2D;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.Card2D
{
    public class CardLabel2D : TextCanevas2D
    {
        private RectangleShape bannerShape;

        private int label;

        private int bonus;

        public override bool IsActive
        {
            get
            {
                return base.IsActive || this.IsAnimationRunning();
            }
        }

        public int Label
        {
            get
            {
                return this.label;
            }
            set
            {
                if(this.label != value)
                {
                    this.label = value;

                    //this.CreateTextOfParagraph(0, this.label.ToString(), "CardLabel", Color.White);

                    this.textParagraph2Ds[0].UpdateParameterText(0, this.label.ToString());

                    if (this.IsActive)
                    {
                        this.PlayAnimation(2);
                    }
                }
            }
        }

        public int Bonus
        {
            get
            {
                return this.bonus;
            }
            set
            {
                if (this.bonus != value)
                {
                    this.bonus = value;

                    if (this.bonus > 0)
                    {
                        //this.SpriteColor = Color.Green;
                        this.textParagraph2Ds[0].UpdateParameterColor(0, Color.Green);
                    }
                    else if(this.bonus < 0)
                    {
                        //this.SpriteColor = Color.Red;
                        this.textParagraph2Ds[0].UpdateParameterColor(0, Color.Red);
                    }
                    else
                    {
                        //this.SpriteColor = Color.White;
                        this.textParagraph2Ds[0].UpdateParameterColor(0, Color.White);
                    }
                }
            }
        }

        public override Vector2f Position
        {
            get
            {
                return this.bannerShape.Position;
            }
            set
            {
                Vector2f realPosition = new Vector2f(value.X - this.bannerShape.Size.X / 2, value.Y);

                base.Position = realPosition;

                this.bannerShape.Position = realPosition * MainWindow.MODEL_TO_VIEW;
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
                base.SpriteColor = value;

                this.bannerShape.FillColor = new Color(0, 0, 0, (byte)(value.A / 2));
            }
        }

        public override IntRect Canevas
        {
            get
            {
                return new IntRect(0, 0, (int)this.bannerShape.Size.X, (int)this.bannerShape.Size.Y);
            }

            set
            {
                Vector2f newSize = new Vector2f(value.Width, value.Height);

                if (this.bannerShape.Size != newSize)
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

        public CardLabel2D(ALayer2D parentLayer, int label)
            : base(parentLayer)
        {
            this.label = int.MinValue;
            this.Bonus = 0;

            this.bannerShape = new RectangleShape(new Vector2f(50, 25));
            this.Position = new Vector2f(0, 0);

            this.CreateTextParagraph2D(new Vector2f(0, 0), new Vector2f(0, 0), TextParagraph2D.Alignment.CENTER, 30);

            this.UpdateTextOfParagraph(0, "field_content");

            this.SpriteColor = new Color(255, 255, 255, 0);

            IAnimation showAnimation = new ColorAnimation(new Color(255, 255, 255, 0), new Color(255, 255, 255, 255), Time.FromSeconds(0.5f), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            this.animationsList.Add(showAnimation);

            //SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(2), AnimationType.LOOP);
            //IAnimation focusedAnimation = new ColorAnimation(new Color(0, 0, 0, 255), new Color(125, 125, 125, 255), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            //sequence.AddAnimation(0, focusedAnimation);

            //focusedAnimation = new ColorAnimation(new Color(125, 125, 125, 255), new Color(0, 0, 0, 255), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            //sequence.AddAnimation(1, focusedAnimation);
            //this.animationsList.Add(sequence);

            IAnimation hideAnimation = new ColorAnimation(new Color(255, 255, 255, 255), new Color(255, 255, 255, 0), Time.FromSeconds(0.5f), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            this.animationsList.Add(hideAnimation);

            IAnimation labelChangedAnimation = new ZoomAnimation(3, 1, Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            this.animationsList.Add(labelChangedAnimation);

            this.IsActive = false;

            this.Label = label;
        }

        public void ShowLabel()
        {
            this.IsActive = true;

            this.PlayAnimation(0);
        }

        public void HideLabel()
        {
            if (this.IsActive)
            {
                this.IsActive = false;

                this.PlayAnimation(1);
            }
        }

        //public override void DrawIn(RenderWindow window, Time deltaTime)
        //{
        //    if (this.IsActive)
        //    {
        //        window.Draw(this.bannerShape);
        //    }

        //    base.DrawIn(window, deltaTime);
        //}
    }
}
