using Metempsychoid.Model.Node;
using Metempsychoid.Model.Node.TestWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Layer.BoardBannerLayer
{
    public class BoardBannerLayer: EntityLayer.EntityLayer
    {
        public Dictionary<string, int> PlayerNameToModifier
        {
            get;
            private set;
        }

        public Dictionary<string, List<int>> PlayerNameToTotalScores
        {
            get;
            private set;
        }

        public Player.Player Player
        {
            get;
            private set;
        }

        public Player.Player Opponent
        {
            get;
            private set;
        }

        public Player.Player PlayerTurn
        {
            get;
            set;
        }

        public int TurnIndex
        {
            get;
            set;
        }

        public ToolTipEntity ToolTip
        {
            get;
            private set;
        }

        public event Action<string, int> PlayerScoreUpdated;

        public void AddVictoryPointsTo(string playerName, int nbVictoryPointsToAdd)
        {
            this.PlayerNameToModifier[playerName] += nbVictoryPointsToAdd;

            int playerTotalScore = 0;
            if (this.PlayerNameToTotalScores.TryGetValue(playerName, out List<int> scoresList) && scoresList.Count > 0)
            {
                playerTotalScore = scoresList.Last();
            }

            this.PlayerScoreUpdated?.Invoke(playerName, this.PlayerNameToModifier[playerName] + playerTotalScore);
        }

        public void ClearModifiers()
        {
            this.PlayerNameToModifier = new Dictionary<string, int>();
            this.PlayerNameToModifier.Add(this.Player.PlayerName, 0);
            this.PlayerNameToModifier.Add(this.Opponent.PlayerName, 0);
        }

        protected override void InternalInitializeLayer(World world, ALevelNode levelNode)
        {
            this.Player = world.Player;
            this.Opponent = (levelNode as CardBoardLevel).Opponent;

            this.PlayerTurn = this.Player;
            this.TurnIndex = 0;

            this.ToolTip = new ToolTipEntity(this);
            this.AddEntityToLayer(this.ToolTip);

            this.PlayerNameToTotalScores = new Dictionary<string, List<int>>();
            this.PlayerNameToTotalScores.Add(this.Player.PlayerName, new List<int>());
            this.PlayerNameToTotalScores.Add(this.Opponent.PlayerName, new List<int>());

            this.ClearModifiers();
        }
    }
}
