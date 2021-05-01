using Metempsychoid.Model.Layer.BoardGameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Constellations
{
    public class ConstellationLink
    {
        public string Name
        {
            get;
            set;
        }

        public ConstellationNode Node1
        {
            get;
            private set;
        }

        public ConstellationNode Node2
        {
            get;
            private set;
        }

        public ConstellationLink(ConstellationNode node1, ConstellationNode node2)
        {
            this.Node1 = node1;
            this.Node2 = node2;
        }

        public Stack<StarEntity> GetPotentialLinkedStars(BoardGameLayer boardGameLayer, StarEntity fromStar)
        {
            Stack<StarEntity> result = new Stack<StarEntity>();

            HashSet<StarLinkEntity> linksConnectedToStar = boardGameLayer.StarToLinks[fromStar];

            foreach(StarLinkEntity starLinks in linksConnectedToStar)
            {
                if (this.IsStarLinkValid(starLinks))
                {
                    StarEntity starEntity;
                    if(starLinks.StarFrom == fromStar)
                    {
                        starEntity = starLinks.StarTo;
                    }
                    else
                    {
                        starEntity = starLinks.StarFrom;
                    }

                    result.Push(starEntity);
                }
            }

            return result;
        }

        protected virtual bool IsStarLinkValid(StarLinkEntity link)
        {
            return true;
        }

    }
}
