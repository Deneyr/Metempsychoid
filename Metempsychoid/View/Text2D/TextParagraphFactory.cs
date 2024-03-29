﻿using Astrategia.View.Card2D;
using Astrategia.View.Layer2D.BoardBannerLayer2D;
using Astrategia.View.Layer2D.MenuTextLayer2D;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Astrategia.View.Text2D
{
    public class TextParagraphFactory
    {
        private Dictionary<string, List<TextToken2D>> idToTokens;

        private DirectoryInfo localizationFolder;

        private string culture;

        public Dictionary<string, string> IdToTexts
        {
            get;
            private set;
        }

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

            this.IdToTexts = new Dictionary<string, string>();

            this.localizationFolder = new DirectoryInfo(@"Assets\Localization");

            this.culture = null;
        }

        public void CreateTextTokensIn(TextParagraph2D paragraph2D, string id, params string[] parameters)
        {
            List<TextToken2D> textToken2Ds = this.CreateTextTokens(id);

            //if (parameters.Count() > 0)
            //{
            //    IEnumerable<TextToken2D> tokenParameters = textToken2Ds.Where(pElem => pElem.ParameterIndex >= 0);
            //    foreach (TextToken2D textToken2D in tokenParameters)
            //    {
            //        textToken2D.FullText = parameters[textToken2D.ParameterIndex];
            //    }
            //}

            paragraph2D.UpdateTextTokens(textToken2Ds, parameters);
        }

        public List<TextToken2D> CreateTextTokens(string id)
        {
            List<TextToken2D> textToken2Ds = this.idToTokens[id];
            textToken2Ds = textToken2Ds.Select(pElem => pElem.CloneToken()).ToList();

            return textToken2Ds;
        }

        private void UpdateTextTokensFromCulture()
        {
            this.idToTokens.Clear();
            this.IdToTexts.Clear();

            FileInfo[] locFiles = this.localizationFolder.GetFiles("*.loc");

            foreach(FileInfo locFile in locFiles)
            {
                using (Stream locStream = new FileStream(locFile.FullName, FileMode.Open))
                {
                    XDocument locDocument = XDocument.Load(locStream);

                    XElement rootElement = locDocument.Element("localization");

                    IEnumerable<XElement> paragraphEntries = rootElement.Elements("paragraphEntry");

                    foreach(XElement entry in paragraphEntries)
                    {
                        string key = entry.Element("key").Value;

                        XElement paragraph = entry.Elements("paragraph").FirstOrDefault(pElem => pElem.Attribute("culture").Value == this.Culture);
                        if(paragraph != null)
                        {
                            IEnumerable<XElement> paragraphElements = paragraph.Elements();

                            if (paragraphElements.Count() == 0)
                            {
                                string content = paragraph.Value;

                                //content = CleanSpecialCaracters(content);

                                List<TextToken2D> token2DsList = CreateTextTokens("Normal", content, Color.White);

                                this.idToTokens.Add(key, token2DsList);
                                this.IdToTexts.Add(key, content);
                            }
                            else
                            {
                                List<TextToken2D> token2DsList = new List<TextToken2D>();

                                StringBuilder fullContent = new StringBuilder();

                                int i = 0;
                                foreach (XElement paragraphElement in paragraphElements)
                                {
                                    string content = paragraphElement.Value;
                                    if(i != 0)
                                    {
                                        fullContent.Append(' ');
                                    }
                                    fullContent.Append(content.Trim());

                                    //content = CleanSpecialCaracters(content);

                                    XAttribute colorAttribut = paragraphElement.Attribute("color");
                                    Color tokenFillColor = Color.White;
                                    if (colorAttribut != null)
                                    {
                                        string[] colorTokens = colorAttribut.Value.Split(':');
                                        if(colorTokens.Count() >= 3)
                                        {
                                            tokenFillColor = new Color(byte.Parse(colorTokens[0]), byte.Parse(colorTokens[1]), byte.Parse(colorTokens[2]));
                                        }
                                    }

                                    this.AppendTextTokens(token2DsList, content, paragraphElement.Name.LocalName, tokenFillColor);
                                    i++;
                                }

                                this.idToTokens.Add(key, token2DsList);
                                this.IdToTexts.Add(key, fullContent.ToString());
                            }
                        }
                    }
                }
            }
        }

        private static string CleanSpecialCaracters(string str)
        {
            str = str.Replace("é", "e");
            str = str.Replace("è", "e");
            str = str.Replace("ê", "e");
            str = str.Replace("ë", "e");

            str = str.Replace("à", "a");
            str = str.Replace("â", "a");

            str = str.Replace("ô", "o");

            str = str.Replace("ù", "u");

            return str;
        }

        public static List<TextToken2D> CreateTextTokens(string tokenType, string text, Color fillColor)
        {
            List<TextToken2D> tokens = new List<TextToken2D>();

            string[] textTokens = text.Split(' ');

            foreach (string textToken in textTokens)
            {
                // Replace by ctr choice.
                TextToken2D textToken2D = CreateTextToken(tokenType, textToken, fillColor);

                tokens.Add(textToken2D);
            }

            return tokens;
        }

        public static TextToken2D CreateTextToken(string tokenType, string textToken, Color fillColor)
        {
            TextToken2D textToken2D = null;
            switch (tokenType)
            {
                case "Normal":
                    textToken2D = new TextToken2D(textToken, fillColor);
                    break;
                case "BannerTitle":
                    textToken2D = new TitleBannerTextToken2D(textToken, fillColor);
                    break;
                case "MenuTitle":
                    textToken2D = new TitleTextToken2D(textToken, fillColor);
                    break;
                case "CardLabel":
                    textToken2D = new CardLabelTextToken2D(textToken, fillColor);
                    break;
                default:
                    textToken2D = new TextToken2D(textToken, fillColor);
                    break;
            }

            return textToken2D;
        }

        public void AppendTextTokens(List<TextToken2D> tokenListToAppend, string text, string tokenType, Color fillColor)
        {
            string[] textTokens = text.Split(' ');

            foreach (string textToken in textTokens)
            {
                if (string.IsNullOrEmpty(textToken) == false)
                {
                    TextToken2D textToken2D = null;

                    if (textToken.StartsWith("{") && textToken.EndsWith("}") && int.TryParse(textToken.Substring(1, textToken.Length - 2), out int parameterIndex))
                    {
                        textToken2D = new ParameterTextToken2D(tokenType, fillColor);
                    }
                    else
                    {
                        textToken2D = CreateTextToken(tokenType, textToken, fillColor);
                    }

                    tokenListToAppend.Add(textToken2D);
                }
            }
        }
    }
}
