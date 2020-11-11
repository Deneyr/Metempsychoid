using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Metempsychoid.Model
{
    public abstract class ALayer : IObject
    {
        public event Action<IObject> ObjectAdded;
        public event Action<IObject> ObjectRemoved;

        public event Action<IObject, string> ObjectPropertyChanged;

        public Vector2f Position
        {
            get;
            protected set;
        }

        public HashSet<Type> TypesInChunk
        {
            get;
            protected set;
        }

        public ALayer()
        {
            this.TypesInChunk = new HashSet<Type>();
        }

        protected void NotifyObjectAdded(IObject obj)
        {
            this.ObjectAdded?.Invoke(obj);
        }

        protected void NotifyObjectRemoved(IObject obj)
        {
            this.ObjectRemoved?.Invoke(obj);
        }

        protected void NotifyObjectPropertyChanged(IObject obj, string propertyName)
        {
            this.ObjectPropertyChanged?.Invoke(obj, propertyName);
        }
    }
}
