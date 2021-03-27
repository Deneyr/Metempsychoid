using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model;
using Metempsychoid.Model.Layer.BackgroundLayer;
using SFML.System;

namespace Metempsychoid.View.Layer2D.BackgroundLayer2D
{
    public class BackgroundLayer2DFactory : AObject2DFactory
    {
        private static readonly string BACKGROUND_PATH = @"Assets\Graphics\Backgrounds";

        public Vector2i Area
        {
            get;
            private set;
        }

        public BackgroundLayer2DFactory(string backgroundName)
        {
            DirectoryInfo directory = new DirectoryInfo(BACKGROUND_PATH + Path.DirectorySeparatorChar + backgroundName);
            FileInfo header = new FileInfo(directory.FullName + Path.DirectorySeparatorChar + backgroundName + ".header");

            if (header.Exists)
            {
                StreamReader file = new StreamReader(header.FullName);
                string line = file.ReadLine();
                string[] token = line.Split(':');

                this.Area = new Vector2i(int.Parse(token[0]), int.Parse(token[1]));

                DirectoryInfo subDirectory = new DirectoryInfo(directory.FullName + Path.DirectorySeparatorChar + "1");
                FileInfo[] images = subDirectory.GetFiles("*.jpg");

                foreach(FileInfo image in images)
                {
                    this.texturesPath.Add(image.FullName);
                }
            }

            this.InitializeFactory();
        }

        public override IObject2D CreateObject2D(World2D world2D, IObject obj)
        {
            if (obj is BackgroundLayer)
            {
                BackgroundLayer backgroundLayer = obj as BackgroundLayer;

                return new BackgroundLayer2D(world2D, this, backgroundLayer);
            }

            return null;
        }
    }
}
