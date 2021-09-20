using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardGameLayer.Actions
{
    public class SecureDomainAction : IBoardGameAction
    {
        public CJStarDomain DomainToSecure
        {
            get;
            private set;
        }

        public Player.Player DomainOwner
        {
            get;
            private set;
        }

        public SecureDomainAction(CJStarDomain domainToSecure, Player.Player domainOwner)
        {
            this.DomainToSecure = domainToSecure;
            this.DomainOwner = domainOwner;
        }

        public void ExecuteAction(BoardGameLayer layerToPerform)
        {
            this.DomainToSecure.TemporaryDomainOwner = this.DomainOwner;
        }

        public bool IsStillValid(BoardGameLayer layerToPerform)
        {
            return true;
        }
    }
}
