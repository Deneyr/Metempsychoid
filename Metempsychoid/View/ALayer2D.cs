using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model;
using Metempsychoid.View.Controls;
using Metempsychoid.View.Layer2D.BackgroundLayer2D;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Metempsychoid.View
{
    public abstract class ALayer2D : AObject2D
    {
        protected SFML.Graphics.View view;
        private Vector2f defaultViewSize;

        protected WeakReference<World2D> world2D;
        protected ALayer parentLayer;

        protected Dictionary<AEntity, AEntity2D> objectToObject2Ds;

        protected float zoom;

        public List<ALayer2D> ChildrenLayer2D
        {
            get;
            protected set;
        }

        public Vector2i Area
        {
            get;
            protected set;
        }

        public override Vector2f Position
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

                    foreach (ALayer2D child in this.ChildrenLayer2D)
                    {
                        child.Position = this.Position;
                    }
                }
            }
        }

        public override float Rotation
        {
            get
            {
                return this.view.Rotation;
            }

            set
            {
                if (value != this.Rotation)
                {
                    this.view.Rotation = value;

                    foreach (ALayer2D child in this.ChildrenLayer2D)
                    {
                        child.Rotation = this.Rotation;
                    }
                }
            }
        }

        public override float Zoom
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

                    foreach (ALayer2D child in this.ChildrenLayer2D)
                    {
                        child.Position = this.Position;
                        child.Zoom = this.Zoom;
                    }
                }
            }
        }

        public override Color SpriteColor
        {
            get;
            set;
        }

        public override IntRect Canevas
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
                if (value != this.defaultViewSize)
                {
                    this.defaultViewSize = value;

                    this.ClampZoom(this.Zoom);

                    this.ClampPosition(this.Position);
                }
            }
        }

        public override FloatRect Bounds
        {
            get
            {
                return new FloatRect(this.view.Center.X - this.view.Size.X / 2, this.view.Center.Y - this.view.Size.Y / 2, this.view.Size.X, this.view.Size.Y);
            }
        }

        public Vector2i MousePosition
        {
            get
            {
                Vector2i windowPosition = new Vector2i((int)this.Position.X, (int)this.Position.Y);
                Vector2i mousePosition =  new Vector2i((int)(Mouse.GetPosition().X * this.Zoom), (int)(Mouse.GetPosition().Y * this.Zoom));
                mousePosition += windowPosition - new Vector2i((int)this.view.Size.X, (int)this.view.Size.Y) / 2;

                return mousePosition;
            }
        }

        public ALayer2D(World2D world2D, ALayer layer)
        {
            this.ChildrenLayer2D = new List<ALayer2D>();

            this.view = new SFML.Graphics.View();
            this.defaultViewSize = this.view.Size;
            this.Position = new Vector2f(0, 0);
            this.Area = new Vector2i(0, 0);

            this.objectToObject2Ds = new Dictionary<AEntity, AEntity2D>();

            this.world2D = new WeakReference<World2D>(world2D);

            this.zoom = 1;

            this.parentLayer = layer;
            this.parentLayer.EntityAdded += OnEntityAdded;
            this.parentLayer.EntityRemoved += OnEntityRemoved;

            this.parentLayer.PositionChanged += OnPositionChanged;
            this.parentLayer.RotationChanged += OnRotationChanged;

            this.parentLayer.EntityPropertyChanged += OnEntityPropertyChanged;

            this.parentLayer.LevelStateChanged += OnLevelStateChanged;
        }

        public AEntity2D GetEntity2DFromEntity(AEntity entity)
        {
            return this.objectToObject2Ds[entity];
        }

        public virtual void InitializeLayer(IObject2DFactory factory)
        {
            foreach (AEntity entity in this.parentLayer.Entities)
            {
                this.AddEntity(entity);
            }
        }

        private void OnRotationChanged(float rotation)
        {
            this.Rotation = rotation;
        }

        private void OnPositionChanged(Vector2f position)
        {
            this.Position = position;
        }

        protected virtual void OnEntityRemoved(AEntity obj)
        {
            this.objectToObject2Ds.Remove(obj);
        }

        protected virtual void OnEntityAdded(AEntity obj)
        {
            this.AddEntity(obj);
        }

        protected AEntity2D AddEntity(AEntity obj)
        {
            if (this.world2D.TryGetTarget(out World2D world2D))
            {
                AEntity2D object2D = World2D.MappingObjectModelView[obj.GetType()].CreateObject2D(world2D, this, obj) as AEntity2D;

                this.objectToObject2Ds.Add(obj, object2D);

                return object2D;
            }
            return null;
        }

        protected virtual void OnEntityPropertyChanged(AEntity obj, string propertyName)
        {
            switch (propertyName)
            {
                case "Position":
                    this.objectToObject2Ds[obj].Position = obj.Position;
                    break;
                case "Rotation":
                    this.objectToObject2Ds[obj].Rotation = obj.Rotation;
                    break;
                case "IsActive":
                    this.objectToObject2Ds[obj].IsActive = obj.IsActive;
                    break;
            }
        }

        protected virtual void OnLevelStateChanged(string obj)
        {
            // To override
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

            if (newClampedPosition != this.view.Center)
            {
                this.view.Center = newClampedPosition;
            }
        }

        protected void ClampZoom(float newZoom)
        {
            float zoomXMax = ((float)this.Area.X) / this.defaultViewSize.X;
            float zoomYMax = ((float)this.Area.Y) / this.defaultViewSize.Y;

            float newZoomClamped = Math.Min(Math.Min(newZoom, zoomXMax), zoomYMax);

            if (newZoomClamped != this.zoom)
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
            if(eventType == ControlEventType.MOUSE_LEFT_CLICK
                || eventType == ControlEventType.MOUSE_RIGHT_CLICK)
            {
                Vector2i windowPosition = new Vector2i((int) this.Position.X, (int) this.Position.Y);

                Vector2i mousePosition = this.MousePosition;

                foreach (AEntity2D entity in this.objectToObject2Ds.Values)
                {
                    if (entity is IHitRect)
                    {
                        IHitRect hitRectEntity = entity as IHitRect;

                        if (hitRectEntity.HitZone.Contains(mousePosition.X, mousePosition.Y))
                        {
                            if (details == "pressed")
                            {
                                hitRectEntity.OnMousePressed(eventType);
                            }
                            else if (details == "released")
                            {
                                hitRectEntity.OnMouseReleased(eventType);
                            }
                        }
                    }
                }
            }
            return true;
        }

        public virtual void UpdateGraphics(Time deltaTime)
        {
            // To override
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            this.UpdateGraphics(deltaTime);

            List<AEntity2D> listObjects = this.objectToObject2Ds.Values.ToList();
            listObjects.Sort(new EntityComparer());

            foreach (AEntity2D entity2D in listObjects)
            {
                entity2D.UpdateGraphics(deltaTime);
            }

            SFML.Graphics.View defaultView = window.DefaultView;

            this.UpdateViewSize(defaultView.Size, deltaTime);

            this.view.Zoom(this.zoom);

            window.SetView(this.view);

            FloatRect bounds = this.Bounds;
            foreach (AEntity2D entity2D in listObjects)
            {
                if (entity2D.IsActive 
                    && entity2D.Bounds.Intersects(bounds))
                {
                    entity2D.DrawIn(window, deltaTime);
                }
            }

            window.SetView(defaultView);
        }

        public void SetCanevas(IntRect newCanevas)
        {
            // Nothing to do
        }

        public virtual void FlushEntities()
        {
            foreach (IObject2D object2D in this.objectToObject2Ds.Values)
            {
                object2D.Dispose();
            }
            this.objectToObject2Ds.Clear();

            this.ChildrenLayer2D.Clear();
        }

        public override void Dispose()
        {
            this.FlushEntities();

            this.parentLayer.EntityAdded -= OnEntityAdded;
            this.parentLayer.EntityRemoved -= OnEntityRemoved;

            this.parentLayer.PositionChanged -= OnPositionChanged;
            this.parentLayer.RotationChanged -= OnRotationChanged;

            this.parentLayer.EntityPropertyChanged -= OnEntityPropertyChanged;

            this.parentLayer.LevelStateChanged -= OnLevelStateChanged;

            this.parentLayer = null;
        }

        protected class EntityComparer : IComparer<AEntity2D>
        {
            public int Compare(AEntity2D x, AEntity2D y)
            {
                if(x.Priority > y.Priority)
                {
                    return 1;
                }
                else if(x.Priority < y.Priority)
                {
                    return -1;
                }
                return 0;
            }
        }
    }
}
