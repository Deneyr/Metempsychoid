using SFML.Audio;
using SFML.System;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.SoundsManager
{
    public class MusicObject2D : AAudioObject2D
    {
        private Music music;

        //
        // Résumé :
        //     3D position of the sound in the audio scene. Only sounds with one channel (mono
        //     sounds) can be spatialized. The default position of a sound is (0, 0, 0).
        public override Vector2f Position
        {
            get
            {
                return new Vector2f(this.music.Position.X, this.music.Position.Z);
            }
            set
            {
                this.music.Position = new Vector3f(value.X, 0, value.Y);
            }
        }

        //
        // Résumé :
        //     Current playing position of the sound. The playing position can be changed when
        //     the sound is either paused or playing.
        public Time PlayingOffset
        {
            get
            {
                return this.music.PlayingOffset;
            }
            set
            {
                this.music.PlayingOffset = value;
            }
        }

        //
        // Résumé :
        //     Volume of the sound. The volume is a value between 0 (mute) and 100 (full volume).
        //     The default value for the volume is 100.
        public override float Volume
        {
            get
            {
                return this.music.Volume;
            }
            set
            {
                this.music.Volume = value;
            }
        }

        //
        // Résumé :
        //     Pitch of the sound. The pitch represents the perceived fundamental frequency
        //     of a sound; thus you can make a sound more acute or grave by changing its pitch.
        //     A side effect of changing the pitch is to modify the playing speed of the sound
        //     as well. The default value for the pitch is 1.
        public float Pitch
        {
            get
            {
                return this.music.Pitch;
            }
            set
            {
                this.music.Pitch = value;
            }
        }

        //
        // Résumé :
        //     Flag if the sound should loop after reaching the end. If set, the sound will
        //     restart from beginning after reaching the end and so on, until it is stopped
        //     or Loop = false is set. The default looping state for sounds is false.
        public bool Loop
        {
            get
            {
                return this.music.Loop;
            }
            set
            {
                this.music.Loop = value;
            }
        }

        //
        // Résumé :
        //     Minimum distance of the sound. The "minimum distance" of a sound is the maximum
        //     distance at which it is heard at its maximum volume. Further than the minimum
        //     distance, it will start to fade out according to its attenuation factor. A value
        //     of 0 ("inside the head of the listener") is an invalid value and is forbidden.
        //     The default value of the minimum distance is 1.
        public float MinDistance
        {
            get
            {
                return this.music.MinDistance;
            }
            set
            {
                this.music.MinDistance = value;
            }
        }

        //
        // Résumé :
        //     Current status of the sound (see SoundStatus enum)
        public SoundStatus Status
        {
            get
            {
                return this.music.Status;
            }
        }

        //
        // Résumé :
        //     Attenuation factor of the music. The attenuation is a multiplicative factor which
        //     makes the music more or less loud according to its distance from the listener.
        //     An attenuation of 0 will produce a non-attenuated sound, i.e. its volume will
        //     always be the same whether it is heard from near or from far. On the other hand,
        //     an attenuation value such as 100 will make the sound fade out very quickly as
        //     it gets further from the listener. The default value of the attenuation is 1.
        public float Attenuation
        {
            get
            {
                return this.music.Attenuation;
            }
            set
            {
                this.music.Attenuation = value;
            }
        }

        public MusicObject2D(ALayer2D parentLayer, IObject2D owner, string musicPath)
            : base(parentLayer, owner)
        {
            this.music = new Music(musicPath);
        }

        //
        // Résumé :
        //     Pause the sound. This function pauses the sound if it was playing, otherwise
        //     (sound already paused or stopped) it has no effect.
        public void Pause()
        {
            this.music.Pause();
        }
        //
        // Résumé :
        //     Start or resume playing the sound. This function starts the stream if it was
        //     stopped, resumes it if it was paused, and restarts it from beginning if it was
        //     it already playing. This function uses its own thread so that it doesn't block
        //     the rest of the program while the sound is played.
        public void Play()
        {
            this.music.Play();
        }
        //
        // Résumé :
        //     Stop playing the sound. This function stops the sound if it was playing or paused,
        //     and does nothing if it was already stopped. It also resets the playing position
        //     (unlike pause()).
        public void Stop()
        {
            this.music.Stop();
        }

        public override void Dispose()
        {
            base.Dispose();

            this.music.Dispose();
        }
    }
}