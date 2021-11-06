using Astrategia.Model;
using Astrategia.Model.Event;
using Astrategia.View.Animation;
using Astrategia.View.Controls;
using Astrategia.View.SoundsManager;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.AI
{
    public abstract class AAIEntity: AAIObject
    {
        protected WeakReference<AAILayer> parentLayer;

        public virtual bool IsActive
        {
            get;
            set;
        }

        public AAIEntity(AAILayer parentLayer, IAIObjectFactory factory, bool isActive)
            : base(factory)
        {
            this.parentLayer = new WeakReference<AAILayer>(parentLayer);

            this.IsActive = isActive;
        }

        public AAIEntity(AAILayer parentLayer, IAIObjectFactory factory, AEntity entity)
            : base(factory)
        {
            this.parentLayer = new WeakReference<AAILayer>(parentLayer);

            this.IsActive = entity.IsActive;
        }
    }
}
