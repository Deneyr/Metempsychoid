using Metempsychoid.Model.Layer.BoardGameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Constellations
{
    public class ConstellationNode
    {
        public virtual bool IsStarValid(StarEntity star)
        {
            return star.CardSocketed != null;
        }

    }
}
