using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Metempsychoid.Model
{
    public abstract class ALayer : IObject
    {
        public Vector2f Position
        {
            get;
            protected set;
        }

        public HashSet<Type> TypesInChunk
        {
            get;
            protected set;
        }
    }
}
