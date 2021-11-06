using System;
using System.Collections.Generic;
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
    public class ImageBackgroundLayer2D : ALayer2D
    {
        private ImageBackgroundObject2D imageBackground2D;

        public ImageBackgroundLayer2D(World2D world2D, IObject2DFactory layerFactory, ALayer layer) 
            : base(world2D, layerFactory, layer)
        {
            this.Area = new Vector2i(int.MaxValue, int.MaxValue);

            this.imageBackground2D = new ImageBackgroundObject2D(this);

            (layer as ImageBackgroundLayer).CurrentImageIdChanged += this.OnCurrentImageIdChanged;
        }

        public override void InitializeLayer(IObject2DFactory factory)
        {
            base.InitializeLayer(factory);

            this.imageBackground2D.ObjectSprite.Texture = null;
            this.imageBackground2D.DisplayImage((this.parentLayer as ImageBackgroundLayer).CurrentImageId);
        }

        public override void InitializeSpatialLayer()
        {
            IntRect imageCanevas = this.imageBackground2D.Canevas;

            float maxZoom = Math.Max(imageCanevas.Width / this.DefaultViewSize.X, imageCanevas.Height / this.DefaultViewSize.Y);

            this.Zoom = maxZoom;
        }

        private void OnCurrentImageIdChanged(string obj)
        {
            this.PlaySound("slidePassed");

            this.imageBackground2D.DisplayImage(obj);
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            base.UpdateGraphics(deltaTime);

            this.imageBackground2D.UpdateGraphics(deltaTime);
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            base.DrawIn(window, deltaTime);

            SFML.Graphics.View defaultView = window.DefaultView;
            window.SetView(this.view);

            this.imageBackground2D.DrawIn(window, deltaTime);

            window.SetView(defaultView);
        }

        public override bool OnControlActivated(Controls.ControlEventType eventType, string details, bool mustForwardEvent)
        {
            if (mustForwardEvent == false)
            {
                return mustForwardEvent;
            }

            mustForwardEvent = base.OnControlActivated(eventType, details, mustForwardEvent);

            if (eventType == ControlEventType.MOUSE_LEFT_CLICK && details == "pressed")
            {
                this.SendEventToWorld(Model.Event.EventType.LEVEL_PHASE_CHANGE, null, null);
            }

            return mustForwardEvent;
        }

        public override void Dispose()
        {
            (this.parentLayer as ImageBackgroundLayer).CurrentImageIdChanged -= this.OnCurrentImageIdChanged;

            base.Dispose();
        }
    }
}
