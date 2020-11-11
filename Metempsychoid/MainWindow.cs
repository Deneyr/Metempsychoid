using Metempsychoid.Model;
using Metempsychoid.View;
using Metempsychoid.View.Controls;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Metempsychoid
{
    public class MainWindow
    {
        public static readonly int MODEL_TO_VIEW = 1;

        public World World
        {
            get;
            private set;
        }

        public World2D World2D
        {
            get;
            private set;
        }

        public RenderWindow Window
        {
            get;
            private set;
        }

    public MainWindow()
        {
            var mode = new SFML.Window.VideoMode(800, 600);
            this.Window = new SFML.Graphics.RenderWindow(SFML.Window.VideoMode.FullscreenModes[0], "Metempsychoid", SFML.Window.Styles.Fullscreen);
            // this.window = new SFML.Graphics.RenderWindow(mode, "Metempsychoid");
            this.Window.SetVerticalSyncEnabled(true);

            this.World = new World();

            this.World2D = new World2D(this);
        }

        public void Run()
        {
            Clock clock = new Clock();

            // Test
            this.World.TestLevel();

            // Start the game loop
            while (this.Window.IsOpen)
            {
                Time deltaTime = clock.Restart();

                // Process events
                this.Window.DispatchEvents();

                // Game logic update
                this.World.UpdateLogic(null, deltaTime);

                // Update Animation
                AObject2D.UpdateZoomAnimationManager(deltaTime);

                // Draw window
                this.Window.Clear();

                this.World2D.DrawIn(this.Window, deltaTime);

                // Finally, display the rendered frame on screen
                this.Window.Display();
            }

            this.World2D.Dispose(this);
            this.World.Dispose();

            AObject2D.StopAnimationManager();
        }      
    }
}
