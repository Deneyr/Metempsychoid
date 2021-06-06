using Metempsychoid.Animation;
using Metempsychoid.Model.Card;
using Metempsychoid.View.Animation;
using Metempsychoid.View.Text2D;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Layer2D.BoardNotifLayer2D
{
    public class EffectBanner2D : AEntity2D
    {
        private RectangleShape bannerShape;

        private bool isActive;

        public override bool IsActive
        {
            get
            {
                return this.isActive || this.IsAnimationRunning();
            }
            set
            {
                this.isActive = value;
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

        public override float Zoom
        {
            get
            {
                return this.bannerShape.Scale.Y;
            }
            set
            {
                this.bannerShape.Scale = new Vector2f(1, value);
            }
        }

        public EffectBanner2D(ALayer2D parentLayer)
            : base(parentLayer, false)
        {
            this.bannerShape = new RectangleShape(new Vector2f(3000, 200));
            this.bannerShape.Origin = new Vector2f(this.bannerShape.Size.X / 2, this.bannerShape.Size.Y / 2);

            this.bannerShape.FillColor = new Color(0, 0, 0, 128);

            this.Position = new Vector2f(0, 0);

            SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(6), AnimationType.ONETIME);

            IAnimation anim = new ZoomAnimation(0, 1, Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(0, anim);

            anim = new ZoomAnimation(1, 0, Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(5, anim);

            this.animationsList.Add(sequence);

            this.IsActive = false;
        }

        public void DisplayEffectBanner()
        {
             this.Zoom = 0;

            this.IsActive = true;
            this.PlayAnimation(0);
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            if (this.IsActive)
            {
                window.Draw(this.bannerShape);
            }
        }
    }
}

