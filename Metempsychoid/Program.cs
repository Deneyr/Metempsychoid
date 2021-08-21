using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid
{
    class Program
    {
        static void Main(string[] args)
        {
            //View.Text2D.TextParagraphFactory factory = new View.Text2D.TextParagraphFactory();

            //factory.Culture = "fr";

            MainWindow mainWindow = new MainWindow();

            mainWindow.Run();
        }
    }
}
