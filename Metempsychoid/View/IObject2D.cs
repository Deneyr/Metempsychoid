using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View
{
    public interface IObject2D: IDisposable
    {
        Vector2f Position
        {
            get;
            set;
        }

        float Rotation
        {
            get;
            set;
        }

        FloatRect Bounds
        {
            get;
        }

        void DrawIn(RenderWindow window, Time deltaTime);

        // Part animations
        void SetCanevas(IntRect newCanevas);

        void SetZoom(float newZoom);
    }
}
