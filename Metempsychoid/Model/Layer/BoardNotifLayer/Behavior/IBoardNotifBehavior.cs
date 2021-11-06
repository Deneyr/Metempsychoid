using Astrategia.Model.Card;
using Astrategia.Model.Event;
using Astrategia.Model.Node.TestWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Layer.BoardNotifLayer.Behavior
{
    public interface IBoardNotifBehavior
    {
        CardEntity OwnerCardEntity
        {
            get;
        }

        CardBoardLevel NodeLevel
        {
            get;
            set;
        }

        bool IsThereEndButton
        {
            get;
        }

        bool IsThereBehaviorLabel
        {
            get;
        }

        void StartNotif(World world);

        bool UpdateNotif(World world);

        void EndNotif(World world);

        void HandleGameEvents(Dictionary<EventType, List<GameEvent>> gameEvents);
    }
}
