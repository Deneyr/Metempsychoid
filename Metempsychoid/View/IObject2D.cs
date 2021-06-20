using Metempsychoid.Model;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View
{
    public interface IObject2D: IObject, IDisposable
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
