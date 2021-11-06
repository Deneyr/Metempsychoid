using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Layer.BoardGameLayer
{
    public interface IBoardToLayerPositionConverter
    {
        Vector2f BoardToLayerPosition(BoardGameLayer layer, Vector2f boardPosition);

        Vector2f LayerToBoardPosition(BoardGameLayer layer, Vector2f layerPosition);
    }
}
