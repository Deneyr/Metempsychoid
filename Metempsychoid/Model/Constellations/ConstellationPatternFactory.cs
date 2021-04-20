using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Constellations
{
    public static class ConstellationPatternFactory
    {
        public static ConstellationPattern CreateDefaultConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationNode();
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationNode();
            patternToCreate.AddNode(node2);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(node1, node2);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link2);

            return patternToCreate;
        }

    }
}
