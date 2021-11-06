using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model
{
    public interface IObject
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
    }
}
