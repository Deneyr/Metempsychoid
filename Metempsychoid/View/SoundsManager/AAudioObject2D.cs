using Astrategia.Animation;
using Astrategia.View.Animation;
using SFML.System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.SoundsManager
{
    public abstract class AAudioObject2D : AEntity2D
    {
        private float FADE_PERIOD = 5;
        private int VOLUME = 50;

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
            parentLayer.ViewChanged += this.OnViewChanged;

            IAnimation fadeInAnimation = new AudioVolumeAnimation(0, VOLUME, SFML.System.Time.FromSeconds(FADE_PERIOD), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            this.animationsList.Add(fadeInAnimation);

            IAnimation fadeOutAnimation = new AudioVolumeAnimation(VOLUME, 0, SFML.System.Time.FromSeconds(FADE_PERIOD), AnimationType.ONETIME, InterpolationMethod.LINEAR);
            this.animationsList.Add(fadeOutAnimation);
        }

        private void OnViewChanged(ALayer2D obj)
        {
            this.Position = new Vector2f(obj.GetNormalizedPositionInWindow(this.Owner.Position).X, 1f);
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

            if(this.parentLayer.TryGetTarget(out ALayer2D parentLayer))
            {
                parentLayer.ViewChanged -= this.OnViewChanged;
            }
        }
    }
}
