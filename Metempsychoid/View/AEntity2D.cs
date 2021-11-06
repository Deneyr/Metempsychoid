using Astrategia.Model;
using Astrategia.Model.Event;
using Astrategia.View.Animation;
using Astrategia.View.Controls;
using Astrategia.View.SoundsManager;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View
{
    public abstract class AEntity2D: AObject2D
    {
        protected static Clock timer = new Clock();

        protected Sprite sprite;

        protected WeakReference<ALayer2D> parentLayer;

        public int Priority
        {
            get;
            set;
        }

        public Sprite ObjectSprite
        {
            get
            {
                return this.sprite;
            }

            protected set
            {
                this.sprite = value;
            }
        }

        public override Vector2f Position
        {
            get
            {
                return this.sprite.Position;
            }

            set
            {
                this.sprite.Position = value * MainWindow.MODEL_TO_VIEW;

                this.NotifyPropertyChanged("Position");
            }
        }

        public override float Rotation
        {
            get
            {
                return this.sprite.Rotation;
            }

            set
            {
                this.sprite.Rotation = value;
            }
        }

        public override float Zoom
        {
            get
            {
                return this.CustomZoom.X;
            }
            set
            {
                this.CustomZoom = new Vector2f(value, value);
            }
        }

        public override Vector2f CustomZoom
        {
            get
            {
                return this.sprite.Scale;
            }
            set
            {
                this.sprite.Scale = value;
            }
        }

        public override Color SpriteColor
        {
            get
            {
                return this.sprite.Color;
            }
            set
            {
                this.sprite.Color = value;
            }
        }

        public override IntRect Canevas
        {
            get
            {
                return this.sprite.TextureRect;
            }
            set
            {
                if (this.sprite.TextureRect != value)
                {
                    this.sprite.TextureRect = value;
                }
            }
        }

        public virtual bool IsActive
        {
            get;
            set;
        }

        public override FloatRect Bounds
        {
            get
            {
                return this.ObjectSprite.GetGlobalBounds();
            }
        }

        public AEntity2D(ALayer2D parentLayer, IObject2DFactory factory, bool isActive)
            : base(factory)
        {
            this.parentLayer = new WeakReference<ALayer2D>(parentLayer);

            this.sprite = new Sprite();

            this.Priority = 0;
            this.IsActive = isActive;
        }

        public AEntity2D(ALayer2D parentLayer, IObject2DFactory factory, AEntity entity)
            : base(factory)
        {
            this.parentLayer = new WeakReference<ALayer2D>(parentLayer);

            this.sprite = new Sprite();

            this.Priority = 10;

            this.Position = entity.Position;
            this.Rotation = entity.Rotation;

            this.IsActive = entity.IsActive;
        }

        public override void PlaySound(string soundId)
        {
            if (this.parentLayer.TryGetTarget(out ALayer2D parentLayer))
            {
                SoundBuffer soundBuffer = null;

                if(this.parentFactory != null)
                {
                    soundBuffer = this.parentFactory.GetSoundById(soundId);
                }

                if (soundBuffer != null)
                {
                    AObject2D.soundMusicPlayer.PlaySound(new SoundObject2D(parentLayer, this, soundBuffer));
                }
                else
                {
                    parentLayer.PlaySound(soundId, this);
                }
            }
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            window.Draw(this.ObjectSprite);
        }

        public virtual void UpdateGraphics(Time deltaTime)
        {
            // To override
        }
    }
}
