using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Metempsychoid.View
{
    public abstract class ALayer2D: IObject2D
    {
        protected SFML.Graphics.View view;

        protected List<IObject2D> object2DsList;

        protected float zoom;

        public Vector2f Position
        {
            get
            {
                return this.view.Center;
            }

            protected set
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

            protected set
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

        public ALayer2D()
        {
            this.view = new SFML.Graphics.View();

            this.object2DsList = new List<IObject2D>();
        }

        public void DrawIn(RenderWindow window)
        {
            SFML.Graphics.View defaultView = window.DefaultView;
            this.view.Size = defaultView.Size;

            window.SetView(this.view);

            foreach(IObject2D object2D in this.object2DsList)
            {
                if (object2D.Bounds.Intersects(this.Bounds))
                {
                    object2D.DrawIn(window);
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
            foreach (IObject2D object2D in this.object2DsList)
            {
                object2D.Dispose();
            }
        }
    }
}
