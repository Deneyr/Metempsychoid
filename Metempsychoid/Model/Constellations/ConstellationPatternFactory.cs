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
            node1.Name = "head1";
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationNode();
            node2.Name = "head2";
            patternToCreate.AddNode(node2);
            ConstellationNode node3 = new ConstellationNode();
            node3.Name = "tail";
            patternToCreate.AddNode(node3);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            self.Name = "self";
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node2);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link2);
            ConstellationLink link3 = new ConstellationLink(node1, node2);
            patternToCreate.AddNodeLink(link3);
            ConstellationLink link4 = new ConstellationLink(self, node3);
            patternToCreate.AddNodeLink(link4);

            return patternToCreate;
        }

    }
}
