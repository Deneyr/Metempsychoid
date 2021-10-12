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

            this.AddTexturePath("backCardTexture", @"Assets\Graphics\Cards\Astrategia_Cartes_Verso.png");

            this.AddTexturePath("starHaloTexture", @"Assets\Graphics\Entities\StarHalo.png");

            this.AddTexturePath("META_beginning", @"Assets\Graphics\Cards\Astrategia_Cartes_Bataille.png");
            this.AddTexturePath("META_rules", @"Assets\Graphics\Cards\Astrategia_Cartes_Regles.png");
            this.AddTexturePath("META_architects", @"Assets\Graphics\Cards\Astrategia_Cartes_Credits.png");

            this.AddTexturePath("strength", @"Assets\Graphics\Cards\Astrategia_Cartes_Force.png");
            this.AddTexturePath("justice", @"Assets\Graphics\Cards\Astrategia_Cartes_Justice.png");
            this.AddTexturePath("moon", @"Assets\Graphics\Cards\Astrategia_Cartes_Lune.png");
            this.AddTexturePath("death", @"Assets\Graphics\Cards\Astrategia_Cartes_Mort.png");
            this.AddTexturePath("lover", @"Assets\Graphics\Cards\Astrategia_Cartes_Amoureux.png");
            this.AddTexturePath("priestess", @"Assets\Graphics\Cards\Astrategia_Cartes_Pretresse.png");
            this.AddTexturePath("temperance", @"Assets\Graphics\Cards\Astrategia_Cartes_Temperance.png");
            this.AddTexturePath("cart", @"Assets\Graphics\Cards\Astrategia_Cartes_Chariot.png");
            this.AddTexturePath("devil", @"Assets\Graphics\Cards\Astrategia_Cartes_Diable.png");
            this.AddTexturePath("fool", @"Assets\Graphics\Cards\Astrategia_Cartes_Fou.png");
            this.AddTexturePath("hierophant", @"Assets\Graphics\Cards\Astrategia_Cartes_Hierophante.png");
            this.AddTexturePath("magician", @"Assets\Graphics\Cards\Astrategia_Cartes_Magicien.png");
            this.AddTexturePath("world", @"Assets\Graphics\Cards\Astrategia_Cartes_Monde.png");
            this.AddTexturePath("emperor", @"Assets\Graphics\Cards\Astrategia_Cartes_Empereur.png");
            this.AddTexturePath("hangedMan", @"Assets\Graphics\Cards\Astrategia_Cartes_Pendu.png");
            this.AddTexturePath("hermite", @"Assets\Graphics\Cards\Astrategia_Cartes_Hermite.png");
            this.AddTexturePath("rock", @"Assets\Graphics\Cards\Astrategia_Cartes_Rocher.png");
            this.AddTexturePath("empress", @"Assets\Graphics\Cards\Astrategia_Cartes_Imperatrice.png");
            this.AddTexturePath("wheel", @"Assets\Graphics\Cards\Astrategia_Cartes_Roue.png");
            this.AddTexturePath("tower", @"Assets\Graphics\Cards\Astrategia_Cartes_Tour.png");
            this.AddTexturePath("judgement", @"Assets\Graphics\Cards\Astrategia_Cartes_Jugement.png");
            this.AddTexturePath("sun", @"Assets\Graphics\Cards\Astrategia_Cartes_Soleil.png");
            this.AddTexturePath("star", @"Assets\Graphics\Cards\Astrategia_Cartes_Etoile.png");

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
