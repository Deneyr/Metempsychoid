using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model;
using Metempsychoid.View.Controls;
using SFML.Graphics;
using SFML.System;

namespace Metempsychoid.View
{
    public abstract class ALayer2D: IObject2D
    {
        protected SFML.Graphics.View view;

        protected WeakReference<World2D> world2D;
        protected ALayer parentLayer;

        protected Dictionary<IObject, IObject2D> objectToObject2Ds;

        protected float zoom;

        public Vector2f Position
        {
            get
            {
                return this.view.Center;
            }

            set
            {
                this.view.Center = value * MainWindow.MODEL_TO_VIEW;
            }
        }

        public float Rotation
        {
            get
            {
                return this.view.Rotation;
            }

            set
            {
                this.view.Rotation = value;
            }
        }

        public FloatRect Viewport
        {
            get
            {
                return this.view.Viewport;
            }

            protected set
            {
                this.view.Viewport = value;
            }
        }

        public FloatRect Bounds
        {
            get
            {
                return new FloatRect(this.view.Center.X - this.view.Size.X / 2, this.view.Center.Y - this.view.Size.Y / 2, this.view.Size.X, this.view.Size.Y);
            }
        }

        public ALayer2D(World2D world2D, ALayer layer)
        {
            this.view = new SFML.Graphics.View();
            this.Position = new Vector2f(0, 0);

            this.objectToObject2Ds = new Dictionary<IObject, IObject2D>();

            this.world2D = new WeakReference<World2D>(world2D);

            this.zoom = 0;

            this.parentLayer = layer;
            this.parentLayer.ObjectAdded += OnObjectAdded;
            this.parentLayer.ObjectRemoved += OnObjectRemoved;
            this.parentLayer.ObjectPropertyChanged += OnObjectPropertyChanged;
        }

        protected virtual void OnObjectRemoved(IObject obj)
        {
            this.objectToObject2Ds.Remove(obj);
        }

        protected virtual void OnObjectAdded(IObject obj)
        {
            if(this.world2D.TryGetTarget(out World2D world2D))
            {
                IObject2D object2D = World2D.MappingObjectModelView[obj.GetType()].CreateObject2D(world2D, obj);

                this.objectToObject2Ds.Add(obj, object2D);
            }
        }

        protected virtual void OnObjectPropertyChanged(IObject obj, string propertyName)
        {
            switch (propertyName)
            {
                case "Position":
                    this.objectToObject2Ds[obj].Position = obj.Position;
                    break;
            }
        }

        protected virtual void UpdateViewSize(Vector2f viewSize, Time deltaTime)
        {
            this.view.Size = viewSize;
        }

        public virtual bool OnControlActivated(ControlEventType eventType, string details)
        {
            // To override
            return true;
        }

        public virtual void DrawIn(RenderWindow window, Time deltaTime)
        {
            SFML.Graphics.View defaultView = window.DefaultView;

            this.UpdateViewSize(defaultView.Size, deltaTime);

            window.SetView(this.view);

            FloatRect bounds = this.Bounds;
            foreach (IObject2D object2D in this.objectToObject2Ds.Values)
            {
                if (object2D.Bounds.Intersects(bounds))
                {
                    object2D.DrawIn(window, deltaTime);
                }
            }

            window.SetView(defaultView);
        }

        public void SetCanevas(IntRect newCanevas)
        {
            // Nothing to do
        }

        public void SetZoom(float newZoom)
        {
            this.zoom = newZoom;
        }

        public void Dispose()
        {
            foreach (IObject2D object2D in this.objectToObject2Ds.Values)
            {
                object2D.Dispose();
            }

            this.parentLayer.ObjectAdded -= OnObjectAdded;
            this.parentLayer.ObjectRemoved -= OnObjectRemoved;
            this.parentLayer.ObjectPropertyChanged -= OnObjectPropertyChanged;
            this.parentLayer = null;
        }
    }
}
