using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.Model;
using Astrategia.Model.Layer.BoardGameLayer;

namespace Astrategia.AI.AIBoardGameLayer
{
    public class AIBoardGameLayerFactory : AAIObjectFactory
    {
        public override IAIObject CreateObjectAI(AIWorld worldAI, IObject obj)
        {
            if (obj is BoardGameLayer)
            {
                BoardGameLayer boardGameLayer = obj as BoardGameLayer;

                return new AIBoardGameLayer(worldAI, this, boardGameLayer);
            }

            return null;
        }
    }
}
