﻿using Metempsychoid.Animation;
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
    public class BeamsEffect2D : AEntity2D
    {

        public BeamsEffect2D(IObject2DFactory factory, ALayer2D parentLayer, CardEntityDecorator2D parentCardDecorator2D) :
            base(parentLayer, false)
        {
            this.Position = parentCardDecorator2D.Position;

            this.ObjectSprite.Texture = factory.GetTextureByIndex(4);

            this.ObjectSprite.Origin = new SFML.System.Vector2f(this.ObjectSprite.TextureRect.Width / 2, this.ObjectSprite.TextureRect.Height / 2);

            // Active animation
            SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(6), AnimationType.ONETIME);

            IAnimation anim = new ZoomAnimation(0.05f, 1, Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            sequence.AddAnimation(2, anim);

            anim = new ZoomAnimation(1, 0.1f, Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            sequence.AddAnimation(5, anim);

            anim = new RotationAnimation(0, 360, Time.FromSeconds(4), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(2.01f, anim);

            this.animationsList.Add(sequence);

            this.IsActive = false;
        }

        public void DisplayBeamsEffect()
        {
            this.IsActive = true;

            this.Zoom = 0.05f;

            this.PlayAnimation(0);
        }

        public void HideBeamsEffect()
        {
            this.IsActive = false;
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
