﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrategia.Model;
using Astrategia.Model.Layer.BackgroundLayer;
using Astrategia.View.Controls;
using SFML.Graphics;
using SFML.System;

namespace Astrategia.View.Layer2D.BackgroundLayer2D
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
            base(world2D, factory, layer)
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

        public override bool OnControlActivated(ControlEventType eventType, string details, bool mustForwardEvent)
        {
            switch (eventType)
            {
                case ControlEventType.MOUSE_WHEEL:
                    float delta = float.Parse(details);

                    this.ZoomRatio -= delta;

                    break;
            }

            return mustForwardEvent;
        }

        public override void OnMouseMoved(Vector2i newPosition, Vector2i deltaPosition)
        {
            base.OnMouseMoved(newPosition, deltaPosition);

            if (this.world2D.TryGetTarget(out World2D world2D))
            {
                if (world2D.ControlManager.IsMousePressed(SFML.Window.Mouse.Button.Left))
                {
                    Vector2f deltaPositionFloat = new Vector2f(deltaPosition.X, deltaPosition.Y);
                    deltaPositionFloat *= this.Zoom;

                    this.Position += deltaPositionFloat;
                }
            }
        }

        public override void InitializeLayer(IObject2DFactory factory)
        {
            base.InitializeLayer(factory);

            this.Position = this.parentLayer.Position;

            this.Rotation = this.parentLayer.Rotation;

            foreach (string texturePath in factory.TexturesPath)
            {
                string idTexture = Path.GetFileNameWithoutExtension(texturePath);
                this.nameToTiles.Add(idTexture, new TileBackgoundObject2D(this, factory.GetTextureById(idTexture), idTexture));
            }
        }

        public override void FlushEntities()
        {
            base.FlushEntities();

            foreach (TileBackgoundObject2D tile in this.nameToTiles.Values)
            {
                tile.Dispose();
            }

            this.nameToTiles.Clear();
        }
    }
}
