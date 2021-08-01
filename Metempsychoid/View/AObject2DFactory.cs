using Metempsychoid.Model;
using Metempsychoid.View.Text2D;
using SFML.Audio;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View
{
    public abstract class AObject2DFactory: IObject2DFactory
    {
        private static Dictionary<string, Font> nameToFonts;
        private static Dictionary<Font, float> fontToWidths;

        private static readonly Texture BLANK_TEXTURE;
        private static readonly SoundBuffer BLANK_SOUND;

        private Dictionary<string, Texture> resources;
        private Dictionary<string, SoundBuffer> sounds;

        protected HashSet<string> texturesPath;
        protected Dictionary<string, string> soundsPath;
        protected Dictionary<string, string> musicsPath;

        public Dictionary<string, Texture> Resources
        {
            get
            {
                return this.resources;
            }
        }

        public Dictionary<string, SoundBuffer> Sounds
        {
            get
            {
                return this.sounds;
            }
        }

        //public HashSet<string> MusicsPath
        //{
        //    get
        //    {
        //        return this.musicsPath;
        //    }
        //}

        static AObject2DFactory()
        {
            BLANK_TEXTURE = new Texture((uint)MainWindow.MODEL_TO_VIEW * 16, (uint)MainWindow.MODEL_TO_VIEW * 16);
            BLANK_SOUND = null;

            nameToFonts = new Dictionary<string, Font>();
            fontToWidths = new Dictionary<Font, float>();

            Font font = new Font(@"Assets\Graphics\Fonts\ProtectorBy7NTypes.otf");
            nameToFonts.Add("Protector", font);
            fontToWidths.Add(font, 600);

            font = new Font(@"Assets\Graphics\Fonts\PERLS.ttf");
            nameToFonts.Add("perls", font);
            fontToWidths.Add(font, 250);

            font = new Font(@"Assets\Graphics\Fonts\SansByAaron.ttf");
            nameToFonts.Add("Sans", font);
            fontToWidths.Add(font, 190);

            font = new Font(@"Assets\Graphics\Fonts\nzoda.ttf");
            nameToFonts.Add("Nzoda", font);
            fontToWidths.Add(font, 200);

            //InitFontToWidths();
        }

        private static void InitFontToWidths()
        {
            fontToWidths = new Dictionary<Font, float>();

            foreach(Font font in nameToFonts.Values)
            {
                Text text = new Text(" ", font, 1000);
                fontToWidths.Add(font, text.GetGlobalBounds().Width);
            }
        }

        public static int GetWidthFromTextToken(TextToken2D textToken2D)
        {
            return (int) (fontToWidths[textToken2D.TextFont] * textToken2D.CharacterSize / 1000f);
        }

        public static Font GetFontByName(string fontName)
        {
            return nameToFonts[fontName];
        }

        public AObject2DFactory()
        {
            this.texturesPath = new HashSet<string>();
            this.soundsPath = new Dictionary<string, string>();
            this.musicsPath = new Dictionary<string, string>();
        }

        protected void InitializeFactory()
        {
            this.resources = new Dictionary<string, Texture>();
            this.sounds = new Dictionary<string, SoundBuffer>();

            foreach (string texturesPath in this.texturesPath)
            {
                this.resources.Add(texturesPath, BLANK_TEXTURE);
            }

            foreach (string soundsPath in this.soundsPath.Values)
            {
                this.sounds.Add(soundsPath, BLANK_SOUND);
            }
        }

        public abstract IObject2D CreateObject2D(World2D world2D, IObject obj);

        public virtual IObject2D CreateObject2D(World2D world2D, ALayer2D layer2D, IObject obj)
        {
            return this.CreateObject2D(world2D, obj);
        }

        public Texture GetTextureByIndex(int index)
        {
            return this.Resources[this.texturesPath.ElementAt(index)];
        }

        public SoundBuffer GetSoundById(string id)
        {
            if (this.soundsPath.TryGetValue(id, out string soundPath))
            {
                return this.Sounds[soundPath];
            }
            return null;
        }

        public string GetMusicPathById(string id)
        {
            if (this.musicsPath.TryGetValue(id, out string musicPath))
            {
                return musicPath;
            }
            return null;
        }

        public virtual void OnTextureLoaded(string path, Texture texture)
        {
            if (this.Resources.ContainsKey(path))
            {
                this.Resources[path] = texture;
            }
        }

        public virtual void OnTextureUnloaded(string path)
        {
            if (this.Resources.ContainsKey(path))
            {
                this.Resources[path] = BLANK_TEXTURE;
            }
        }

        public virtual void OnSoundLoaded(string path, SoundBuffer sound)
        {
            if (this.Sounds.ContainsKey(path))
            {
                this.Sounds[path] = sound;
            }
        }

        public virtual void OnSoundUnloaded(string path)
        {
            if (this.Sounds.ContainsKey(path))
            {
                this.Sounds[path] = BLANK_SOUND;
            }
        }
    }
}
