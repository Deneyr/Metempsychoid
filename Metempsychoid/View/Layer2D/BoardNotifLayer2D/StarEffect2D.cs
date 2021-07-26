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

namespace Metempsychoid.View.Layer2D.BoardNotifLayer2D
{
    public class StarEffect2D : AEntity2D
    {
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

        public StarEffect2D(IObject2DFactory factory, ALayer2D parentLayer, CardEntityAwakenedDecorator2D parentCardDecorator2D) :
            base(parentLayer, factory, false)
        {
            this.Position = parentCardDecorator2D.Position;

            this.ObjectSprite.Texture = factory.GetTextureByIndex(3);

            this.ObjectSprite.Origin = new SFML.System.Vector2f(this.ObjectSprite.TextureRect.Width / 2, this.ObjectSprite.TextureRect.Height / 2);

            // Active animation
            SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(4), AnimationType.ONETIME);

            IAnimation anim = new ZoomAnimation(0.1f, 1, Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            sequence.AddAnimation(0, anim);

            //anim = new ZoomAnimation(1, 0.1f, Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            //sequence.AddAnimation(5, anim);

            anim = new RotationAnimation(0, 360, Time.FromSeconds(6), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(0.01f, anim);

            this.animationsList.Add(sequence);

            this.IsActive = false;
        }

        public void DisplayStarEffect()
        {
            this.IsActive = true;

            this.Zoom = 0.1f;

            this.PlayAnimation(0);
        }

        public void ContinueStarEffect()
        {
            IAnimation anim = new RotationAnimation(this.Rotation, this.Rotation + 360, Time.FromSeconds(6), AnimationType.LOOP, InterpolationMethod.LINEAR);

            this.PlayAnimation(anim);
        }

        public void HideStarEffect()
        {
            this.IsActive = false;

            SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(1), AnimationType.ONETIME);

            IAnimation anim = new ZoomAnimation(1, 0.1f, Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            sequence.AddAnimation(0, anim);

            anim = new RotationAnimation(this.Rotation, this.Rotation + 60, Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(0.01f, anim);

            this.PlayAnimation(sequence);
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            if (this.IsActive)
            {
                base.DrawIn(window, deltaTime);
            }
        }
    }
}
