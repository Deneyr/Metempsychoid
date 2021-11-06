using Astrategia.Model;
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
    public interface IAIObject: IObject, IDisposable, INotifyPropertyChanged
    {
        void UpdateAI(Time deltaTime);
    }
}
