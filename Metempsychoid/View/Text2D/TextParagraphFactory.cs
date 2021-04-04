using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.View.Text2D
{
    public class TextParagraphFactory
    {
        private Dictionary<string, List<TextToken2D>> idToTokens;

        private string culture;

        public string Culture
        {
            get
            {
                return this.culture;
            }
            set
            {
                if(this.culture != value)
                {
                    this.culture = value;

                    this.UpdateTextTokensFromCulture();
                }
            }
        }

        public TextParagraphFactory()
        {
            this.idToTokens = new Dictionary<string, List<TextToken2D>>();

            this.culture = null;
        }

        public void CreateTextTokensIn(TextParagraph2D paragraph2D, string id)
        {
            paragraph2D.UpdateTextTokens(this.idToTokens[id]);
        }

        private void UpdateTextTokensFromCulture()
        {
            this.idToTokens.Clear();

            // Replace by searching in the localization file.
            string text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

            List<TextToken2D> token2DsList = this.CreateTextTokens(text);

            this.idToTokens.Add("test", token2DsList);
        }

        private List<TextToken2D> CreateTextTokens(string text)
        {
            List<TextToken2D> tokens = new List<TextToken2D>();

            string[] textTokens = text.Split(' ');

            foreach (string textToken in textTokens)
            {
                // Replace by ctr choice.
                TextToken2D textToken2D = new TextToken2D(textToken);

                tokens.Add(textToken2D);
            }

            return tokens;
        }
    }
}
