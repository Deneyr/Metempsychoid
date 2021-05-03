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

        private Stack<Tuple<ConstellationLink, ConstellationNode>> constellationStack;
        private Stack<Snapshot> snapshotStack;

        private HashSet<ConstellationNode> alreadyEncounteredNodes;
        private HashSet<StarEntity> alreadyEncounteredStarEntities;

        private Stack<StarEntity> pathStarEntities;

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

            this.constellationStack = new Stack<Tuple<ConstellationLink, ConstellationNode>>();
            this.snapshotStack = new Stack<Snapshot>();

            this.alreadyEncounteredNodes = new HashSet<ConstellationNode>();
            this.alreadyEncounteredStarEntities = new HashSet<StarEntity>();

            this.pathStarEntities = new Stack<StarEntity>();
        }

        public bool CreateConstellationSystem(
            BoardGameLayer boardGameLayer, 
            StarEntity startStarEntity, 
            Dictionary<ConstellationNode, StarEntity> nodeToStarEntity, 
            Dictionary<ConstellationLink, StarLinkEntity> linkToStarLinkEntity)
        {
            nodeToStarEntity.Clear();
            linkToStarLinkEntity.Clear();

            if (this.NodeSelf != null)
            {
                this.Initialize(boardGameLayer, startStarEntity);

                while (this.constellationStack.Any())
                {
                    Tuple<ConstellationLink, ConstellationNode> currentTuple = this.constellationStack.Peek();

                    bool isRewind = this.snapshotStack.Count > 0 && currentTuple == this.snapshotStack.Peek().CurrentTopStack;
                    bool alreadyExploredNode = false;

                    Stack<StarEntity> potentialStarEntities = null;
                    if (isRewind)
                    {
                        potentialStarEntities = this.snapshotStack.Peek().PotentialStarEntities;
                    }
                    else
                    {
                        potentialStarEntities = this.CreatePotentialStarEntitiesFrom(boardGameLayer, nodeToStarEntity, out alreadyExploredNode);
                    }

                    if (potentialStarEntities.Count > 0)
                    {
                        StarEntity starEntity = potentialStarEntities.Pop();

                        if (alreadyExploredNode == false)
                        {
                            if (isRewind == false && potentialStarEntities.Any())
                            {
                                this.snapshotStack.Push(new Snapshot(this, potentialStarEntities));
                            }

                            //if (nodeToStarEntity.ContainsKey(currentTuple.Item2))
                            //{
                            //    nodeToStarEntity[currentTuple.Item2] = starEntity;
                            //}
                            //else
                            //{
                            //    nodeToStarEntity.Add(currentTuple.Item2, starEntity);
                            //}

                            this.UpdateMapping(boardGameLayer, nodeToStarEntity, linkToStarLinkEntity, currentTuple, starEntity);

                            this.alreadyEncounteredStarEntities.Add(starEntity);
                            this.alreadyEncounteredNodes.Add(currentTuple.Item2);

                            //this.pathStarEntities.Push(starEntity);

                            this.constellationStack.Pop();
                            this.pathStarEntities.Pop();
                            this.StackConstellationElementFrom(currentTuple.Item1, currentTuple.Item2, starEntity);
                        }
                        else
                        {
                            this.constellationStack.Pop();
                            this.pathStarEntities.Pop();
                        }
                    }
                    else
                    {
                        if (this.RewindConstellationStack())
                        {
                            this.RestoreCurrentSnapshot();
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private void Initialize(BoardGameLayer boardGameLayer, StarEntity startStarEntity)
        {
            this.snapshotStack.Clear();
            this.constellationStack.Clear();

            this.alreadyEncounteredNodes.Clear();
            this.alreadyEncounteredStarEntities.Clear();

            this.pathStarEntities.Clear();

            this.constellationStack.Push(new Tuple<ConstellationLink, ConstellationNode>(null, this.NodeSelf));
            this.pathStarEntities.Push(startStarEntity);

            //Stack<StarEntity> potentialStarEntity = this.CreatePotentialStarEntitiesFrom(boardGameLayer, null, out bool alreadyExploredNode);
        }

        private void UpdateMapping(BoardGameLayer boardGameLayer,
            Dictionary<ConstellationNode, StarEntity> nodeToStarEntity, 
            Dictionary<ConstellationLink, StarLinkEntity> linkToStarLinkEntity,
            Tuple<ConstellationLink, ConstellationNode> currentTuple, 
            StarEntity starEntity)
        {
            if (nodeToStarEntity.ContainsKey(currentTuple.Item2))
            {
                nodeToStarEntity[currentTuple.Item2] = starEntity;
            }
            else
            {
                nodeToStarEntity.Add(currentTuple.Item2, starEntity);
            }

            if(currentTuple.Item1 != null)
            {
                StarEntity fromStar = this.pathStarEntities.Peek();

                foreach(StarLinkEntity link in boardGameLayer.StarToLinks[fromStar])
                {
                    if(link.StarFrom == starEntity
                        || link.StarTo == starEntity)
                    {
                        if (linkToStarLinkEntity.ContainsKey(currentTuple.Item1))
                        {
                            linkToStarLinkEntity[currentTuple.Item1] = link;
                        }
                        else
                        {
                            linkToStarLinkEntity.Add(currentTuple.Item1, link);
                        }
                    }
                }
            }
        }

        private bool RewindConstellationStack()
        {
            while (this.snapshotStack.Count > 0 && this.snapshotStack.Peek().PotentialStarEntities.Count == 0)
            {
                this.snapshotStack.Pop();
            }

            if(this.snapshotStack.Count > 0)
            {
                return true;
            }
            return false;
        }

        private void RestoreCurrentSnapshot()
        {
            Snapshot currentSnapshot = this.snapshotStack.Peek();

            this.constellationStack = currentSnapshot.CurrentStack;

            this.alreadyEncounteredNodes = currentSnapshot.CurrentEncouteredNodes;
            this.alreadyEncounteredStarEntities = currentSnapshot.CurrentEncouteredStarEntities;

            this.pathStarEntities = currentSnapshot.CurrentPathStarEntities;
        }

        private Stack<StarEntity> CreatePotentialStarEntitiesFrom(BoardGameLayer boardGameLayer, Dictionary<ConstellationNode, StarEntity> nodeToStarEntity, out bool alreadyExploredNode)
        {
            Stack<StarEntity> potentialStarEntities = new Stack<StarEntity>();

            StarEntity currentStarEntity = this.pathStarEntities.Peek();

            Tuple<ConstellationLink, ConstellationNode> currentConstellationTuple = constellationStack.Peek();

            alreadyExploredNode = this.alreadyEncounteredNodes.Contains(currentConstellationTuple.Item2);

            if (currentConstellationTuple.Item1 != null)
            {
                potentialStarEntities = currentConstellationTuple.Item1.GetPotentialLinkedStars(boardGameLayer, currentStarEntity);

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
                    potentialStarEntities = new Stack<StarEntity>(potentialStarEntities.Where(pElem => currentConstellationTuple.Item2.IsStarValid(pElem) && this.alreadyEncounteredStarEntities.Contains(pElem) == false));
                }
            }
            else
            {
                potentialStarEntities.Push(currentStarEntity);
            }

            return potentialStarEntities;
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

        private void StackConstellationElementFrom(ConstellationLink fromLink, ConstellationNode node, StarEntity starEntity)
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

                    this.pathStarEntities.Push(starEntity);
                    this.constellationStack.Push(new Tuple<ConstellationLink, ConstellationNode>(link, otherNode));
                }
            }
        }

        public void UpdateConstellationOrder()
        {
            if (this.NodeSelf != null)
            {

            }
        }

        private class Snapshot
        {
            private Stack<Tuple<ConstellationLink, ConstellationNode>> currentStack;

            private HashSet<ConstellationNode> alreadyEncounteredNodes;
            private HashSet<StarEntity> alreadyEncounteredStarEntities;

            private Stack<StarEntity> currentPathStarEntities;

            private Tuple<ConstellationLink, ConstellationNode> currentTopStack;

            public HashSet<ConstellationNode> CurrentEncouteredNodes
            {
                get
                {
                    return new HashSet<ConstellationNode>(this.alreadyEncounteredNodes);
                }
            }

            public HashSet<StarEntity> CurrentEncouteredStarEntities
            {
                get
                {
                    return new HashSet<StarEntity>(this.alreadyEncounteredStarEntities);
                }
            }

            public Stack<Tuple<ConstellationLink, ConstellationNode>> CurrentStack
            {
                get
                {
                    return new Stack<Tuple<ConstellationLink, ConstellationNode>>(this.currentStack);
                }
            }

            public Tuple<ConstellationLink, ConstellationNode> CurrentTopStack
            {
                get
                {
                    return this.currentTopStack;
                }
            }

            public Stack<StarEntity> CurrentPathStarEntities
            {
                get
                {
                    return new Stack<StarEntity>(this.currentPathStarEntities);
                }
            }

            public Stack<StarEntity> PotentialStarEntities
            {
                get;
                private set;
            }

            public Snapshot(ConstellationPattern parentConstellationPattern, Stack<StarEntity> potentialStarEntities)
            {
                this.currentStack = new Stack<Tuple<ConstellationLink, ConstellationNode>>(parentConstellationPattern.constellationStack);
                this.currentTopStack = parentConstellationPattern.constellationStack.Peek();

                this.alreadyEncounteredNodes = new HashSet<ConstellationNode>(parentConstellationPattern.alreadyEncounteredNodes);
                this.alreadyEncounteredStarEntities = new HashSet<StarEntity>(parentConstellationPattern.alreadyEncounteredStarEntities);

                this.currentPathStarEntities = new Stack<StarEntity>(parentConstellationPattern.pathStarEntities);

                this.PotentialStarEntities = potentialStarEntities;
            }
        }
    }
}
