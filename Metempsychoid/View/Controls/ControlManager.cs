using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SFML.Window.Keyboard;
using static SFML.Window.Mouse;

namespace Metempsychoid.View.Controls
{
    public class ControlManager
    {
        private Vector2i previousMousePosition;

        public event Action<ControlEventType, string> ControlActivated;

        public Vector2i MousePosition
        {
            get
            {
                return Mouse.GetPosition();
            }
        }

        public ControlManager(RenderWindow window)
        {
            this.previousMousePosition = this.MousePosition;

            window.KeyPressed += OnKeyPressed;

            window.MouseWheelScrolled += OnMouseWheelScrolled;

            window.MouseButtonPressed += OnMouseButtonPressed;
            window.MouseButtonReleased += OnMouseButtonReleased;
            window.MouseMoved += OnMouseMoved;
        }

        private void OnMouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            this.NotifyControlActivated(ControlEventType.MOUSE_WHEEL, e.Delta.ToString());
        }

        public bool IsKeyPressed(Key key)
        {
            return Keyboard.IsKeyPressed(key);
        }

        public bool IsMousePressed(Button button)
        {
            return Mouse.IsButtonPressed(button);
        }

        private void OnMouseMoved(object sender, SFML.Window.MouseMoveEventArgs e)
        {
            Vector2i newPosition = new Vector2i(e.X, e.Y);
            Vector2i deltaPosition = this.previousMousePosition - newPosition;
            this.previousMousePosition = newPosition;

            this.NotifyControlActivated(ControlEventType.MOUSE_MOVED, deltaPosition.X + "," + deltaPosition.Y);
        }

        private void OnMouseButtonReleased(object sender, SFML.Window.MouseButtonEventArgs e)
        {

        }

        private void OnMouseButtonPressed(object sender, SFML.Window.MouseButtonEventArgs e)
        {

        }

        /// <summary>
        /// Function called when a key is pressed
        /// </summary>
        private void OnKeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            var window = (SFML.Window.Window)sender;
            if (e.Code == SFML.Window.Keyboard.Key.Escape)
            {
                window.Close();
            }
            else if (e.Code == SFML.Window.Keyboard.Key.Z)
            {
                this.NotifyControlActivated(ControlEventType.UP, string.Empty);
            }
            else if (e.Code == SFML.Window.Keyboard.Key.S)
            {
                this.NotifyControlActivated(ControlEventType.DOWN, string.Empty);
            }
            else if (e.Code == SFML.Window.Keyboard.Key.Q)
            {
                this.NotifyControlActivated(ControlEventType.LEFT, string.Empty);
            }
            else if (e.Code == SFML.Window.Keyboard.Key.D)
            {
                this.NotifyControlActivated(ControlEventType.RIGHT, string.Empty);
            }
        }

        private void NotifyControlActivated(ControlEventType controlEvent, string details)
        {
            this.ControlActivated?.Invoke(controlEvent, details);
        }

        public void Dispose(RenderWindow window)
        {
            window.KeyPressed -= OnKeyPressed;

            window.MouseButtonPressed -= OnMouseButtonPressed;
            window.MouseButtonReleased -= OnMouseButtonReleased;
            window.MouseMoved -= OnMouseMoved;
        }
    }
}
