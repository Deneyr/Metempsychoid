﻿using Metempsychoid.Animation;
using Metempsychoid.View.Animation;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Card2D
{
    public class CardHalo2D : AEntity2D
    {
        RenderStates render;

        Clock timer = new Clock();

        private bool isActive;

        public override bool IsActive
        {
            get
            {
                return this.HaloState != CardHaloState.NOT_ACTIVE;
            }

            set
            {
                if (this.isActive != value)
                {
                    this.isActive = value;

                    if (this.HaloState == CardHaloState.TRANSITIONING)
                    {
                        this.PlayAnimation(this.CreateTransitioningAnimation());
                    }
                }
            }
        }

        public CardHaloState HaloState
        {
            get;
            private set;
        }

        public CardHalo2D(IObject2DFactory factory, ALayer2D parentLayer, CardEntity2D parentCard) :
            base(parentLayer, false)
        {

            Shader shader = new Shader(null, null, @"Assets\Graphics\Shaders\StarFrag.frag");

            Texture distortionMap = factory.GetTextureByIndex(0);
            distortionMap.Repeated = true;
            distortionMap.Smooth = true;
            shader.SetUniform("currentTexture", new Shader.CurrentTextureType());
            shader.SetUniform("distortionMapTexture", distortionMap);

            render = new RenderStates(BlendMode.Alpha);
            render.Shader = shader;

            this.SpriteColor = Color.Yellow;
            this.ObjectSprite.Texture = factory.GetTextureByIndex(2);

            this.ObjectSprite.Origin = new SFML.System.Vector2f(this.ObjectSprite.TextureRect.Width / 2, this.ObjectSprite.TextureRect.Height / 2);

            // Active animation
            SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(4), AnimationType.LOOP);

            IAnimation anim = new ZoomAnimation(3, 3.5f, Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(0, anim);

            anim = new ZoomAnimation(3.5f, 3, Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            sequence.AddAnimation(2, anim);

            this.animationsList.Add(sequence);

            this.Initialize(parentCard);
        }

        private void Initialize(CardEntity2D entity)
        {
            this.HaloState = CardHaloState.NOT_ACTIVE;
            this.Position = entity.Position;
        }

        private IAnimation CreateTransitioningAnimation()
        {
            IAnimation result;
            if (this.isActive)
            {
                float time = Math.Abs(3 - this.Zoom);
                result = new ZoomAnimation(this.Zoom, 3f, Time.FromSeconds(time), AnimationType.ONETIME, InterpolationMethod.SQUARE_DEC);
            }
            else
            {
                float time = Math.Abs(this.Zoom);
                result = new ZoomAnimation(this.Zoom, 0f, Time.FromSeconds(time), AnimationType.ONETIME, InterpolationMethod.SQUARE_DEC);
            }

            return result;
        }

        private void StartTransitioningState()
        {
            this.HaloState = CardHaloState.TRANSITIONING;

            this.PlayAnimation(this.CreateTransitioningAnimation());
        }

        private void StartNotActiveState()
        {
            this.HaloState = CardHaloState.NOT_ACTIVE;
        }

        private void StartActiveState()
        {
            this.HaloState = CardHaloState.ACTIVE;

            this.PlayAnimation(0);
        }

        private void UpdateTransitioning(Time deltaTime)
        {
            if (this.IsAnimationRunning() == false)
            {
                if (this.isActive)
                {
                    this.StartActiveState();
                }
                else
                {
                    this.StartNotActiveState();
                }
            }
        }

        private void UpdateActive(Time deltaTime)
        {
            if (this.isActive == false)
            {
                this.StartTransitioningState();
            }
        }

        private void UpdateNotActive(Time deltaTime)
        {
            if (this.isActive)
            {
                this.StartTransitioningState();
            }
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            switch (this.HaloState)
            {
                case CardHaloState.ACTIVE:
                    this.UpdateActive(deltaTime);
                    break;
                case CardHaloState.TRANSITIONING:
                    this.UpdateTransitioning(deltaTime);
                    break;
                case CardHaloState.NOT_ACTIVE:
                    this.UpdateNotActive(deltaTime);
                    break;
            }

            render.Shader.SetUniform("time", timer.ElapsedTime.AsSeconds());
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            if (this.IsActive)
            {
                window.Draw(this.ObjectSprite, this.render);
            }
        }

        public enum CardHaloState
        {
            NOT_ACTIVE,
            TRANSITIONING,
            ACTIVE
        }
    }
}
