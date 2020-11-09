using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model
{
    public interface IUpdatable
    {
        void UpdateLogic(World world, Time deltaTime);
    }
}

