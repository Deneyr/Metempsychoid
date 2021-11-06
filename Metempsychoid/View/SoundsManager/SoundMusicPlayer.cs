using Astrategia.Model;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.SoundsManager
{
    public class SoundMusicPlayer
    {
        private Dictionary<IObject2D, List<SoundObject2D>> object2DToSounds;
        private List<SoundObject2D> oldSounds;

        private Dictionary<IObject2D, MusicObject2D> object2DToMusics;
        private List<MusicObject2D> oldMusics;

        //static SoundMusicPlayer()
        //{
        //    Listener.Position (10.f, 0.f, 5.f);
        //}

        public SoundMusicPlayer()
        {
            this.object2DToSounds = new Dictionary<IObject2D, List<SoundObject2D>>();
            this.oldSounds = new List<SoundObject2D>();

            this.object2DToMusics = new Dictionary<IObject2D, MusicObject2D>();
            this.oldMusics = new List<MusicObject2D>();
        }

        public void PlaySound(SoundObject2D soundToPlay, bool isLooping = false)
        {
            List<SoundObject2D> sounds = null;

            if (this.object2DToSounds.TryGetValue(soundToPlay.Owner, out sounds) == false)
            {
                sounds = new List<SoundObject2D>();

                this.object2DToSounds[soundToPlay.Owner] = sounds;
            }

            sounds.Add(soundToPlay);

            soundToPlay.Loop = isLooping;
            soundToPlay.Play();
        }

        public void StopSounds(IObject2D owner)
        {
            if (this.object2DToSounds.TryGetValue(owner, out List<SoundObject2D> sounds))
            {
                foreach (SoundObject2D sound in sounds)
                {
                    sound.Dispose();
                }

                this.object2DToSounds.Remove(owner);
            }
        }

        public void RemoveSound(SoundObject2D soundToRemove)
        {
            if (this.object2DToSounds.TryGetValue(soundToRemove.Owner, out List<SoundObject2D> sounds))
            {
                sounds.Remove(soundToRemove);

                if(sounds.Count == 0)
                {
                    this.object2DToSounds.Remove(soundToRemove.Owner);
                }
            }

            soundToRemove.Stop();

            soundToRemove.Dispose();
        }

        public void PlayMusic(MusicObject2D musicToPlay, bool isLooping)
        {
            if (this.object2DToMusics.TryGetValue(musicToPlay.Owner, out MusicObject2D music))
            {
                music.PlayAnimation(1);

                this.oldMusics.Add(music);

                this.object2DToMusics[musicToPlay.Owner] = musicToPlay;
            }
            else
            {
                this.object2DToMusics.Add(musicToPlay.Owner, musicToPlay);
            }

            musicToPlay.Volume = 0;
            musicToPlay.PlayAnimation(0);

            musicToPlay.Loop = isLooping;
            musicToPlay.Play();
        }

        public void StopMusics(IObject2D owner)
        {
            if (this.object2DToMusics.TryGetValue(owner, out MusicObject2D music))
            {
                music.PlayAnimation(1);

                this.oldMusics.Add(music);
            }
        }

        public void RemoveMusic(MusicObject2D musicToRemove)
        {
            this.object2DToMusics.Remove(musicToRemove);

            musicToRemove.Stop();

            musicToRemove.Dispose();
        }

        //public void OnObject2DPropertyChanged(IObject2D objectConcerned, string propertyName)
        //{
        //    if(this.object2DToSounds.TryGetValue(objectConcerned, out SoundObject2D sound))
        //    {
        //        sound.OnOwnerPropertyChanged(objectConcerned, propertyName);
        //    }

        //    if (this.object2DToMusics.TryGetValue(objectConcerned, out SoundObject2D music))
        //    {
        //        music.OnOwnerPropertyChanged(objectConcerned, propertyName);
        //    }
        //}

        public void DisposeAudioObject2D(IObject2D object2DToDispose)
        {
            this.StopSounds(object2DToDispose);

            this.StopMusics(object2DToDispose);
        }

        public void Run(Time deltaTime)
        {
            // Part sounds
            foreach(List<SoundObject2D> sounds in this.object2DToSounds.Values)
            {
                foreach (SoundObject2D sound in sounds)
                {
                    if(sound.Status == SoundStatus.Stopped)
                    {
                        this.oldSounds.Add(sound);
                    }
                }
            }

            foreach(SoundObject2D sound in this.oldSounds)
            {
                this.RemoveSound(sound);
            }
            this.oldSounds.Clear();

            // Part musics
            foreach (MusicObject2D music in this.object2DToMusics.Values)
            {
                if (music.Status == SoundStatus.Stopped)
                {
                    this.oldMusics.Add(music);
                }
            }

            List<MusicObject2D> musicsToRemove = this.oldMusics.Where(pElem => pElem.Status == SoundStatus.Stopped || pElem.Volume < float.Epsilon).ToList();

            foreach (MusicObject2D music in musicsToRemove)
            {
                this.oldMusics.Remove(music);

                this.RemoveMusic(music);
            }
        }
    }
}
