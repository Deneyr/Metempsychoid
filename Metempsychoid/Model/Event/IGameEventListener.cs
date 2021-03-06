using Metempsychoid.Model.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Event
{
    public interface IGameEventListener
    {
        void OnGameEvent(World world, GameEvent gameEvent);
    }
}
