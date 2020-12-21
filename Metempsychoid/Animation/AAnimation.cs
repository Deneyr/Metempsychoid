using Metempsychoid.Model;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Metempsychoid.Animation
{
    public abstract class AAnimation: IAnimation
    {
        private float valueFrom;
        private float valueTo;
        protected float currentValue;

        private Time animationPeriod;
        private Time timeElapsed;

        private AnimationType type;

        private InterpolationMethod method;

        public AAnimation(float valueFrom, float valueTo, Time animationPeriod, AnimationType type, InterpolationMethod method)
        {
            this.animationPeriod = animationPeriod;
            this.timeElapsed = Time.Zero;

            this.valueFrom = valueFrom;
            this.valueTo = valueTo;
            this.currentValue = valueFrom;

            this.State = AnimationState.STARTING;

            this.type = type;

            this.method = method;
        }

        public AnimationState State
        {
            get;
            private set;
        }

        public void Run(Time deltaTime)
        {

            switch (this.State)
            {
                case AnimationState.STARTING:
                    this.State = AnimationState.RUNNING;

                    this.Iterate(deltaTime);
                    break;
                case AnimationState.RUNNING:
                    this.Iterate(deltaTime);

                    if (this.timeElapsed >= this.animationPeriod)
                    {
                        this.State = AnimationState.FINALIZING;
                    }
                    break;
                case AnimationState.FINALIZING:

                    if (this.type == AnimationType.LOOP)
                    {
                        this.timeElapsed = Time.Zero;

                        this.State = AnimationState.RUNNING;

                        this.Iterate(deltaTime);
                    }
                    else
                    {
                        this.State = AnimationState.ENDING;
                    }
                    break;
                case AnimationState.ENDING:
                    break;
            }
        }

        private void Iterate(Time deltaTime)
        {
            this.timeElapsed += deltaTime;

            if(this.timeElapsed >= this.animationPeriod)
            {
                this.currentValue = this.valueTo;
            }
            else
            {
                float ratioElapsed = this.timeElapsed.AsSeconds() / this.animationPeriod.AsSeconds();
                switch (this.method)
                {
                    case InterpolationMethod.LINEAR:
                        this.currentValue = this.valueFrom * (1 - ratioElapsed) + this.valueTo * ratioElapsed;
                        break;
                    case InterpolationMethod.SQUARE_ACC:
                        this.currentValue = this.valueFrom * (1 - ratioElapsed * ratioElapsed) + this.valueTo * ratioElapsed * ratioElapsed;
                        break;
                    case InterpolationMethod.SQUARE_DEC:
                        this.currentValue = this.valueFrom * (ratioElapsed - 1) * (ratioElapsed - 1) + this.valueTo * (ratioElapsed * (2 - ratioElapsed));
                        break;
                    case InterpolationMethod.SIGMOID:
                        this.currentValue = this.valueFrom + ((this.valueTo - this.valueFrom) * (1.0132f / (1 + (float) Math.Exp(-10 * (ratioElapsed - 0.5)))) - 0.0068f);
                        break;
                }
            }
        }

        public void Reset()
        {
            this.timeElapsed = Time.Zero;

            this.currentValue = valueFrom;

            this.State = AnimationState.STARTING;
        }

        public void Stop(bool reset)
        {
            if (reset)
            {
                this.currentValue = 1;
            }

            this.State = AnimationState.ENDING;
        }

        public abstract void Visit(IObject parentObject);
    }
}
