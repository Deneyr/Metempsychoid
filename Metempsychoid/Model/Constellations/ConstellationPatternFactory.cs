using Metempsychoid.Model.Layer.BoardGameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Constellations
{
    public static class ConstellationPatternFactory
    {
        // Meta

        public static ConstellationPattern CreateBeginningConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationOpponentNode();
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationOpponentNode();
            patternToCreate.AddNode(node2);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node2);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link2);
            ConstellationLink link3 = new ConstellationLink(node1, node2);
            patternToCreate.AddNodeLink(link3);

            return patternToCreate;
        }

        public static ConstellationPattern CreateRulesConstellation()
        {
            return CreateFoolConstellation();
        }

        public static ConstellationPattern CreateArchitectsConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationAllyNode();
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationAllyNode();
            patternToCreate.AddNode(node2);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(node1, node2);
            patternToCreate.AddNodeLink(link2);

            return patternToCreate;
        }

        public static ConstellationPattern CreateToolsConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationAllyNode();
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationAllyNode();
            patternToCreate.AddNode(node2);
            ConstellationNode node3 = new ConstellationOpponentNode();
            patternToCreate.AddNode(node2);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(self, node2);
            patternToCreate.AddNodeLink(link2);
            ConstellationLink link3 = new ConstellationLink(self, node3);
            patternToCreate.AddNodeLink(link3);

            return patternToCreate;
        }

        // Intra

        public static ConstellationPattern CreateStrengthConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationAllyNode();
            patternToCreate.AddNode(node1);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link);

            return patternToCreate;
        }

        public static ConstellationPattern CreateJusticeConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationNode();
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationNode();
            patternToCreate.AddNode(node2);
            ConstellationNode node3 = new ConstellationNode();
            patternToCreate.AddNode(node3);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node2);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link2);
            ConstellationLink link3 = new ConstellationLink(self, node3);
            patternToCreate.AddNodeLink(link3);

            return patternToCreate;
        }

        public static ConstellationPattern CreateMoonConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationAllyNode();
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationAllyNode();
            patternToCreate.AddNode(node2);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node2);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link2);
            ConstellationLink link3 = new ConstellationLink(node1, node2);
            patternToCreate.AddNodeLink(link3);

            return patternToCreate;
        }

        public static ConstellationPattern CreateDeathConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationNode();
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationNode();
            patternToCreate.AddNode(node2);
            ConstellationNode node3 = new ConstellationOpponentNode();
            patternToCreate.AddNode(node3);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node2);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link2);
            ConstellationLink link3 = new ConstellationLink(self, node3);
            patternToCreate.AddNodeLink(link3);

            return patternToCreate;
        }

        public static ConstellationPattern CreateLoverConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationOpponentNode();
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationOpponentNode();
            patternToCreate.AddNode(node2);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node2);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link2);
            ConstellationLink link3 = new ConstellationLink(node1, node2);
            patternToCreate.AddNodeLink(link3);

            return patternToCreate;
        }

        public static ConstellationPattern CreatePriestessConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationAllyNode();
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationAllyNode();
            patternToCreate.AddNode(node2);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node2);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link2);

            return patternToCreate;
        }

        public static ConstellationPattern CreateTemperanceConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationNode();
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationNode();
            patternToCreate.AddNode(node2);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node2);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link2);

            return patternToCreate;
        }

        public static ConstellationPattern CreateCartConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationNode();
            patternToCreate.AddNode(node1);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link);

            return patternToCreate;
        }

        public static ConstellationPattern CreateDevilConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationOpponentNode();
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationOpponentNode();
            patternToCreate.AddNode(node2);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node2);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link2);

            return patternToCreate;
        }

        public static ConstellationPattern CreateFoolConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationOpponentNode();
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationAllyNode();
            patternToCreate.AddNode(node2);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node2);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link2);

            return patternToCreate;
        }

        public static ConstellationPattern CreateHierophantConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationNode();
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationNode();
            patternToCreate.AddNode(node2);

            ConstellationNode node3 = new ConstellationNode();
            patternToCreate.AddNode(node3);
            ConstellationNode node4 = new ConstellationNode();
            patternToCreate.AddNode(node4);

            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(node1, node2);
            patternToCreate.AddNodeLink(link2);

            ConstellationLink link3 = new ConstellationLink(self, node3);
            patternToCreate.AddNodeLink(link3);
            ConstellationLink link4 = new ConstellationLink(node3, node4);
            patternToCreate.AddNodeLink(link4);

            return patternToCreate;
        }

        public static ConstellationPattern CreateMagicianConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationNode();
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationAllyNode();
            patternToCreate.AddNode(node2);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node2);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link2);

            return patternToCreate;
        }

        public static ConstellationPattern CreateWorldConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationAllyNode();
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationAllyNode();
            patternToCreate.AddNode(node2);

            ConstellationNode node3 = new ConstellationAllyNode();
            patternToCreate.AddNode(node3);
            ConstellationNode node4 = new ConstellationAllyNode();
            patternToCreate.AddNode(node4);

            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(self, node2);
            patternToCreate.AddNodeLink(link2);

            ConstellationLink link3 = new ConstellationLink(self, node3);
            patternToCreate.AddNodeLink(link3);
            ConstellationLink link4 = new ConstellationLink(self, node4);
            patternToCreate.AddNodeLink(link4);

            return patternToCreate;
        }

        public static ConstellationPattern CreateEmperorConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationOpponentNode();
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationSpecificCardNode("empress", ConstellationSpecificCardNode.NodeType.ALLY);
            patternToCreate.AddNode(node2);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node2);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link2);

            return patternToCreate;
        }

        public static ConstellationPattern CreateHangedManConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationOpponentNode();
            patternToCreate.AddNode(node1);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link);

            return patternToCreate;
        }

        public static ConstellationPattern CreateHermiteConstellation()
        {
            return CreateCartConstellation();
        }

        public static ConstellationPattern CreateRockConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationLockNode();
            patternToCreate.AddNode(node1);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link);

            return patternToCreate;
        }

        public static ConstellationPattern CreateEmpressConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationAllyNode();
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationSpecificCardNode("emperor", ConstellationSpecificCardNode.NodeType.ALLY);
            patternToCreate.AddNode(node2);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node2);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link2);

            return patternToCreate;
        }

        public static ConstellationPattern CreateWheelConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationNode();
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationNode();
            patternToCreate.AddNode(node2);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(node1, node2);
            patternToCreate.AddNodeLink(link2);

            return patternToCreate;
        }

        public static ConstellationPattern CreateMinimalConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            return patternToCreate;
        }

        public static ConstellationPattern CreateTowerConstellation()
        {
            return CreateTemperanceConstellation();
        }

        public static ConstellationPattern CreateJudgementConstellation()
        {
            return CreateFoolConstellation();
        }

        public static ConstellationPattern CreateSunConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationAllyNode();
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationAllyNode();
            patternToCreate.AddNode(node2);
            ConstellationNode node3 = new ConstellationNode();
            patternToCreate.AddNode(node3);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node2);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link2);
            ConstellationLink link3 = new ConstellationLink(node2, node3);
            patternToCreate.AddNodeLink(link3);
            ConstellationLink link4 = new ConstellationLink(node1, node3);
            patternToCreate.AddNodeLink(link4);

            return patternToCreate;
        }

        public static ConstellationPattern CreateStarConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationAllyNode();
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationNode();
            patternToCreate.AddNode(node2);

            ConstellationNode node3 = new ConstellationNode();
            patternToCreate.AddNode(node3);
            ConstellationNode node4 = new ConstellationOpponentNode();
            patternToCreate.AddNode(node4);

            ConstellationNodeSelf self = new ConstellationNodeSelf();
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(self, node2);
            patternToCreate.AddNodeLink(link2);

            ConstellationLink link3 = new ConstellationLink(self, node3);
            patternToCreate.AddNodeLink(link3);
            ConstellationLink link4 = new ConstellationLink(self, node4);
            patternToCreate.AddNodeLink(link4);

            return patternToCreate;
        }

        public static ConstellationPattern CreateDefaultConstellation()
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            ConstellationNode node1 = new ConstellationNode();
            node1.Name = "head1";
            patternToCreate.AddNode(node1);
            ConstellationNode node2 = new ConstellationNode();
            node2.Name = "head2";
            patternToCreate.AddNode(node2);
            //ConstellationNode node3 = new ConstellationNode();
            //node3.Name = "tail";
            //patternToCreate.AddNode(node3);
            ConstellationNodeSelf self = new ConstellationNodeSelf();
            self.Name = "self";
            patternToCreate.AddNode(self);

            ConstellationLink link = new ConstellationLink(self, node2);
            patternToCreate.AddNodeLink(link);
            ConstellationLink link2 = new ConstellationLink(self, node1);
            patternToCreate.AddNodeLink(link2);
            //ConstellationLink link3 = new ConstellationLink(node1, node2);
            //patternToCreate.AddNodeLink(link3);
            //ConstellationLink link4 = new ConstellationLink(self, node3);
            //patternToCreate.AddNodeLink(link4);

            return patternToCreate;
        }

        public static ConstellationPattern CreateAssociatedCardsPatternFrom(ConstellationPattern modelPattern, IConstellation constellation)
        {
            ConstellationPattern patternToCreate = new ConstellationPattern();

            Dictionary<ConstellationNode, ConstellationNode> nodeToNewNode = new Dictionary<ConstellationNode, ConstellationNode>();

            ConstellationNode newNode;
            foreach (ConstellationNode node in modelPattern.ConstellationNodeSystem)
            {
                if (node != modelPattern.NodeSelf)
                {
                    StarEntity associatedStarEntity = constellation.NodeToStarEntity[node];

                    newNode = new ConstellationCardEntityNode(associatedStarEntity.CardSocketed);
                    nodeToNewNode.Add(node, newNode);

                    patternToCreate.AddNode(newNode);
                }
            }

            newNode = new ConstellationNodeSelf();
            nodeToNewNode.Add(modelPattern.NodeSelf, newNode);
            patternToCreate.AddNode(newNode);

            foreach (ConstellationLink link in modelPattern.ConstellationLinkSystem)
            {
                patternToCreate.AddNodeLink(new ConstellationLink(nodeToNewNode[link.Node1], nodeToNewNode[link.Node2]));
            }

            return patternToCreate;
        }

    }
}
