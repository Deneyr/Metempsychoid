using Metempsychoid.Model.Layer.BoardGameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Constellations
{
    public class ConstellationLockNode : ConstellationNode
    {
        public override bool IsStarValid(StarEntity star, StarEntity startStarEntity)
        {
            return false;
        }
    }
}
