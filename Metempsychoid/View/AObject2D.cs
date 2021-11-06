using Astrategia.Animation;
using Astrategia.View.Animation;
using Astrategia.View.SoundsManager;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View
{
    public abstract class AObject2D: IObject2D
    {
        protected static AnimationManager animationManager;

        protected static SoundMusicPlayer soundMusicPlayer;

        protected IObject2DFactory parentFactory;

        protected List<IAnimation> animationsList;

        public event PropertyChangedEventHandler PropertyChanged;

        public abstract float Zoom
        {
            get;
            set;
        }

        public abstract Vector2f CustomZoom
        {
            get;
            set;
        }

        public abstract IntRect Canevas
        {
            get;
            set;
        }

        public abstract FloatRect Bounds
        {
            get;
        }

        public abstract Vector2f Position
        {
            get;
            set;
        }

        public abstract float Rotation
        {
            get;
            set;
        }

        public abstract Color SpriteColor
        {
            get;
            set;
        }

        static AObject2D()
        {
            AObject2D.animationManager = new AnimationManager();

            AObject2D.soundMusicPlayer = new SoundMusicPlayer();
        }

        public AObject2D(IObject2DFactory parentFactory)
        {
            this.parentFactory = parentFactory;

            this.animationsList = new List<IAnimation>();
        }

        public virtual void Dispose()
        {
            AObject2D.animationManager.StopAnimation(this);
            AObject2D.soundMusicPlayer.DisposeAudioObject2D(this);

            this.parentFactory = null;
        }

        public abstract void DrawIn(RenderWindow window, Time deltaTime);

        // Part animations.
        public static IntRect[] CreateAnimation(int leftStart, int topStart, int width, int height, int nbFrame)
        {
            IntRect[] result = new IntRect[nbFrame];

            for (int i = 0; i < nbFrame; i++)
            {
                result[i] = new IntRect(leftStart + i * width, topStart, width, height);
            }

            return result;
        }

        public void PlayAnimation(int index)
        {
            IAnimation animation = this.animationsList[index];

            AObject2D.animationManager.PlayAnimation(this, animation);
        }

        public void PlayAnimation(IAnimation animation)
        {
            AObject2D.animationManager.PlayAnimation(this, animation);
        }

        public virtual void PlaySound(string soundId)
        {
            // to override
        }

        public virtual bool IsAnimationRunning()
        {
            IAnimation animation = AObject2D.animationManager.GetAnimationFromAObject2D(this);

            return animation != null;
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static void UpdateAnimationManager(Time deltaTime)
        {
            AObject2D.animationManager.Run(deltaTime);

            AObject2D.soundMusicPlayer.Run(deltaTime);
        }
    }
}
