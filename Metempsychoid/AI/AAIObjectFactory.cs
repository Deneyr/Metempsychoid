using Metempsychoid.Model;
using Metempsychoid.View.Text2D;
using SFML.Audio;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.AI
{
    public abstract class AAIObjectFactory: IAIObjectFactory
    {       
        public abstract IAIObject CreateObjectAI(AIWorld worldAI, IObject obj);

        public virtual IAIObject CreateObjectAI(AIWorld worldAI, AAILayer layerAI, IObject obj)
        {
            return this.CreateObjectAI(worldAI, obj);
        }
    }
}
