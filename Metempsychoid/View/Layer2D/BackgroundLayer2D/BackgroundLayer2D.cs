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

        public float ZoomRatio
        {
            get
            {
                return (float)Math.Log(this.Zoom, 1.5);
            }

            set
            {
                this.Zoom = (float)Math.Pow(1.5, value);
            }
        }

        public BackgroundLayer2D(World2D world2D, IObject2DFactory factory, BackgroundLayer layer) : 
            base(world2D, layer)
        {
            this.Area = (factory as BackgroundLayer2DFactory).Area;

            this.nameToTiles = new Dictionary<string, TileBackgoundObject2D>();
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

                    this.ZoomRatio -= delta;

                    break;
                case ControlEventType.MOUSE_MOVED:

                    if (this.world2D.TryGetTarget(out World2D world2D))
                    {
                        if (world2D.ControlManager.IsMousePressed(SFML.Window.Mouse.Button.Left))
                        {
                            string[] token = details.Split(',');

                            Vector2f deltaPosition = new Vector2f(int.Parse(token[0]), int.Parse(token[1]));
                            deltaPosition *= this.Zoom;

                            this.Position += deltaPosition;
                        }
                    }

                    break;
            }

            return true;
        }

        public override void InitializeLayer(IObject2DFactory factory)
        {
            base.InitializeLayer(factory);

            this.Position = this.parentLayer.Position;

            this.Rotation = this.parentLayer.Rotation;

            foreach (KeyValuePair<string, Texture> keyValuePair in factory.Resources)
            {
                this.nameToTiles.Add(Path.GetFileNameWithoutExtension(keyValuePair.Key), new TileBackgoundObject2D(keyValuePair.Value, Path.GetFileNameWithoutExtension(keyValuePair.Key)));
            }
        }

        public override void FlushEntities()
        {
            base.FlushEntities();

            foreach(TileBackgoundObject2D tile in this.nameToTiles.Values)
            {
                tile.Dispose();
            }

            this.nameToTiles.Clear();
        }
    }
}
