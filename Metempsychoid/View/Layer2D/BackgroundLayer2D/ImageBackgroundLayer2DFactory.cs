using Astrategia.Model;
using Astrategia.Model.Layer.BackgroundLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.Layer2D.BackgroundLayer2D
{
    public class ImageBackgroundLayer2DFactory : AObject2DFactory
    {
        public ImageBackgroundLayer2DFactory()
        {
            this.AddTexturePath("slide_credits1", @"Assets\Graphics\Backgrounds\Slides\credits_slide1.png");

            this.AddTexturePath("slide_rules1", @"Assets\Graphics\Backgrounds\Slides\rules_slide1.png");
            this.AddTexturePath("slide_rules2", @"Assets\Graphics\Backgrounds\Slides\rules_slide2.png");
            this.AddTexturePath("slide_rules3", @"Assets\Graphics\Backgrounds\Slides\rules_slide3.png");
            this.AddTexturePath("slide_rules4", @"Assets\Graphics\Backgrounds\Slides\rules_slide4.png");

            this.AddSoundPath("slidePassed", @"Assets\Sounds\cardDrawn.ogg");
        }

        public override IObject2D CreateObject2D(World2D world2D, IObject obj)
        {
            if (obj is ImageBackgroundLayer)
            {
                ImageBackgroundLayer imageBackgroundLayer = obj as ImageBackgroundLayer;

                return new ImageBackgroundLayer2D(world2D, this, imageBackgroundLayer);
            }

            return null;
        }
    }
}