﻿using Astrategia.Animation;
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

namespace Astrategia.AI
{
    public abstract class AAIObject: IAIObject
    {
        protected IAIObjectFactory parentFactory;

        public event PropertyChangedEventHandler PropertyChanged;

        public readonly object objectLock = new object();

        public virtual Vector2f Position
        {
            get;
            set;
        }

        public virtual float Rotation
        {
            get;
            set;
        }

        public AAIObject(IAIObjectFactory parentFactory)
        {
            this.parentFactory = parentFactory;
        }

        public virtual void Dispose()
        {
            this.parentFactory = null;
        }

        public abstract void UpdateAI(Time deltaTime);

        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
