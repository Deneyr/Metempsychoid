using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model;
using Metempsychoid.Model.Layer.BackgroundLayer;
using Metempsychoid.View.Controls;
using SFML.Graphics;
using SFML.System;

namespace Metempsychoid.View.Layer2D.BackgroundLayer2D
{
    public class BackgroundLayer2D : ALayer2D
    {
        private Dictionary<string, TileBackgoundObject2D> nameToTiles;

        private float zoomRatio;

        public Vector2i Area
        {
            get;
            private set;
        }

        public BackgroundLayer2D(World2D world2D, IObject2DFactory factory, BackgroundLayer layer) : 
            base(world2D, layer)
        {
            this.Area = (factory as BackgroundLayer2DFactory).Area;

            this.nameToTiles = new Dictionary<string, TileBackgoundObject2D>();

            this.zoomRatio = 1;

            foreach(KeyValuePair<string, Texture> keyValuePair in factory.Resources)
            {
                this.nameToTiles.Add(Path.GetFileNameWithoutExtension(keyValuePair.Key), new TileBackgoundObject2D(keyValuePair.Value, Path.GetFileNameWithoutExtension(keyValuePair.Key)));
            }

            Console.WriteLine();
        }

        protected override void UpdateViewSize(Vector2f viewSize, Time deltaTime)
        {
            base.UpdateViewSize(viewSize, deltaTime);

            float zoomRatio = (float)Math.Pow(1.5, this.zoom);

            float zoomXMax = ((float)this.Area.X) / this.view.Size.X;
            float zoomYMax = ((float)this.Area.Y) / this.view.Size.Y;

            this.zoomRatio = Math.Min(Math.Min(zoomRatio, zoomXMax), zoomYMax);

            float minXPosition = Math.Min(0, -this.Area.X / 2f + this.view.Size.X * this.zoomRatio / 2);
            float minYPosition = Math.Min(0, -this.Area.Y / 2f + this.view.Size.Y * this.zoomRatio / 2);

            float maxXPosition = Math.Max(0, this.Area.X / 2f - this.view.Size.X * this.zoomRatio / 2);
            float maxYPosition = Math.Max(0, this.Area.Y / 2f - this.view.Size.Y * this.zoomRatio / 2);

            float newXPosition = Math.Min(Math.Max(this.Position.X, minXPosition), maxXPosition);
            float newYPosition = Math.Min(Math.Max(this.Position.Y, minYPosition), maxYPosition);
            this.Position = new Vector2f(newXPosition, newYPosition);

            this.view.Zoom(this.zoomRatio);

            //if(this.world2D.TryGetTarget(out World2D world2D))
            //{
            //    float timeInSec = deltaTime.AsMilliseconds() / 1000f;
            //    float speed = 1000;

            //    if (world2D.ControlManager.IsKeyPressed(SFML.Window.Keyboard.Key.Z))
            //    {
            //        this.Position += new Vector2f(0, -timeInSec * speed);
            //    }

            //    if (world2D.ControlManager.IsKeyPressed(SFML.Window.Keyboard.Key.S))
            //    {
            //        this.Position += new Vector2f(0, timeInSec * speed);
            //    }

            //    if (world2D.ControlManager.IsKeyPressed(SFML.Window.Keyboard.Key.Q))
            //    {
            //        this.Position += new Vector2f(-timeInSec * speed, 0);
            //    }

            //    if (world2D.ControlManager.IsKeyPressed(SFML.Window.Keyboard.Key.D))
            //    {
            //        this.Position += new Vector2f(timeInSec * speed, 0);
            //    }
            //}
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            base.DrawIn(window, deltaTime);

            SFML.Graphics.View defaultView = window.DefaultView;
            window.SetView(this.view);

            FloatRect bounds = this.Bounds;
            foreach (IObject2D object2D in this.nameToTiles.Values)
            {
                if (object2D.Bounds.Intersects(bounds))
                {
                    object2D.DrawIn(window, deltaTime);
                }
            }

            window.SetView(defaultView);
        }

        public override bool OnControlActivated(ControlEventType eventType, string details)
        {
            switch (eventType)
            {
                case ControlEventType.MOUSE_WHEEL:
                    int delta = int.Parse(details);

                    this.SetZoom(this.zoom - delta);

                    //if (this.zoom + delta < 0)
                    //{
                    //    this.SetZoom(0);
                    //}
                    //else
                    //{
                    //    this.SetZoom(this.zoom + delta);
                    //}

                    break;
                case ControlEventType.MOUSE_MOVED:

                    if (this.world2D.TryGetTarget(out World2D world2D))
                    {
                        if (world2D.ControlManager.IsMousePressed(SFML.Window.Mouse.Button.Left))
                        {
                            string[] token = details.Split(',');

                            Vector2f deltaPosition = new Vector2f(int.Parse(token[0]), int.Parse(token[1]));
                            deltaPosition *= this.zoomRatio;

                            this.Position += deltaPosition;
                        }
                    }

                    break;
            }

            return true;
        }

    }
}
