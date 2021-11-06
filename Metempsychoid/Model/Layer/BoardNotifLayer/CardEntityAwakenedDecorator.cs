using Astrategia.Model.Card;
using Astrategia.Model.Layer.BoardGameLayer;
using Astrategia.Model.Layer.EntityLayer;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Layer.BoardNotifLayer
{
    public class CardEntityAwakenedDecorator : CardEntityDecorator
    {
        private int valueBeforeAwakened;


        public override int CardValue
        {
            get
            {
                return this.valueBeforeAwakened;
            }
        }

        public CardEntityAwakenedDecorator(EntityLayer.EntityLayer entityLayer, CardEntity cardEntity, int valueBeforeAwakened) 
            : base(entityLayer, cardEntity)
        {
            this.valueBeforeAwakened = valueBeforeAwakened;
        }
    }
}
