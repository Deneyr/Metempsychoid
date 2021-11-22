using Astrategia.Model;
using Astrategia.Model.Layer.BoardPlayerLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.AI.AIBoardPlayerLayer
{
    public class AIBoardPlayerLayerFactory : AAIObjectFactory
    {
        public override IAIObject CreateObjectAI(AIWorld worldAI, IObject obj)
        {
            if (obj is BoardPlayerLayer)
            {
                BoardPlayerLayer boardPlayerLayer = obj as BoardPlayerLayer;

                return new AIBoardPlayerLayer(worldAI, this, boardPlayerLayer);
            }

            return null;
        }
    }
}