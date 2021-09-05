using Metempsychoid.Animation;
using Metempsychoid.View.Animation;
using Metempsychoid.View.Controls;
using Metempsychoid.View.Text2D;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.MenuLayer2D
{
    public abstract class ACJButton2D : AButton2D
    {
        private RectangleShape bannerShape;

        public override bool IsActive
        {
            get
            {
                return base.IsActive || this.IsAnimationRunning(); ;
            }
            set
            {
                base.IsActive = value;
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
                Vector2f realPosition = new Vector2f(value.X - this.bannerShape.Size.X / 2, value.Y - this.bannerShape.Size.Y / 2);

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

                this.bannerShape.FillColor = new Color(value.R, value.G, value.B, (byte)(value.A / 2));
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

        public ACJButton2D(ALayer2D parentLayer, int width, string idLabel)
            : base(parentLayer)
        {
            this.bannerShape = new RectangleShape(new Vector2f(width, 75));
            this.SpriteColor = new Color(128, 128, 128, 255);

            this.Position = new Vector2f(0, 0);

            this.CreateTextParagraph2D(new Vector2f(0, 25), new Vector2f(0, 0), TextParagraph2D.Alignment.CENTER, 20);
            this.UpdateTextOfParagraph(0, idLabel);

            IAnimation showAnimation = new ColorAnimation(new Color(0, 0, 0, 0), new Color(128, 128, 128, 255), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            this.animationsList.Add(showAnimation);

            SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(2), AnimationType.LOOP);
            IAnimation focusedAnimation = new ColorAnimation(new Color(128, 128, 128, 255), new Color(255, 255, 255, 255), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(0, focusedAnimation);

            focusedAnimation = new ColorAnimation(new Color(255, 255, 255, 255), new Color(128, 128, 128, 255), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(1, focusedAnimation);
            this.animationsList.Add(sequence);

            IAnimation hideAnimation = new ColorAnimation(new Color(128, 128, 128, 255), new Color(0, 0, 0, 0), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            this.animationsList.Add(hideAnimation);

            this.IsActive = true;
        }

        public override bool IsFocusable(ALayer2D parentLayer)
        {
            return true;
        }

        public virtual void ActiveButton()
        {
            this.SpriteColor = new Color(0, 0, 0, 0);

            this.IsActive = true;

            this.PlayAnimation(0);
        }

        public virtual void DeactiveButton()
        {
            this.IsActive = false;

            this.PlayAnimation(2);
        }

        public override void OnMouseFocused(ALayer2D parentLayer, ControlEventType eventType)
        {
            base.OnMouseFocused(parentLayer, eventType);

            this.PlayAnimation(1);
        }

        public override bool OnMousePressed(ALayer2D parentLayer, ControlEventType eventType)
        {
            this.Zoom = 1.1f;
            return false;
        }

        public override void OnMouseUnFocused(ALayer2D parentLayer, ControlEventType eventType)
        {
            AObject2D.animationManager.StopAnimation(this);

            this.SpriteColor = new Color(128, 128, 128, 255);
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
