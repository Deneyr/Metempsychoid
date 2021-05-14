using Metempsychoid.Animation;
using Metempsychoid.Model.Animation;
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
    public class BannerEntity2D : AEntity2D
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

        public BannerEntity2D(ALayer2D parentLayer) 
            : base(parentLayer, false)
        {
            this.bannerShape = new RectangleShape(new Vector2f(3000, 200));
            this.bannerShape.Origin = new Vector2f(this.bannerShape.Size.X / 2, this.bannerShape.Size.Y / 2);

            this.bannerShape.FillColor = new Color(0, 0, 0, 128);

            this.Position = new Vector2f(-this.bannerShape.Size.X, 0);

            SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(6), AnimationType.ONETIME);

            IAnimation anim = new PositionAnimation(new Vector2f(-this.bannerShape.Size.X, 0), new Vector2f(0, 0), Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.SQUARE_DEC);
            sequence.AddAnimation(0, anim);

            anim = new ColorAnimation(new Color(0, 0, 0, 128), new Color(0, 0, 0, 0), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            sequence.AddAnimation(5, anim);
            this.animationsList.Add(sequence);

            sequence = new SequenceAnimation(Time.FromSeconds(4), AnimationType.ONETIME);
            anim = new ColorAnimation(new Color(0, 0, 0, 0), new Color(0, 0, 0, 128), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            sequence.AddAnimation(0, anim);

            anim = new ColorAnimation(new Color(0, 0, 0, 128), new Color(0, 0, 0, 0), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            sequence.AddAnimation(3, anim);
            this.animationsList.Add(sequence);
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
