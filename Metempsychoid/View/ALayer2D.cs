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
        private Vector2f defaultViewSize;

        protected WeakReference<World2D> world2D;
        protected ALayer parentLayer;

        protected Dictionary<IObject, IObject2D> objectToObject2Ds;

        protected float zoom;

        public Vector2i Area
        {
            get;
            protected set;
        }

        public Vector2f Position
        {
            get
            {
                return this.view.Center;
            }

            set
            {
                if (value != this.Position)
                {
                    this.ClampPosition(value);
                }
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

        public float Zoom
        {
            get
            {
                return this.zoom;
            }

            set
            {
                if (value != this.zoom)
                {
                    this.ClampZoom(value);

                    this.ClampPosition(this.Position);
                }
            }
        }

        public IntRect Canevas
        {
            get
            {
                return new IntRect(-this.Area.X / 2, -this.Area.Y / 2, this.Area.X / 2, this.Area.Y / 2);
            }
            set
            {
                // Nothing to do
            }
        }

        protected Vector2f DefaultViewSize
        {
            get
            {
                return this.defaultViewSize;
            }

            set
            {
                if(value != this.defaultViewSize)
                {
                    this.defaultViewSize = value;

                    this.ClampZoom(this.Zoom);

                    this.ClampPosition(this.Position);
                }
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
            this.defaultViewSize = this.view.Size;
            this.Position = new Vector2f(0, 0);
            this.Area = new Vector2i(0, 0);

            this.objectToObject2Ds = new Dictionary<IObject, IObject2D>();

            this.world2D = new WeakReference<World2D>(world2D);

            this.zoom = 1;

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

        protected void ClampPosition(Vector2f newPosition)
        {
            float minXPosition = Math.Min(0, -this.Area.X / 2f + this.defaultViewSize.X * this.zoom / 2);
            float minYPosition = Math.Min(0, -this.Area.Y / 2f + this.defaultViewSize.Y * this.zoom / 2);

            float maxXPosition = Math.Max(0, this.Area.X / 2f - this.defaultViewSize.X * this.zoom / 2);
            float maxYPosition = Math.Max(0, this.Area.Y / 2f - this.defaultViewSize.Y * this.zoom / 2);

            float newXPosition = Math.Min(Math.Max(newPosition.X, minXPosition), maxXPosition);
            float newYPosition = Math.Min(Math.Max(newPosition.Y, minYPosition), maxYPosition);
            Vector2f newClampedPosition = new Vector2f(newXPosition, newYPosition);

            if(newClampedPosition != this.view.Center)
            {
                this.view.Center = newClampedPosition;
            }
        }

        protected void ClampZoom(float newZoom)
        {
            float zoomXMax = ((float)this.Area.X) / this.defaultViewSize.X;
            float zoomYMax = ((float)this.Area.Y) / this.defaultViewSize.Y;

            float newZoomClamped = Math.Min(Math.Min(newZoom, zoomXMax), zoomYMax);

            if(newZoomClamped != this.zoom)
            {
                this.zoom = newZoomClamped;
            }
        }

        protected virtual void UpdateViewSize(Vector2f viewSize, Time deltaTime)
        {
            this.DefaultViewSize = viewSize;
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

            this.view.Zoom(this.zoom);

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
