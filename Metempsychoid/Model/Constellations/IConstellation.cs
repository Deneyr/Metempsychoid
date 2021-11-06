using Astrategia.Model.Layer.BoardGameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Constellations
{
    public interface IConstellation: IDisposable
    {
        Dictionary<ConstellationNode, StarEntity> NodeToStarEntity
        {
            get;
        }

        Dictionary<ConstellationLink, List<StarLinkEntity>> LinkToStarLinkEntity
        {
            get;
        }

        bool IsAwakened
        {
            get;
        }

        void OnOtherStarEntitiesChanged(BoardGameLayer boardGameLayer, StarEntity starEntity, HashSet<StarEntity> starEntitiesChanged);

        void ResetConstellation();

        IConstellation Clone(Card.Card parentCard);
    }
}
