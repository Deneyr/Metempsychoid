using Metempsychoid.Model;
using Metempsychoid.Model.Card;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Card2D
{
    public class CardEntity2DFactory : AObject2DFactory
    {
        //private Dictionary<string, int> cardNameToIndex;

        public CardEntity2DFactory()
        {
            this.AddTexturePath("distorsionTexture", @"Assets\Graphics\Shaders\distortion_map.png");

            this.AddTexturePath("backCardTexture", @"Assets\Graphics\Cards\backCard.png");

            this.AddTexturePath("starHaloTexture", @"Assets\Graphics\Entities\StarHalo.png");

            this.AddTexturePath("testWheelTexture", @"Assets\Graphics\Cards\wheelCard.jpg");

            this.AddTexturePath("strength", @"Assets\Graphics\Cards\Carte_LaForce.png");
            this.AddTexturePath("justice", @"Assets\Graphics\Cards\Carte_LaJustice.png");
            this.AddTexturePath("moon", @"Assets\Graphics\Cards\Carte_LaLune.png");
            this.AddTexturePath("death", @"Assets\Graphics\Cards\Carte_LaMort.png");
            this.AddTexturePath("lover", @"Assets\Graphics\Cards\Carte_LAmoureux.png");
            this.AddTexturePath("priestess", @"Assets\Graphics\Cards\Carte_LaPretresse.png");
            this.AddTexturePath("temperance", @"Assets\Graphics\Cards\Carte_LaTemperence.png");
            this.AddTexturePath("cart", @"Assets\Graphics\Cards\Carte_LeChariot.png");
            this.AddTexturePath("devil", @"Assets\Graphics\Cards\Carte_LeDiable.png");
            this.AddTexturePath("fool", @"Assets\Graphics\Cards\Carte_LeFou.png");
            this.AddTexturePath("hierophant", @"Assets\Graphics\Cards\Carte_LeHierophante.png");
            this.AddTexturePath("magician", @"Assets\Graphics\Cards\Carte_LeMagicien.png");
            this.AddTexturePath("world", @"Assets\Graphics\Cards\Carte_LeMonde.png");
            this.AddTexturePath("emperor", @"Assets\Graphics\Cards\Carte_LEmpereur.png");
            this.AddTexturePath("hangedMan", @"Assets\Graphics\Cards\Carte_LePendu.png");
            this.AddTexturePath("hermite", @"Assets\Graphics\Cards\Carte_LErmite.png");
            this.AddTexturePath("rock", @"Assets\Graphics\Cards\Carte_LeRocher.png");
            this.AddTexturePath("empress", @"Assets\Graphics\Cards\Carte_LImperatrice.png");
            this.AddTexturePath("wheel", @"Assets\Graphics\Cards\LaRoue.png");
            this.AddTexturePath("tower", @"Assets\Graphics\Cards\LaTour.png");
            this.AddTexturePath("judgement", @"Assets\Graphics\Cards\LeJugement.png");
            this.AddTexturePath("sun", @"Assets\Graphics\Cards\LeSoleil.png");
            this.AddTexturePath("star", @"Assets\Graphics\Cards\LEtoile.png");

            // this.cardNameToIndex = new Dictionary<string, int>();
            //this.cardNameToIndex.Add("wheel", 5);

            //this.cardNameToIndex.Add("strength", 6);
            //this.cardNameToIndex.Add("justice", 7);
            //this.cardNameToIndex.Add("moon", 8);
            //this.cardNameToIndex.Add("death", 9);
            //this.cardNameToIndex.Add("lover", 10);
            //this.cardNameToIndex.Add("priestess", 11);
            //this.cardNameToIndex.Add("temperance", 12);
            //this.cardNameToIndex.Add("cart", 13);
            //this.cardNameToIndex.Add("devil", 14);
            //this.cardNameToIndex.Add("fool", 15);
            //this.cardNameToIndex.Add("hierophant", 16);
            //this.cardNameToIndex.Add("magician", 17);
            //this.cardNameToIndex.Add("world", 18);
            //this.cardNameToIndex.Add("emperor", 19);
            //this.cardNameToIndex.Add("hangedMan", 20);
            //this.cardNameToIndex.Add("hermite", 21);
            //this.cardNameToIndex.Add("rock", 22);
            //this.cardNameToIndex.Add("empress", 23);
            //this.cardNameToIndex.Add("wheel", 24);
            //this.cardNameToIndex.Add("tower", 25);
            //this.cardNameToIndex.Add("judgement", 26);
            //this.cardNameToIndex.Add("sun", 27);
            //this.cardNameToIndex.Add("star", 28);

            // Sounds
            this.AddSoundPath("cardDrawn", @"Assets\Sounds\cardDrawn.ogg");
            this.AddSoundPath("cardFocused", @"Assets\Sounds\cardFocused.ogg");
            this.AddSoundPath("cardPicked", @"Assets\Sounds\cardPicked.ogg");

            //this.InitializeFactory();
        }

        public override IObject2D CreateObject2D(World2D world2D, IObject obj)
        {
            return this.CreateObject2D(world2D, null, obj);
        }

        public override IObject2D CreateObject2D(World2D world2D, ALayer2D layer2D, IObject obj)
        {
            if (obj is CardEntity)
            {
                CardEntity entity = obj as CardEntity;

                return new CardEntity2D(this, layer2D, entity);
            }

            return null;
        }

        //public override void OnTextureLoaded(string path, Texture texture)
        //{
        //    if (this.Resources.ContainsKey(path))
        //    {
        //        texture.Smooth = true;

        //        this.Resources[path] = texture;
        //    }
        //}

        //public int GetIndexFromCardName(string cardName)
        //{
        //    return this.cardNameToIndex[cardName];
        //}
    }
}
