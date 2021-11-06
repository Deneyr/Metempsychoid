using Astrategia.Model.Layer.BoardGameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Constellations
{
    public class ConstellationNode
    {
        public string Name
        {
            get;
            set;
        }

        public virtual bool IsStarValid(StarEntity star, StarEntity startStarEntity)
        {
            return star.CardSocketed != null;
        }

    }
}
