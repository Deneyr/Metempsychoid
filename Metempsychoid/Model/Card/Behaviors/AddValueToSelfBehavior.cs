using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.Model.Layer.BoardGameLayer;
using Astrategia.Model.Layer.BoardGameLayer.Actions;

namespace Astrategia.Model.Card.Behaviors
{
    public class AddValueToSelfBehavior : ACardBehavior
    {
        public int Value
        {
            get;
            private set;
        }

        public AddValueToSelfBehavior(int value)
        {
            this.Value = value;
        }

        public override void OnAwakened(BoardGameLayer layer, StarEntity starEntity)
        {
            layer.PendingActions.Add(new AddCardValueModifier(starEntity.CardSocketed, this, this.Value));
        }

        public override void OnUnawakened(BoardGameLayer layer, CardEntity ownerCardEntity)
        {
            layer.PendingActions.Add(new ClearCardValueModifier(ownerCardEntity, this));
        }

        public override ICardBehavior Clone()
        {
            return new AddValueToSelfBehavior(this.Value);
        }
    }
}
