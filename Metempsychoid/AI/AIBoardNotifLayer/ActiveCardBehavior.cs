using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.AI.AIBoardNotifLayer
{
    public class ActiveCardBehavior
    {
        public Type BehaviorType
        {
            get;
            private set;
        }

        public int NbUses
        {
            get;
            set;
        }

        public string CurrentPhase
        {
            get;
            set;
        }

        public string OwnerCardName
        {
            get;
            private set;
        }

        public ActiveCardBehavior(Type behaviorType, int nbUses, string phase, string ownerCardName)
        {
            this.BehaviorType = behaviorType;
            this.NbUses = nbUses;
            this.OwnerCardName = ownerCardName;
            this.CurrentPhase = phase;
        }

    }
}
