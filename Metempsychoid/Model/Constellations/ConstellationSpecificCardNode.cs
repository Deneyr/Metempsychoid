using Metempsychoid.Model.Layer.BoardGameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Constellations
{
    public class ConstellationSpecificCardNode : ConstellationNode
    {
        public NodeType Type
        {
            get;
            private set;
        }

        public string CardName
        {
            get;
            private set;
        }

        public ConstellationSpecificCardNode(string cardName, NodeType type)
        {
            this.Type = type;

            this.CardName = cardName;
        }

        public override bool IsStarValid(StarEntity star, StarEntity startStarEntity)
        {
            bool isValid = true;
            switch (this.Type)
            {
                case NodeType.ALLY:
                    isValid = startStarEntity.CardSocketed.Card.CurrentOwner == star.CardSocketed.Card.CurrentOwner;
                    break;
                case NodeType.OPPONENT:
                    isValid = startStarEntity.CardSocketed.Card.CurrentOwner != star.CardSocketed.Card.CurrentOwner;
                    break;
            }

            return isValid && base.IsStarValid(star, startStarEntity) && startStarEntity.CardSocketed.Card.Name == this.CardName;
        }

        public enum NodeType
        {
            DEFAULT,
            ALLY,
            OPPONENT
        }
    }
}

