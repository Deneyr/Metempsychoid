using Metempsychoid.Model.Card;
using Metempsychoid.Model.Event;
using Metempsychoid.Model.Layer.BoardBannerLayer;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Layer.BoardNotifLayer;
using Metempsychoid.Model.Layer.BoardPlayerLayer;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metempsychoid.Model.Node.TestWorld
{
    public class AstraMenuLevel : CardBoardLevel
    {
        public AstraMenuLevel(World world) : base(world)
        {
        }

        public override void VisitStart(World world)
        {
            this.NodeState = NodeState.ACTIVE;
            this.pendingGameEvents = new Dictionary<EventType, List<GameEvent>>();

            Player.Player player = new Player.Player(Color.Green, "startPlayer");
            player.Deck.CardIds.Add("META_beginning");
            player.Deck.CardIds.Add("META_rules");
            player.Deck.CardIds.Add("META_architects");

            Player.Player opponent = new Player.Player(Color.Red, "startOppPlayer");

            this.MainPlayer = player;
            this.Opponent = opponent;

            this.playerIndex = 0;

            this.BoardBannerLayer = world.LoadedLayers["bannerLayer"] as BoardBannerLayer;
            this.BoardBannerLayer.PreInitMaxTurnCount = 1;

            this.BoardGameLayer = world.LoadedLayers["gameLayer"] as BoardGameLayer;
            this.BoardGameLayer.PreInitGalaxyName = "Menu";

            world.InitializeLevel(new List<string>()
            {
                "VsO7nJK",
                "menuTextLayer",
                "gameLayer",
                "menuPlayerLayer",
                //"opponentLayer",
                "menuNotifLayer"
                //"bannerLayer"
            }, this);

            this.BoardPlayersList.Clear();
            this.BoardPlayersList.Add(world.LoadedLayers["menuPlayerLayer"] as BoardPlayerLayer);
            //this.BoardplayersList.Add(world.LoadedLayers["opponentLayer"] as BoardPlayerLayer);

            this.BoardNotifLayer = world.LoadedLayers["menuNotifLayer"] as BoardNotifLayer;

            this.InitializeStartLevelPhase(world);
        }

        protected override void FowardFocusPileEvent(World world)
        {
            // Nothing to do.
        }

        protected override void UpdateCreateHandPhase(World world)
        {
            BoardPlayerLayer boardPlayerLayer = this.BoardPlayersList[0];

            if (this.CheckDrawCardEvent(world, boardPlayerLayer))
            {
                boardPlayerLayer.DrawCard();
            }

            //if (this.CheckDrawCardEvent(world, boardOpponentLayer))
            //{
            //    boardOpponentLayer.DrawCard(false);
            //}

            if (this.CheckNextTurnPhaseEvent(TurnPhase.START_TURN, null))
            {
                this.InitializeMainPhase(world);
            }
        }

        protected override void InitializeMainPhase(World world)
        {
            this.TurnIndex = 0;

            this.BoardBannerLayer.PlayerTurn = this.CurrentBoardPlayer.SupportedPlayer;
            this.BoardGameLayer.PlayerTurn = this.CurrentBoardPlayer.SupportedPlayer;

            this.SetCurrentTurnPhase(world, TurnPhase.MAIN);

            foreach (BoardPlayerLayer playerLayer in this.BoardPlayersList)
            {
                playerLayer.CardPileFocused = BoardPlayerLayer.PileFocused.HAND;
            }

            this.InitCardSocketed(world);
        }

        private void InitCardSocketed(World world)
        {
            StarEntity star;
            Card.Card card;

            star = this.BoardGameLayer.StarSystem.First(pElem => pElem.Name == "1");
            card = world.CardLibrary.CreateCard("rock", this.Opponent);
            this.BoardGameLayer.PickCard(card, false);
            this.BoardGameLayer.SocketCard(star);

            star = this.BoardGameLayer.StarSystem.First(pElem => pElem.Name == "2");
            card = world.CardLibrary.CreateCard("rock", this.Opponent);
            this.BoardGameLayer.PickCard(card, false);
            this.BoardGameLayer.SocketCard(star);

            star = this.BoardGameLayer.StarSystem.First(pElem => pElem.Name == "3");
            card = world.CardLibrary.CreateCard("rock", this.MainPlayer);
            this.BoardGameLayer.PickCard(card, false);
            this.BoardGameLayer.SocketCard(star);

            star = this.BoardGameLayer.StarSystem.First(pElem => pElem.Name == "4");
            card = world.CardLibrary.CreateCard("rock", this.MainPlayer);
            this.BoardGameLayer.PickCard(card, false);
            this.BoardGameLayer.SocketCard(star);

            star = this.BoardGameLayer.StarSystem.First(pElem => pElem.Name == "5");
            card = world.CardLibrary.CreateCard("moon", this.MainPlayer);
            this.BoardGameLayer.PickCard(card, false);
            this.BoardGameLayer.SocketCard(star);

            star = this.BoardGameLayer.StarSystem.First(pElem => pElem.Name == "6");
            card = world.CardLibrary.CreateCard("sun", this.Opponent);
            this.BoardGameLayer.PickCard(card, false);
            this.BoardGameLayer.SocketCard(star);
        }
    }
}
