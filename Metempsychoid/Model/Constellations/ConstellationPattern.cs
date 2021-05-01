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

                Dictionary<ConstellationNode, int> alreadyEncounteredNodes = new Dictionary<ConstellationNode, int>();
                Dictionary<StarEntity, int> alreadyEncounteredStars = new Dictionary<StarEntity, int>();
                int counter = 0;

                Stack<StarEntity> pathStarEntities = new Stack<StarEntity>();

                Stack<Tuple<ConstellationLink, ConstellationNode>> constellationStack = new Stack<Tuple<ConstellationLink, ConstellationNode>>();
                Stack<Tuple<ConstellationLink, ConstellationNode>> constellationHandledStack = new Stack<Tuple<ConstellationLink, ConstellationNode>>();
                constellationStack.Push(firstTuple);

                while (constellationStack.Count > 0)
                {
                    Tuple<ConstellationLink, ConstellationNode> currentConstellationTuple = constellationStack.Peek();
                    if (constellationHandledStack.Count > 0 && currentConstellationTuple == constellationHandledStack.Peek())
                    {
                        constellationStack.Pop();
                        constellationHandledStack.Pop();
                        StarEntity starEntity = pathStarEntities.Pop();

                        //alreadyEncounteredStars.Remove(starEntity);
                        //alreadyEncounteredNodes.Remove(currentConstellationTuple.Item2);

                        linkToPotentialStarEntities.Remove(currentConstellationTuple);
                    }
                    else
                    {
                        constellationHandledStack.Push(currentConstellationTuple);

                        Stack<StarEntity> potentialStarEntities = new Stack<StarEntity>();

                        bool alreadyExploredNode = alreadyEncounteredNodes.ContainsKey(currentConstellationTuple.Item2);

                        if (linkToPotentialStarEntities.ContainsKey(currentConstellationTuple) == false)
                        {
                            if (currentConstellationTuple.Item1 != null)
                            {
                                potentialStarEntities = currentConstellationTuple.Item1.GetPotentialLinkedStars(boardGameLayer, pathStarEntities.Peek());

                                if (alreadyExploredNode)
                                {
                                    StarEntity starEntityAlreadyExplored = nodeToStarEntity[currentConstellationTuple.Item2];
                                    bool potentialStarContainsAlreadyExplored = potentialStarEntities.Contains(starEntityAlreadyExplored);
                                    potentialStarEntities = new Stack<StarEntity>();
                                    if (potentialStarContainsAlreadyExplored)
                                    {
                                        potentialStarEntities.Push(starEntityAlreadyExplored);
                                    }
                                }
                                else
                                {
                                    potentialStarEntities = new Stack<StarEntity>(potentialStarEntities.Where(pElem => currentConstellationTuple.Item2.IsStarValid(pElem) && alreadyEncounteredStars.ContainsKey(pElem) == false));
                                }
                            }
                            else
                            {
                                potentialStarEntities.Push(startStarEntity);
                            }

                            linkToPotentialStarEntities.Add(currentConstellationTuple, potentialStarEntities);
                        }
                        else
                        {
                            potentialStarEntities = linkToPotentialStarEntities[currentConstellationTuple];
                        }

                        if (potentialStarEntities.Count > 0)
                        {
                            StarEntity starEntity = potentialStarEntities.Pop();
                            pathStarEntities.Push(starEntity);

                            if (alreadyExploredNode == false)
                            {
                                nodeToStarEntity.Add(currentConstellationTuple.Item2, starEntity);

                                alreadyEncounteredStars.Add(starEntity, counter);
                                alreadyEncounteredNodes.Add(currentConstellationTuple.Item2, counter);
                                counter++;

                                this.StackConstellationElementFrom(currentConstellationTuple.Item1, constellationStack, currentConstellationTuple.Item2);
                            }
                        }
                        else
                        {
                            Tuple<ConstellationLink, ConstellationNode> constellationTupleToRemove = constellationStack.Pop();
                            constellationHandledStack.Pop();
                            linkToPotentialStarEntities.Remove(constellationTupleToRemove);
                            bool reachAnotherTuple;
                            do
                            {
                                reachAnotherTuple = false;

                                if (constellationStack.Count > 0)
                                {
                                    Tuple<ConstellationLink, ConstellationNode> constellationTuple = constellationStack.Peek();

                                    if (constellationTuple == constellationHandledStack.Peek())
                                    {
                                        constellationHandledStack.Pop();

                                        reachAnotherTuple = linkToPotentialStarEntities[constellationTuple].Count > 0;

                                        StarEntity starEntity = pathStarEntities.Pop();

                                        nodeToStarEntity.Remove(constellationTuple.Item2);

                                        //alreadyEncounteredStars.Remove(starEntity);
                                        //alreadyEncounteredNodes.Remove(constellationTuple.Item2);

                                        //linkToPotentialStarEntities.Remove(constellationTuple);

                                        if (reachAnotherTuple)
                                        {
                                            int counterToReach = alreadyEncounteredNodes[constellationTuple.Item2];

                                            alreadyEncounteredStars = alreadyEncounteredStars.Where(pElem => pElem.Value < counterToReach).ToDictionary(x => x.Key, x => x.Value);
                                            alreadyEncounteredNodes = alreadyEncounteredNodes.Where(pElem => pElem.Value < counterToReach).ToDictionary(x => x.Key, x => x.Value);
                                        }
                                        else
                                        {
                                            linkToPotentialStarEntities.Remove(constellationTuple);
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

                            if (constellationStack.Count == 0)
                            {
                                return false;
                            }
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
