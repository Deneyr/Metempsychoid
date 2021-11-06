using Astrategia.Model;
using SFML.Audio;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.AI
{
    public interface IAIObjectFactory
    {
        IAIObject CreateObjectAI(AIWorld worldAI, IObject obj);

        IAIObject CreateObjectAI(AIWorld worldAI, AAILayer layerAI, IObject obj);
    }
}
