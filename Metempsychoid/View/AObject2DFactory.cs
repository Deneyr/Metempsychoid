﻿using Metempsychoid.Model;
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
        private static readonly Texture BLANK_TEXTURE;

        private Dictionary<string, Texture> resources;

        protected HashSet<string> texturesPath;

        static AObject2DFactory()
        {
            BLANK_TEXTURE = new Texture((uint)MainWindow.MODEL_TO_VIEW * 16, (uint)MainWindow.MODEL_TO_VIEW * 16);
        }

        public AObject2DFactory()
        {
            this.texturesPath = new HashSet<string>();
        }

        protected void InitializeFactory()
        {
            this.resources = new Dictionary<string, Texture>();
            Texture blankTexture = BLANK_TEXTURE;
            foreach (string texturesPath in this.texturesPath)
            {
                this.resources.Add(texturesPath, blankTexture);
            }
        }

        public abstract IObject2D CreateObject2D(World2D world2D, IObject obj);

        public Dictionary<string, Texture> Resources
        {
            get
            {
                return this.resources;
            }
        }

        public Texture GetTextureByIndex(int index)
        {
            return this.Resources[this.texturesPath.ElementAt(index)];
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
    }
}