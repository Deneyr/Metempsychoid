using Astrategia.Model.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.Model.Layer.BackgroundLayer
{
    public class ImageBackgroundLayer: ALayer
    {
        private string currentImageId;

        public event Action<string> CurrentImageIdChanged;

        public string PreInitImageId
        {
            get;
            set;
        }

        public string CurrentImageId
        {
            get
            {
                return this.currentImageId;
            }

            set
            {
                if(this.currentImageId != value)
                {
                    this.currentImageId = value;

                    this.CurrentImageIdChanged?.Invoke(this.currentImageId);
                }
            }
        }


        public ImageBackgroundLayer()
        {
        }

        protected override void InternalInitializeLayer(World world, ALevelNode levelNode)
        {
            this.currentImageId = this.PreInitImageId;
        }
    }
}
