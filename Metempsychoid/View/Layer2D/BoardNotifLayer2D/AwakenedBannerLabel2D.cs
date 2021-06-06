using Metempsychoid.Animation;
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
    public class AwakenedBannerLabel2D : TextCanevas2D
    {
        //private RectangleShape bannerShape;

        //public override Vector2f Position
        //{
        //    get
        //    {
        //        return this.bannerShape.Position;
        //    }
        //    set
        //    {
        //        base.Position = value;

        //        this.bannerShape.Position = value * MainWindow.MODEL_TO_VIEW;
        //    }
        //}

        //public override Color SpriteColor
        //{
        //    get
        //    {
        //        return this.bannerShape.FillColor;
        //    }
        //    set
        //    {
        //        this.bannerShape.FillColor = value;
        //    }
        //}

        public override bool IsActive
        {
            get
            {
                return base.IsActive && this.SpriteColor.A > 0;
            }
        }

        public AwakenedBannerLabel2D(ALayer2D parentLayer)
            : base(parentLayer)
        {
            this.CreateTextParagraph2D(new Vector2f(0, 0), new Vector2f(0, 0), TextParagraph2D.Alignment.CENTER, 60);

            this.Canevas = new IntRect(0, 0, 2000, 0);
            this.Position = new Vector2f(-1000, -50);

            this.UpdateTextOfParagraph(0, "awakened_label");

            SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(5), AnimationType.ONETIME);

            IAnimation anim = new ColorAnimation(new Color(0, 0, 0, 0), new Color(0, 0, 0, 255), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(0, anim);
            this.animationsList.Add(sequence);

            anim = new ZoomAnimation(0f, 1, Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.SQUARE_DEC);
            sequence.AddAnimation(1f, anim);

            anim = new ZoomAnimation(1f, 10, Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            sequence.AddAnimation(3, anim);

            anim = new ColorAnimation(new Color(0, 0, 0, 255), new Color(0, 0, 0, 0), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(4f, anim);
            this.animationsList.Add(sequence);

            this.IsActive = false;
        }

        public void DisplayBanner()
        {
            this.IsActive = true;

            this.Zoom = 0f;

            this.PlayAnimation(0);
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            base.DrawIn(window, deltaTime);
        }

        //public override Color SpriteColor
        //{
        //    get => base.SpriteColor;
        //    set => base.SpriteColor = value;
        //}
    }
}
