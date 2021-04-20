using Metempsychoid.Model.Card;
using Metempsychoid.Model.Layer.BoardGameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Constellations
{
    public class ConstellationPattern
    {
        private Dictionary<ConstellationNode, HashSet<ConstellationLink>> constellationNodeToLinks;

        public ConstellationNodeSelf NodeSelf
        {
            get;
            protected set;
        }

        public int Order
        {
            get;
            private set;
        }

        public HashSet<ConstellationNode> ConstellationNodeSystem
        {
            get;
            protected set;
        }

        public HashSet<ConstellationLink> ConstellationLinkSystem
        {
            get;
            protected set;
        }

        public ConstellationPattern()
        {
            this.ConstellationNodeSystem = new HashSet<ConstellationNode>();

            this.ConstellationLinkSystem = new HashSet<ConstellationLink>();

            this.constellationNodeToLinks = new Dictionary<ConstellationNode, HashSet<ConstellationLink>>();
        }

        public bool CreateConstellationSystem(BoardGameLayer boardGameLayer, StarEntity startStarEntity)
        {
            if(this.NodeSelf != null)
            {
                Tuple<ConstellationLink, ConstellationNode> firstTuple = new Tuple<ConstellationLink, ConstellationNode>(null, this.NodeSelf);

                Dictionary<Tuple<ConstellationLink, ConstellationNode>, Stack<StarEntity>> linkToPotentialStarEntities = new Dictionary<Tuple<ConstellationLink, ConstellationNode>, Stack<StarEntity>>();

                Dictionary<ConstellationNode, StarEntity> nodeToStarEntity = new Dictionary<ConstellationNode, StarEntity>();

                Stack<StarEntity> pathStarEntities = new Stack<StarEntity>();

                Stack<Tuple<ConstellationLink, ConstellationNode>> constellationStack = new Stack<Tuple<ConstellationLink, ConstellationNode>>();
                constellationStack.Push(firstTuple);

                while(constellationStack.Count > 0)
                {
                    Tuple<ConstellationLink, ConstellationNode> currentConstellationTuple = constellationStack.Peek();

                    Stack<StarEntity> potentialStarEntities = new Stack<StarEntity>();

                    bool alreadyExploredNode = nodeToStarEntity.ContainsKey(currentConstellationTuple.Item2);

                    if (linkToPotentialStarEntities.ContainsKey(currentConstellationTuple) == false)
                    {
                        if (currentConstellationTuple.Item2 != this.NodeSelf)
                        {
                            potentialStarEntities = currentConstellationTuple.Item1.GetPotentialLinkedStars(boardGameLayer, pathStarEntities.Peek());

                            if (alreadyExploredNode)
                            {
                                potentialStarEntities = new Stack<StarEntity>();
                                StarEntity starEntityAlreadyExplored = nodeToStarEntity[currentConstellationTuple.Item2];
                                if (potentialStarEntities.Contains(starEntityAlreadyExplored))
                                {
                                    potentialStarEntities.Push(starEntityAlreadyExplored);
                                }
                            }
                            else
                            {
                                potentialStarEntities = new Stack<StarEntity>(potentialStarEntities.Where(pElem => currentConstellationTuple.Item2.IsStarValid(pElem)));
                            }
                        }
                        else
                        {
                            potentialStarEntities.Push(startStarEntity);
                        }
                    }
                    else
                    {
                        potentialStarEntities = linkToPotentialStarEntities[currentConstellationTuple];
                    }

                    if(potentialStarEntities.Count > 0)
                    {
                        if (alreadyExploredNode == false)
                        {
                            StarEntity starEntity = potentialStarEntities.Pop();

                            nodeToStarEntity.Add(currentConstellationTuple.Item2, starEntity);

                            pathStarEntities.Push(starEntity);

                            linkToPotentialStarEntities.Add(currentConstellationTuple, potentialStarEntities);

                            this.StackConstellationElementFrom(currentConstellationTuple.Item1, constellationStack, currentConstellationTuple.Item2);
                        }
                    }
                    else
                    {
                        constellationStack.Pop();
                        bool reachAnotherTuple;
                        do
                        {
                            reachAnotherTuple = false;

                            if (constellationStack.Count > 0)
                            {
                                Tuple<ConstellationLink, ConstellationNode> constellationTuple = constellationStack.Peek();

                                if (linkToPotentialStarEntities.ContainsKey(constellationTuple))
                                {
                                    reachAnotherTuple = linkToPotentialStarEntities[constellationTuple].Count > 0;

                                    if (reachAnotherTuple == false)
                                    {
                                        StarEntity starEntityToPop = pathStarEntities.Pop();

                                        nodeToStarEntity.Remove(constellationTuple.Item2);
                                    }
                                }

                                if (reachAnotherTuple == false)
                                {
                                    constellationStack.Pop();
                                }
                            }
                            else
                            {
                                reachAnotherTuple = true;
                            }

                        } while (reachAnotherTuple == false);

                        if(constellationStack.Count == 0)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public void AddNode(ConstellationNode nodeToAdd)
        {
            this.ConstellationNodeSystem.Add(nodeToAdd);

            this.constellationNodeToLinks.Add(nodeToAdd, new HashSet<ConstellationLink>());

            if(nodeToAdd is ConstellationNodeSelf)
            {
                this.NodeSelf = nodeToAdd as ConstellationNodeSelf;
            }
        }

        public void RemoveNode(ConstellationNode nodeToRemove)
        {
            this.ConstellationNodeSystem.Remove(nodeToRemove);

            HashSet<ConstellationLink> linksList = this.constellationNodeToLinks[nodeToRemove];
            this.ConstellationNodeSystem.Remove(nodeToRemove);

            foreach (ConstellationLink link in linksList)
            {
                this.RemoveNodeLink(link);
            }
        }

        public void AddNodeLink(ConstellationLink linkToAdd)
        {
            this.ConstellationLinkSystem.Add(linkToAdd);

            this.constellationNodeToLinks[linkToAdd.Node1].Add(linkToAdd);
            this.constellationNodeToLinks[linkToAdd.Node2].Add(linkToAdd);
        }

        public void RemoveNodeLink(ConstellationLink linkToRemove)
        {
            this.ConstellationLinkSystem.Remove(linkToRemove);

            if (this.constellationNodeToLinks.ContainsKey(linkToRemove.Node1))
            {
                this.constellationNodeToLinks[linkToRemove.Node1].Remove(linkToRemove);
            }

            if (this.constellationNodeToLinks.ContainsKey(linkToRemove.Node2))
            {
                this.constellationNodeToLinks[linkToRemove.Node2].Remove(linkToRemove);
            }
        }

        private void StackConstellationElementFrom(ConstellationLink fromLink, Stack<Tuple<ConstellationLink, ConstellationNode>> constellationStack, ConstellationNode node)
        {
            HashSet<ConstellationLink> connectedLinks = this.constellationNodeToLinks[node];

            foreach(ConstellationLink link in connectedLinks)
            {
                if (link != fromLink)
                {
                    ConstellationNode otherNode;
                    if (link.Node1 == node)
                    {
                        otherNode = link.Node2;
                    }
                    else
                    {
                        otherNode = link.Node1;
                    }

                    constellationStack.Push(new Tuple<ConstellationLink, ConstellationNode>(link, otherNode));
                }
            }
        }

        public void UpdateConstellationOrder()
        {
            if (this.NodeSelf != null)
            {

            }
        }
    }
}
