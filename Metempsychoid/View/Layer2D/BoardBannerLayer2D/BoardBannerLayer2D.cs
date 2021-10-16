using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metempsychoid.Model;
using Metempsychoid.Model.Card;
using Metempsychoid.Model.Layer.BoardBannerLayer;
using Metempsychoid.Model.Layer.BoardGameLayer;
using Metempsychoid.Model.Node.TestWorld;
using Metempsychoid.Model.Player;
using Metempsychoid.View.Card2D;
using Metempsychoid.View.Layer2D.BoardGameLayer2D;
using SFML.Graphics;
using SFML.System;

namespace Metempsychoid.View.Layer2D.BoardBannerLayer2D
{
    public class BoardBannerLayer2D : ALayer2D
    {
        private BannerEntity2D bannerEntity2D;

        private HeaderEntity2D headerEntity2D;

        private ScoreDomainLabel2D scoreDomainLabel2D;

        private CardToolTip2D cardToolTip;
        private DomainToolTip2D domainToolTip;
        private ALayer2D domainLayerFocused;

        private TurnBanner2D turnBanner2D;

        private EndLevelBanner2D endLevelBanner2D;

        private ReturnMenuButton2D returnMenuButton2D;
        private ReplayMenuButton2D replayMenuButton2D;

        private TurnPhase levelTurnPhase;

        private List<AEntity2D> hittableEntities2D;

        private HashSet<ICardFocusedLayer> cardFocusedLayers;
        private HashSet<IDomainsLayer> domainsLayers;
        private Dictionary<string, IScoreLayer> scoreLayers;

        private CardEntity2D cardFocused;

        private bool endDomainsEvaluated;

        public TurnPhase LevelTurnPhase
        {
            get
            {
                return this.levelTurnPhase;
            }
            private set
            {
                if (this.levelTurnPhase != value)
                {
                    this.levelTurnPhase = value;

                    switch (this.levelTurnPhase)
                    {
                        case TurnPhase.START_TURN:
                            this.InitializeStartTurnPhase();
                            break;
                        case TurnPhase.END_TURN:
                            this.InitializeEndTurnPhase();
                            break;
                        case TurnPhase.COUNT_POINTS:
                            this.endDomainsEvaluated = false;
                            break;
                        case TurnPhase.END_LEVEL:

                            BoardBannerLayer boardBannerLayer = this.parentLayer as BoardBannerLayer;

                            this.endLevelBanner2D.DisplayEndLevelBanner(boardBannerLayer.PlayerNameToTotalScores, boardBannerLayer.Player, boardBannerLayer.Opponent);
                            break;
                    }
                }
            }
        }

        public BoardBannerLayer2D(World2D world2D, IObject2DFactory factory, BoardBannerLayer layer) :
            base(world2D, factory, layer)
        {
            this.Area = new Vector2i(int.MaxValue, int.MaxValue);

            layer.PlayerScoreUpdated += OnPlayerScoreUpdated;
            layer.TurnCountChanged += OnTurnCountChanged;  

            this.bannerEntity2D = new BannerEntity2D(this);

            this.endLevelBanner2D = new EndLevelBanner2D(this);
            this.returnMenuButton2D = new ReturnMenuButton2D(this);
            this.returnMenuButton2D.Position = new Vector2f(-200, this.endLevelBanner2D.Canevas.Height / 2 - this.returnMenuButton2D.Canevas.Height / 2 - 20);
            this.replayMenuButton2D = new ReplayMenuButton2D(this);
            this.replayMenuButton2D.Position = new Vector2f(200, this.endLevelBanner2D.Canevas.Height / 2 - this.returnMenuButton2D.Canevas.Height / 2 - 20);

            this.hittableEntities2D = new List<AEntity2D>();

            this.turnBanner2D = new TurnBanner2D(this);

            this.domainToolTip = new DomainToolTip2D(this);

            this.cardFocusedLayers = new HashSet<ICardFocusedLayer>();
            this.domainsLayers = new HashSet<IDomainsLayer>();
            this.scoreLayers = new Dictionary<string, IScoreLayer>();
        }

