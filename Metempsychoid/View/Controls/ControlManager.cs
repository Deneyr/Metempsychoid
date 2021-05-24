using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private Dictionary<Button, Stopwatch> buttonToStopwatch;

        public event Action<ControlEventType, string> ControlActivated;

        public event Action<Vector2i, Vector2i> MouseMoved;

        public ControlManager(RenderWindow window)
        {
            this.buttonToStopwatch = new Dictionary<Button, Stopwatch>();

            this.previousMousePosition = Mouse.GetPosition(window);

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

            this.NotifyMouseMoved(newPosition, deltaPosition);
        }

        private void OnMouseButtonReleased(object sender, SFML.Window.MouseButtonEventArgs e)
        {
            Stopwatch stopwatch = this.buttonToStopwatch[e.Button];
            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds < 200)
            {
                if (e.Button == Button.Left)
                {
                    this.NotifyControlActivated(ControlEventType.MOUSE_LEFT_CLICK, "click");
                }
                else if (e.Button == Button.Right)
                {
                    this.NotifyControlActivated(ControlEventType.MOUSE_RIGHT_CLICK, "click");
                }
            }

            if (e.Button == Button.Left)
            {
                this.NotifyControlActivated(ControlEventType.MOUSE_LEFT_CLICK, "released");
            }
            else if (e.Button == Button.Right)
            {
                this.NotifyControlActivated(ControlEventType.MOUSE_RIGHT_CLICK, "released");
            }

            this.buttonToStopwatch.Remove(e.Button);
        }

        private void OnMouseButtonPressed(object sender, SFML.Window.MouseButtonEventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();
            this.buttonToStopwatch.Add(e.Button, stopwatch);
            stopwatch.Start();

            if (e.Button == Button.Left)
            {
                this.NotifyControlActivated(ControlEventType.MOUSE_LEFT_CLICK, "pressed");
            }
            else if (e.Button == Button.Right)
            {
                this.NotifyControlActivated(ControlEventType.MOUSE_RIGHT_CLICK, "pressed");
            }
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

        private void NotifyMouseMoved(Vector2i newPosition, Vector2i deltaPosition)
        {
            this.MouseMoved?.Invoke(newPosition, deltaPosition);
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
