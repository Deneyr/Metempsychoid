﻿using Astrategia.Animation;
using Astrategia.Model.Player;
using Astrategia.View.Animation;
using Astrategia.View.Text2D;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrategia.View.Layer2D.BoardBannerLayer2D
{
    public class HeaderEntity2D : TextCanevas2D
    {
        private Dictionary<string, int> playerNameToIndex;

        public override bool IsActive
        {
            get
            {
                return base.IsActive && this.SpriteColor.A > 0;
            }
        }

        public HeaderEntity2D(ALayer2D parentLayer, Player player, Player opponent) : base(parentLayer)
        {
            this.CreateTextParagraph2D(new Vector2f(0, 0), new Vector2f(0, 0), TextParagraph2D.Alignment.CENTER, 60);
            this.CreateTextParagraph2D(new Vector2f(0, 0), new Vector2f(0, 0), TextParagraph2D.Alignment.CENTER, 60);
            this.CreateTextParagraph2D(new Vector2f(0, 0), new Vector2f(0, 0), TextParagraph2D.Alignment.CENTER, 60);
            this.CreateTextParagraph2D(new Vector2f(0, 0), new Vector2f(0, 0), TextParagraph2D.Alignment.CENTER, 60);

            this.Canevas = new IntRect(0, 0, 2000, 500);
            this.Position = new Vector2f(-1000, -50);

            this.playerNameToIndex = new Dictionary<string, int>();

            this.UpdateTextOfParagraph(0, "start_player_turn", player.PlayerName);
            this.UpdateTextOfParagraph(1, "end_player_turn", player.PlayerName);
            this.textParagraph2Ds[0].UpdateParameterColor(0, player.PlayerColor);
            this.textParagraph2Ds[1].UpdateParameterColor(0, player.PlayerColor);
            this.playerNameToIndex.Add(player.PlayerName, 0);

            this.UpdateTextOfParagraph(2, "start_opponent_turn", opponent.PlayerName);
            this.UpdateTextOfParagraph(3, "end_opponent_turn", opponent.PlayerName);
            this.textParagraph2Ds[2].UpdateParameterColor(0, opponent.PlayerColor);
            this.textParagraph2Ds[3].UpdateParameterColor(0, opponent.PlayerColor);
            this.playerNameToIndex.Add(opponent.PlayerName, 2);

            SequenceAnimation sequence = new SequenceAnimation(Time.FromSeconds(6), AnimationType.ONETIME);

            IAnimation anim = new ColorAnimation(new Color(0, 0, 0, 0), new Color(0, 0, 0, 0), Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.SQUARE_DEC);
            sequence.AddAnimation(0, anim);

            anim = new ColorAnimation(new Color(0, 0, 0, 0), new Color(0, 0, 0, 255), Time.FromSeconds(2), AnimationType.ONETIME, InterpolationMethod.SQUARE_DEC);
            sequence.AddAnimation(2, anim);

            anim = new ColorAnimation(new Color(0, 0, 0, 255), new Color(0, 0, 0, 0), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            sequence.AddAnimation(5, anim);
            this.animationsList.Add(sequence);

            sequence = new SequenceAnimation(Time.FromSeconds(4), AnimationType.ONETIME);
            anim = new ColorAnimation(new Color(0, 0, 0, 0), new Color(0, 0, 0, 255), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_DEC);
            sequence.AddAnimation(0, anim);

            anim = new ColorAnimation(new Color(0, 0, 0, 255), new Color(0, 0, 0, 0), Time.FromSeconds(1), AnimationType.ONETIME, InterpolationMethod.SQUARE_ACC);
            sequence.AddAnimation(3, anim);
            this.animationsList.Add(sequence);

            this.IsActive = false;
        }

        public void DisplayHeader(string playerName, int indexParagraph, int indexAnimation)
        {
            this.ActiveOnlyParagraph(this.playerNameToIndex[playerName] + indexParagraph);

            this.IsActive = true;
            this.PlayAnimation(indexAnimation);
        }
    }
}