        public override void InitializeLayer(IObject2DFactory factory)
        {
            base.InitializeLayer(factory);

            this.LevelTurnPhase = TurnPhase.VOID;         

            BoardBannerLayer boardBannerLayer = this.parentLayer as BoardBannerLayer;
            this.headerEntity2D = new HeaderEntity2D(this, boardBannerLayer.Player, boardBannerLayer.Opponent);
            this.scoreDomainLabel2D = new ScoreDomainLabel2D(this, boardBannerLayer.Player, boardBannerLayer.Opponent);

            this.turnBanner2D.ResetTurn(boardBannerLayer.MaxTurnCount);

            this.domainToolTip.HideToolTip();

            this.cardFocused = null;
            this.domainLayerFocused = null;

            this.cardFocusedLayers.Clear();
            this.domainsLayers.Clear();
            this.scoreLayers.Clear();
            if (this.world2D.TryGetTarget(out World2D world2D))
            {
                foreach(ALayer2D layer in world2D.LayersList)
                {
                    ICardFocusedLayer cardFocusedLayer = layer as ICardFocusedLayer;

                    if(cardFocusedLayer != null)
                    {
                        this.cardFocusedLayers.Add(cardFocusedLayer);
                    }

                    IDomainsLayer domainLayer = layer as IDomainsLayer;
                    if (domainLayer != null)
                    {
                        this.domainsLayers.Add(domainLayer);

                        domainLayer.StartDomainEvaluated += OnStartDomainsEvaluated;
                        domainLayer.DomainEvaluated += OnDomainEvaluated;
                        domainLayer.EndDomainEvaluated += OnEndDomainsEvaluated;

                        domainLayer.DomainFocusedChanged += OnDomainFocusedChanged;
                    }

                    IScoreLayer scoreLayer = layer as IScoreLayer;
                    if(scoreLayer != null)
                    {
                        this.scoreLayers.Add(scoreLayer.PlayerName, scoreLayer);
                    }
                }
            }
        }

        private void OnTurnCountChanged(int arg1, int arg2)
        {
            if (this.turnBanner2D != null)
            {
                this.turnBanner2D.UpdateTurnCount(arg1, arg2);
            }
        }

        private void OnPlayerScoreUpdated(string playerName, int scoreUpdated)
        {
            this.scoreLayers[playerName].PlayerScore = scoreUpdated;
        }

        private void OnStartDomainsEvaluated()
        {
            this.bannerEntity2D.IsActive = true;
            this.bannerEntity2D.PlayAnimation(2);
        }

        private void OnEndDomainsEvaluated()
        {
            if (this.scoreDomainLabel2D.IsActive)
            {
                this.bannerEntity2D.PlayAnimation(3);
                this.scoreDomainLabel2D.IsActive = false;
            }

            BoardBannerLayer parentLayer = this.parentLayer as BoardBannerLayer;
            foreach (KeyValuePair<string, IScoreLayer> scoreLayerPairValue in this.scoreLayers)
            {
                if(scoreLayerPairValue.Value.PlayerScore != parentLayer.PlayerNameToTotalScores[scoreLayerPairValue.Key].Last())
                {
                    throw new Exception("The score at the end of a game turn is not the same as the one computed by the view");
                }
            }

            this.endDomainsEvaluated = true;
        }

        private void OnDomainEvaluated(Model.Layer.BoardGameLayer.CJStarDomain domain)
        {
            string domainOwnerName = null;
            if (domain.DomainOwner != null)
            {
                domainOwnerName = domain.DomainOwner.PlayerName;
            }

            this.scoreDomainLabel2D.DisplayScoreDomainLabel(domainOwnerName, domain.TemporaryDomainOwner != null);

            foreach (KeyValuePair<Player, int> keyValuePair in domain.PlayerToPoints)
            {
                this.scoreDomainLabel2D.UpdateScoreDomainLabel(keyValuePair.Key.PlayerName, keyValuePair.Value);
            }
        }

        public void OnDomainEvaluationFinished(string domainOwnerName)
        {
            if (domainOwnerName != null)
            {
                this.scoreLayers[domainOwnerName].PlayerScore++;
            }
            else
            {
                foreach (IScoreLayer scoreLayer in this.scoreLayers.Values)
                {
                    scoreLayer.PlayerScore++;
                }
            }
        }

        private void OnDomainFocusedChanged(IDomainsLayer obj)
        {
            this.domainLayerFocused = obj as ALayer2D;

            if (obj.DomainFocused != null)
            {
                CJStarDomain domainFocused = (obj as ALayer2D).GetEntityFromEntity2D(obj.DomainFocused) as CJStarDomain;
                BoardBannerLayer parentBannerLayer = this.parentLayer as BoardBannerLayer;

                this.domainToolTip.DisplayToolTip(domainFocused, parentBannerLayer.Player, parentBannerLayer.Opponent);
            }
            else
            {
                this.domainToolTip.HideToolTip();
            }
        }

        protected override Vector2f DefaultViewSize
        {
            set
            {
                if (value != this.DefaultViewSize)
                {
                    base.DefaultViewSize = value;

                    //IntRect endTurnButtonCanvevas = this.endTurnButton.Canevas;
                    IntRect turnBannerCanevas = this.turnBanner2D.Canevas;

                    //this.endTurnButton.Position = new Vector2f(-endTurnButtonCanvevas.Width / 2, this.DefaultViewSize.Y / 2 - endTurnButtonCanvevas.Height);
                    this.turnBanner2D.Position = new Vector2f(- this.DefaultViewSize.X / 2 + turnBannerCanevas.Width / 2, -this.DefaultViewSize.Y / 2 + turnBannerCanevas.Height / 2);
                }
            }
        }

