using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model;
using Metempsychoid.Model.Event;
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
        protected Vector2f defaultViewSize;

        protected WeakReference<World2D> world2D;
        protected ALayer parentLayer;

        protected Dictionary<AEntity, AEntity2D> objectToObject2Ds;
        protected Dictionary<AEntity2D, AEntity> object2DToObjects;

        //protected HashSet<IHitRect> focusedEntity2Ds;
        protected IHitRect focusedGraphicEntity2D;
        protected IHitRect selectedGraphicEntity2D;

        private bool mustUpdateMousePosition;
        private Vector2i mousePositionRelativeToWindow;
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

        public IHitRect FocusedGraphicEntity2D
        {
            get
            {
                return this.focusedGraphicEntity2D;
            }

            protected set
            {
                if (this.focusedGraphicEntity2D != value)
                {
                    if (this.focusedGraphicEntity2D != null)
                    {
                        this.focusedGraphicEntity2D.OnMouseUnFocused(this, ControlEventType.MOUSE_MOVED);
                    }

                    this.focusedGraphicEntity2D = value;

                    if (this.focusedGraphicEntity2D != null)
                    {
                        this.focusedGraphicEntity2D.OnMouseFocused(this, ControlEventType.MOUSE_MOVED);
                    }
                }
            }
        }

        public IHitRect SelectedGraphicEntity2D
        {
            get
            {
                return this.selectedGraphicEntity2D;
            }
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

                    this.mustUpdateMousePosition = true;
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

                    this.mustUpdateMousePosition = true;
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

                    this.mustUpdateMousePosition = true;
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

        protected virtual Vector2f DefaultViewSize
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
            get;
            private set;
        }

        public ALayer2D(World2D world2D, ALayer layer)
        {
            this.ChildrenLayer2D = new List<ALayer2D>();

            this.view = new SFML.Graphics.View();
            this.defaultViewSize = this.view.Size;

            this.Position = new Vector2f(0, 0);
            this.Area = new Vector2i(0, 0);

            this.objectToObject2Ds = new Dictionary<AEntity, AEntity2D>();
            this.object2DToObjects = new Dictionary<AEntity2D, AEntity>();

            //this.focusedEntity2Ds = new HashSet<IHitRect>();

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

        public AEntity GetEntityFromEntity2D(AEntity2D entity2D)
        {
            return this.object2DToObjects[entity2D];
        }

        public Vector2f GetPositionInWindow(Vector2f positionInScene)
        {
            Vector2f windowPosition = new Vector2f(this.Position.X - this.view.Size.X / 2, this.Position.Y - this.view.Size.Y / 2);

            Vector2f result = new Vector2f((positionInScene.X - windowPosition.X) / this.Zoom, (positionInScene.Y - windowPosition.Y) / this.Zoom);
            return result;
        }

        public Vector2f GetPositionInScene(Vector2f positionInScene)
        {
            Vector2f windowPosition = new Vector2f(this.Position.X - this.view.Size.X / 2, this.Position.Y - this.view.Size.Y / 2);

            Vector2f result = new Vector2f(positionInScene.X * this.Zoom + windowPosition.X, positionInScene.Y * this.Zoom + windowPosition.Y);
            return result;
        }

        public virtual void InitializeLayer(IObject2DFactory factory)
        {
            this.DefaultViewSize = this.view.Size;

            this.mustUpdateMousePosition = false;

            //this.focusedEntity2Ds.Clear();
            this.focusedGraphicEntity2D = null;
            this.selectedGraphicEntity2D = null;

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
            AEntity2D entity2DToRemove = this.objectToObject2Ds[obj];

            if (this.FocusedGraphicEntity2D == entity2DToRemove)
            {
                this.FocusedGraphicEntity2D = null;
            }

            this.object2DToObjects.Remove(entity2DToRemove);
            this.objectToObject2Ds.Remove(obj);

            entity2DToRemove.Dispose();
        }

        protected virtual void OnEntityAdded(AEntity obj)
        {
            this.AddEntity(obj);
        }

        protected virtual AEntity2D AddEntity(AEntity obj)
        {
            if (this.world2D.TryGetTarget(out World2D world2D))
            {
                AEntity2D object2D = World2D.MappingObjectModelView[obj.GetType()].CreateObject2D(world2D, this, obj) as AEntity2D;

                this.object2DToObjects.Add(object2D, obj);
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

            this.view.Zoom(this.zoom);

            if (this.mustUpdateMousePosition)
            {
                this.UpdateMousePosition();
                this.mustUpdateMousePosition = false;
            }
        }

        public virtual bool OnControlActivated(ControlEventType eventType, string details)
        {
            if(eventType == ControlEventType.MOUSE_LEFT_CLICK
                || eventType == ControlEventType.MOUSE_RIGHT_CLICK)
            {
                //Vector2i windowPosition = new Vector2i((int) this.Position.X, (int) this.Position.Y);

                //Vector2i mousePosition = this.MousePosition;

                //foreach (AEntity2D entity in this.objectToObject2Ds.Values)
                //{
                //    if (entity is IHitRect)
                //    {
                //        IHitRect hitRectEntity = entity as IHitRect;

                //        if (hitRectEntity.HitZone.Contains(mousePosition.X, mousePosition.Y))
                //        {
                //            if (details == "pressed")
                //            {
                //                hitRectEntity.OnMousePressed(this, eventType);
                //            }
                //            else if (details == "released")
                //            {
                //                hitRectEntity.OnMouseReleased(this, eventType);
                //            }
                //        }
                //    }
                //}
                if(this.FocusedGraphicEntity2D != null)
                {
                    if (details == "pressed")
                    {
                        this.SetSelectedGraphicEntity2D(this.FocusedGraphicEntity2D, eventType, details);
                    }
                }

                if (this.SelectedGraphicEntity2D != null)
                {
                    if (details == "click")
                    {
                        this.selectedGraphicEntity2D.OnMouseClicked(this, eventType);
                    }
                }

                if (this.SelectedGraphicEntity2D != null)
                {
                    if (details == "released")
                    {
                        this.SetSelectedGraphicEntity2D(null, eventType, details);
                    }
                }

            }
            return true;
        }

        public virtual void OnMouseMoved(Vector2i newPosition, Vector2i deltaPosition)
        {
            this.mousePositionRelativeToWindow = newPosition;

            this.UpdateMousePosition();
        }

        protected void SetSelectedGraphicEntity2D(IHitRect selectedEntity, ControlEventType eventType, string details)
        {
            if (this.selectedGraphicEntity2D != selectedEntity)
            {
                if (this.selectedGraphicEntity2D != null)
                {
                    this.selectedGraphicEntity2D.OnMouseReleased(this, eventType);
                }

                this.selectedGraphicEntity2D = selectedEntity;

                if (this.selectedGraphicEntity2D != null)
                {
                    this.selectedGraphicEntity2D.OnMousePressed(this, eventType);
                }
            }
        }

        protected void UpdateMousePosition()
        {
            Vector2i windowPosition = new Vector2i((int)this.Position.X, (int)this.Position.Y);
            Vector2i mousePosition = new Vector2i((int)(this.mousePositionRelativeToWindow.X * this.Zoom), (int)(this.mousePositionRelativeToWindow.Y * this.Zoom));
            mousePosition += windowPosition - new Vector2i((int)this.view.Size.X, (int)this.view.Size.Y) / 2;

            this.MousePosition = mousePosition;
        }

        public virtual void UpdateGraphics(Time deltaTime)
        {
            this.UpdateFocusedEntity2Ds();
        }

        protected virtual void UpdateFocusedEntity2Ds()
        {
            Vector2i mousePosition = this.MousePosition;

            AEntity2D newFocusedEntity2D = null;
            IEnumerable<AEntity2D> focusableEntities2D = this.GetEntities2DFocusable();
            foreach (AEntity2D entity2D in focusableEntities2D)
            {
                IHitRect hitRect = entity2D as IHitRect;

                if (hitRect != null
                    && hitRect.IsFocusable(this)
                    && hitRect.HitZone.Contains(mousePosition.X, mousePosition.Y))
                {
                    if (newFocusedEntity2D == null
                        || (Math.Abs(mousePosition.X - entity2D.Position.X) + Math.Abs(mousePosition.Y - entity2D.Position.Y)
                            < Math.Abs(mousePosition.X - newFocusedEntity2D.Position.X) + Math.Abs(mousePosition.Y - newFocusedEntity2D.Position.Y)))
                    {
                        newFocusedEntity2D = entity2D;
                    }
                }
            }

            this.FocusedGraphicEntity2D = newFocusedEntity2D as IHitRect;
        }

        protected virtual IEnumerable<AEntity2D> GetEntities2DFocusable()
        {
            return this.objectToObject2Ds.Values; 
        }

        internal void SendEventToWorld(Model.Event.EventType eventType, AEntity entityConcerned, string details)
        {
            if (this.world2D.TryGetTarget(out World2D world))
            {
                world.SendEventToWorld(new GameEvent(eventType, this.parentLayer, entityConcerned, details));
            }
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
            this.focusedGraphicEntity2D = null;

            foreach (IObject2D object2D in this.objectToObject2Ds.Values)
            {
                object2D.Dispose();
            }
            this.object2DToObjects.Clear();
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
