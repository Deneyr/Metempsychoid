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
            this.Window = new SFML.Graphics.RenderWindow(SFML.Window.VideoMode.DesktopMode, "Metempsychoid", Styles.Fullscreen);
            //this.Window = new SFML.Graphics.RenderWindow(mode, "Metempsychoid");
            this.Window.SetVerticalSyncEnabled(true);

            this.World = new World();

            this.World2D = new World2D(this);
        }

        public void Run()
        {
            Clock clock = new Clock();

            // Initialize the first world node of the game.
            this.World.InitializeGameNode();

            // Start the game loop
            while (this.Window.IsOpen)
            {
                Time deltaTime = clock.Restart();

                // Process events
                this.Window.DispatchEvents();

                // Update Model Animations
                AObject.UpdateAnimationManager(deltaTime);

                // Game logic update
                this.World.UpdateLogic(null, deltaTime);

                // Update View Animations
                AObject2D.UpdateAnimationManager(deltaTime);

                // Draw window
                this.Window.Clear();

                this.World2D.DrawIn(this.Window, deltaTime);

                // Finally, display the rendered frame on screen
                this.Window.Display();
            }

            this.World2D.Dispose();
            this.World.Dispose();
        }      
    }
}