        protected override AEntity2D AddEntity(AEntity obj)
        {
            AEntity2D entityToAdd = base.AddEntity(obj);

            if(entityToAdd is CardToolTip2D)
            {
                this.cardToolTip = entityToAdd as CardToolTip2D;
            }

            return entityToAdd;
        }

        protected override void OnLevelStateChanged(string obj)
        {
            this.LevelTurnPhase = (TurnPhase)Enum.Parse(typeof(TurnPhase), obj.ToString());
        }

        public void ChangeLevel(string levelId)
        {
            this.endLevelBanner2D.HideEndLevelBanner();
            this.returnMenuButton2D.DeactiveButton();
            this.replayMenuButton2D.DeactiveButton();

            this.SendEventToWorld(Model.Event.EventType.LEVEL_CHANGE, null, levelId);
        }

        public override void UpdateGraphics(Time deltaTime)
        {
            base.UpdateGraphics(deltaTime);

            this.UpdateFocusedEntity(deltaTime);

            switch (this.LevelTurnPhase)
            {
                case TurnPhase.START_TURN:
                    this.UpdateStartTurnPhase(deltaTime);
                    break;
                case TurnPhase.END_TURN:
                    this.UpdateEndTurnPhase(deltaTime);
                    break;
                case TurnPhase.COUNT_POINTS:
                    this.UpdateCountPointsPhase(deltaTime);
                    break;
                case TurnPhase.END_LEVEL:
                    this.endLevelBanner2D.UpdateGraphics(deltaTime);
                    if(this.endLevelBanner2D.State == EndLevelBanner2D.BannerState.END
                        && this.returnMenuButton2D.IsActive == false)
                    {
                        this.returnMenuButton2D.ActiveButton();
                        this.replayMenuButton2D.ActiveButton();
                    }

                    break;
            }
        }

        protected override IEnumerable<AEntity2D> GetEntities2DFocusable()
        {
            this.hittableEntities2D.Clear();

            this.hittableEntities2D.Add(this.returnMenuButton2D);
            this.hittableEntities2D.Add(this.replayMenuButton2D);

            return this.hittableEntities2D;
        }

        private void UpdateFocusedEntity(Time deltaTime)
        {
            ICardFocusedLayer firstCardLayerFocused = this.cardFocusedLayers.FirstOrDefault(pElem => pElem.CardFocused != null);
            ALayer2D layer2D = firstCardLayerFocused as ALayer2D;

            if (firstCardLayerFocused != null)
            {
                CardEntity2D cardFocused = firstCardLayerFocused.CardFocused;
                if (this.cardFocused != cardFocused)
                {
                    CardEntity cardEntity = layer2D.GetEntityFromEntity2D(cardFocused) as CardEntity;

                    this.cardToolTip.DisplayToolTip(cardEntity.Card, cardFocused);

                    this.cardFocused = cardFocused;
                }

                if (this.cardToolTip.IsActive)
                {
                    Vector2f cardFocusedPosition = this.cardFocused.Position;

                    Vector2f cardPositionInWindow = layer2D.GetPositionInWindow(cardFocusedPosition);

                    if (cardPositionInWindow.Y > this.defaultViewSize.Y / 2)
                    {
                        cardFocusedPosition.Y -= this.cardFocused.Canevas.Height / 2;

                        cardFocusedPosition = this.GetPositionInScene(layer2D.GetPositionInWindow(cardFocusedPosition));

                        cardFocusedPosition.X -= this.cardToolTip.Canevas.Width / 2;
                        cardFocusedPosition.Y -= this.cardToolTip.Canevas.Height;
                    }
                    else
                    {
                        cardFocusedPosition.Y += this.cardFocused.Canevas.Height / 2;

                        cardFocusedPosition = this.GetPositionInScene(layer2D.GetPositionInWindow(cardFocusedPosition));

                        cardFocusedPosition.X -= this.cardToolTip.Canevas.Width / 2;
                    }

                    this.cardToolTip.Position = cardFocusedPosition;
                }
            }
            else
            {
                this.cardToolTip.HideToolTip();

                this.cardFocused = null;
            }

            if (this.domainToolTip.IsActive)
            {
                Vector2f mousePositionInWindow = this.domainLayerFocused.GetPositionInWindow(new Vector2f(this.domainLayerFocused.MousePosition.X, this.domainLayerFocused.MousePosition.Y));
                Vector2f domainTooltipPosition = this.GetPositionInScene(mousePositionInWindow);

                if (mousePositionInWindow.Y > this.defaultViewSize.Y / 2)
                {
                    domainTooltipPosition.Y -= this.domainToolTip.Canevas.Height;
                }
                //domainTooltipPosition.X -= this.domainToolTip.Canevas.Width / 2;

                this.domainToolTip.Position = domainTooltipPosition;

                this.domainToolTip.UpdateGraphics(deltaTime);
            }
        }

