using Metempsychoid.Model.Card;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.EntityLayer;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardNotifLayer
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
