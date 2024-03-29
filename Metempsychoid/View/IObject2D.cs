﻿using Astrategia.Model;
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
    public interface IObject2D: IObject, IDisposable, INotifyPropertyChanged
    {
        float Zoom
        {
            get;
            set;
        }

        Vector2f CustomZoom
        {
            get;
            set;
        }

        IntRect Canevas
        {
            get;
            set;
        }

        Color SpriteColor
        {
            get;
            set;
        }

        FloatRect Bounds
        {
            get;
        }

        void DrawIn(RenderWindow window, Time deltaTime);
    }
}