        public override void DrawIn(RenderWindow window, Time deltaTime)
        {
            base.DrawIn(window, deltaTime);

            SFML.Graphics.View defaultView = window.DefaultView;
            window.SetView(this.view);

            this.domainToolTip.DrawIn(window, deltaTime);

            this.turnBanner2D.DrawIn(window, deltaTime);

            this.bannerEntity2D.DrawIn(window, deltaTime);

            this.headerEntity2D.DrawIn(window, deltaTime);
            this.scoreDomainLabel2D.DrawIn(window, deltaTime);

            this.endLevelBanner2D.DrawIn(window, deltaTime);
            this.returnMenuButton2D.DrawIn(window, deltaTime);
            this.replayMenuButton2D.DrawIn(window, deltaTime);

            window.SetView(defaultView);
        }

        private void InitializeStartTurnPhase()
        {
            this.bannerEntity2D.IsActive = true;
            this.headerEntity2D.IsActive = true;
            this.bannerEntity2D.SpriteColor = new Color(0, 0, 0, 128);
            this.bannerEntity2D.Position = new Vector2f(-this.bannerEntity2D.ObjectSprite.TextureRect.Width, 0);
            this.bannerEntity2D.PlayAnimation(0);

            this.headerEntity2D.DisplayHeader((this.parentLayer as BoardBannerLayer).PlayerTurn.PlayerName, 0, 0);
        }

        private void InitializeEndTurnPhase()
        {
            this.bannerEntity2D.IsActive = true;
            this.headerEntity2D.IsActive = true;
            this.bannerEntity2D.PlayAnimation(1);

            this.headerEntity2D.DisplayHeader((this.parentLayer as BoardBannerLayer).PlayerTurn.PlayerName, 1, 1);
        }

        private void UpdateStartTurnPhase(Time deltaTime)
        {
            if (this.bannerEntity2D.IsAnimationRunning() == false)
            {
                this.bannerEntity2D.IsActive = false;
                this.headerEntity2D.IsActive = false;

                this.GoOnTurnPhase(TurnPhase.DRAW);
            }
        }

        private void UpdateEndTurnPhase(Time deltaTime)
        {
            if (this.bannerEntity2D.IsAnimationRunning() == false)
            {
                this.bannerEntity2D.IsActive = false;
                this.headerEntity2D.IsActive = false;

                this.GoOnTurnPhase(TurnPhase.START_TURN);
            }
        }

        private void UpdateCountPointsPhase(Time deltaTime)
        {
            this.scoreDomainLabel2D.UpdateGraphics(deltaTime);

            if (this.endDomainsEvaluated && this.bannerEntity2D.IsAnimationRunning() == false)
            {
                this.bannerEntity2D.IsActive = false;

                this.GoOnTurnPhase(TurnPhase.START_TURN);
            }
        }

        private void GoOnTurnPhase(TurnPhase nextTurnPhase)
        {
            this.SendEventToWorld(Model.Event.EventType.LEVEL_PHASE_CHANGE, null, Enum.GetName(typeof(TurnPhase), nextTurnPhase));
        }

        public override void FlushEntities()
        {
            base.FlushEntities();

            this.LevelTurnPhase = TurnPhase.VOID;

            if (this.bannerEntity2D != null)
            {
                this.bannerEntity2D.IsActive = false;
                this.bannerEntity2D.Dispose();
            }

            if (this.headerEntity2D != null)
            {
                this.headerEntity2D.IsActive = false;
                this.headerEntity2D.Dispose();
            }

            foreach (IDomainsLayer domainLayer in this.domainsLayers)
            {
                domainLayer.StartDomainEvaluated -= this.OnStartDomainsEvaluated;
                domainLayer.DomainEvaluated -= OnDomainEvaluated;
                domainLayer.EndDomainEvaluated -= OnEndDomainsEvaluated;

                domainLayer.DomainFocusedChanged -= OnDomainFocusedChanged;
            }

            if (this.scoreDomainLabel2D != null)
            {
                this.scoreDomainLabel2D.Dispose();
            }

            this.domainsLayers.Clear();

            this.scoreLayers.Clear();

            this.cardFocusedLayers.Clear();
        }

        public override void Dispose()
        {
            (this.parentLayer as BoardBannerLayer).PlayerScoreUpdated -= OnPlayerScoreUpdated;
            (this.parentLayer as BoardBannerLayer).TurnCountChanged -= OnTurnCountChanged;

            base.Dispose();
        }
    }
}
