using Metempsychoid.Animation;
using Metempsychoid.View.Animation;
using SFML.System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.SoundsManager
{
    public abstract class AAudioObject2D : AEntity2D
    {
        private float FADE_PERIOD = 2;

        public IObject2D Owner
        {
            get;
            private set;
        }

        public abstract float Volume
        {
            get;
            set;
        }

        public AAudioObject2D(ALayer2D parentLayer, IObject2D owner) 
            : base(parentLayer, null, true)
        {
            this.Owner = owner;

            this.Owner.PropertyChanged += this.OnOwnerPropertyChanged;

            IAnimation fadeInAnimation = new AudioVolumeAnimation(0, 100, SFML.System.Time.FromSeconds(FADE_PERIOD), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            this.animationsList.Add(fadeInAnimation);

            IAnimation fadeOutAnimation = new AudioVolumeAnimation(100, 0, SFML.System.Time.FromSeconds(FADE_PERIOD), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            this.animationsList.Add(fadeOutAnimation);
        }

        public void OnOwnerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.parentLayer.TryGetTarget(out ALayer2D parentLayer)) {
                switch (e.PropertyName)
                {
                    case "Position":
                        this.Position = new Vector2f(parentLayer.GetNormalizedPositionInWindow((sender as IObject2D).Position).X, 1f);
                        break;
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            this.Owner.PropertyChanged -= this.OnOwnerPropertyChanged;
        }
    }
}
