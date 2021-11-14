using Astrategia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.AI
{
    public struct GameEventContainer
    {
        public Model.Event.EventType Type
        {
            get;
            private set;
        }

        public AEntity Entity
        {
            get;
            private set;
        }

        public string Details
        {
            get;
            private set;
        }

        public GameEventContainer(Model.Event.EventType type, AEntity entity, string details)
        {
            this.Type = type;

            this.Entity = entity;

            this.Details = details;
        }
    }
}
