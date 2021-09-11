using Metempsychoid.Model.Card.Behaviors;
using Metempsychoid.Model.Constellations;
using Metempsychoid.Model.Layer.BoardGameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Card
{
    public class CardFactory
    {
        Dictionary<string, CardTemplate> cardLibrary;

        public CardFactory()
        {
            this.InitializeCardLibrary();
        }

        public Card CreateCard(string name, Player.Player player)
        {
            Card card = new Card(this.cardLibrary[name], player);

            return card;
        }

        private void InitializeCardLibrary()
        {
            this.cardLibrary = new Dictionary<string, CardTemplate>();

            CardTemplate cardTemplate;

            cardTemplate = new CardTemplate("strength", "strength_title", "strength_poem", "strength_effect", 0);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateStrengthConstellation());

            cardTemplate.CardBehaviors.Add(new StrengthPassiveBehavior(1));
            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("justice", "justice_title", "justice_poem", "justice_effect", 1);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateJusticeConstellation());

            cardTemplate.CardBehaviors.Add(new JusticeActiveBehavior(1));
            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("moon", "moon_title", "moon_poem", "moon_effect", 1);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateMoonConstellation());

            cardTemplate.CardBehaviors.Add(new MoonPassiveBehavior(2, ConstellationPatternFactory.CreateMoonConstellation()));
            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("death", "death_title", "death_poem", "death_effect", 1);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateDeathConstellation());

            cardTemplate.CardBehaviors.Add(new DeathActiveBehavior(1));
            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("lover", "lover_title", "lover_poem", "lover_effect", 1);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateLoverConstellation());

            cardTemplate.CardBehaviors.Add(new LoverPassiveBehavior(1));
            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("priestess", "priestess_title", "priestess_poem", "priestess_effect", 1);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreatePriestessConstellation());

            cardTemplate.CardBehaviors.Add(new PriestessPassiveBehavior(1, "priestess", "hierophant"));
            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("temperance", "temperance_title", "temperance_poem", "temperance_effect", 1);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateTemperanceConstellation());

            cardTemplate.CardBehaviors.Add(new TemperancePassiveBehavior(-1));
            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("cart", "cart_title", "cart_poem", "cart_effect", 1);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateCartConstellation());

            cardTemplate.CardBehaviors.Add(new CartActiveBehavior(1));
            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("devil", "devil_title", "devil_poem", "devil_effect", 1);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateDevilConstellation());

            cardTemplate.CardBehaviors.Add(new DevilPassiveBehavior(1));
            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("fool", "fool_title", "fool_poem", "fool_effect", 1);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateFoolConstellation());

            cardTemplate.CardBehaviors.Add(new FoolActiveBehavior());
            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("hierophant", "hierophant_title", "hierophant_poem", "hierophant_effect", 1);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateHierophantConstellation());

            cardTemplate.CardBehaviors.Add(new HierophantActiveBehavior(new List<string> { "priestess"}));
            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("magician", "magician_title", "magician_poem", "magician_effect", 1);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateMagicianConstellation());

            cardTemplate.CardBehaviors.Add(new MagicianActiveBehavior());
            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("world", "world_title", "world_poem", "world_effect", 1);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateWorldConstellation());

            cardTemplate.CardBehaviors.Add(new WorldActiveBehavior());
            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("emperor", "emperor_title", "emperor_poem", "emperor_effect", 1);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateEmperorConstellation());

            cardTemplate.CardBehaviors.Add(new EmperorActiveBehavior(1));
            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("hangedMan", "hangedMan_title", "hangedMan_poem", "hangedMan_effect", 0);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateHangedManConstellation());

            cardTemplate.CardBehaviors.Add(new HangedManPassiveBehavior(1));
            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("hermite", "hermite_title", "hermite_poem", "hermite_effect", 3);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateHermiteConstellation());

            cardTemplate.CardBehaviors.Add(new HermitePassiveBehavior(-1));
            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("rock", "rock_title", "rock_poem", "rock_effect", 0);
            cardTemplate.CanBeMoved = false;
            cardTemplate.CanBeValueModified = false;
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateRockConstellation());

            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("empress", "empress_title", "empress_poem", "empress_effect", 1);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateEmpressConstellation());

            cardTemplate.CardBehaviors.Add(new EmperorActiveBehavior(1));
            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("wheel", "wheel_title", "wheel_poem", "wheel_effect", 1);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateWheelConstellation());

            cardTemplate.CardBehaviors.Add(new WheelActiveBehavior());
            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("tower", "tower_title", "tower_poem", "tower_effect", 1);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateTowerConstellation());

            cardTemplate.CardBehaviors.Add(new HierophantActiveBehavior(new List<string> { "rock" }));
            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("judgement", "judgement_title", "judgement_poem", "judgement_effect", 1);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateJudgementConstellation());

            cardTemplate.CardBehaviors.Add(new JudgementActiveBehavior());
            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("sun", "sun_title", "sun_poem", "sun_effect", 1);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateSunConstellation());

            cardTemplate.CardBehaviors.Add(new SunPassiveBehavior(1));
            this.AddCardTemplateToLibrary(cardTemplate);

            cardTemplate = new CardTemplate("star", "star_title", "star_poem", "star_effect", 0);
            cardTemplate.Patterns.Add(ConstellationPatternFactory.CreateStarConstellation());

            cardTemplate.CardBehaviors.Add(new AddValueToSelfBehavior(3));
            this.AddCardTemplateToLibrary(cardTemplate);
        }

        private void AddCardTemplateToLibrary(CardTemplate cardTemplate)
        {
            this.cardLibrary.Add(cardTemplate.Name, cardTemplate);
        }

        // Handling methods

        //private void DefaultAwakenedFunction(Card card, BoardGameLayer layer)
        //{
        //    //this.ApplyBonusValue(card, true);
        //}

        //private void DefaultUnAwakenedFunction(Card card, BoardGameLayer layer)
        //{
        //    //this.ApplyBonusValue(card, false);
        //}

        //private void ApplyBonusValue(Card card, bool isApplied)
        //{
        //    if (isApplied)
        //    {
        //        card.ValueModifier += card.BonusValue;
        //    }
        //    else
        //    {
        //        card.ValueModifier -= card.BonusValue;
        //    }
        //}
    }
}
