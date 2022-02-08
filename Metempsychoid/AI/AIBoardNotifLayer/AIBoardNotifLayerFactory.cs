using Astrategia.Model;
using Astrategia.Model.Layer.BoardNotifLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.AI.AIBoardNotifLayer
{
    public class AIBoardNotifLayerFactory: AAIObjectFactory
    {
        public override IAIObject CreateObjectAI(AIWorld worldAI, IObject obj)
        {
            if (obj is BoardNotifLayer)
            {
                BoardNotifLayer boardPlayerLayer = obj as BoardNotifLayer;

                return new AIBoardNotifLayer(worldAI, this, boardPlayerLayer);
            }

            return null;
        }
    }
}
