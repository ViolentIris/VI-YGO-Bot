using YGOSharp.OCGWrapper.Enums;
using System.Collections.Generic;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    [Deck("Fairy", "AI_Fairy")]
    public class FairyExecutor : DefaultExecutor
    {
        public class CardId
        {
            public const int Paladin1 = 16261341;
            public const int Ariadne = 98301564;
            public const int Artemis = 32296881;
            public const int Meltiel = 49905576;
			public const int Honest = 91907707;
            public const int Statue = 46145256;

            public const int CardOfDemise = 59750328;
			public const int Parshath1 = 15449853;
			public const int Ties = 40450317;
			public const int Solidarity = 86780027;

            public const int SolemnWarning = 84749824;
            public const int SolemnStrike = 40605147;
            public const int SolemnJudgment = 41420027;
			public const int Match = 53334471;
			public const int AntiSpellFragrance = 58921041;
			public const int Warlords = 90846359;
			public const int Horn = 1637760;
			public const int Parshath2 = 42444868;
			public const int Powerless = 14315573;
			
			public const int Paladin2 = 69514125;
			public const int Paladin3 = 48589580;
			
			public const int Sanctuary = 56433456;
        }

        public FairyExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            AddExecutor(ExecutorType.Activate, CardId.Ariadne, AriadneScaleActivate);
			AddExecutor(ExecutorType.Activate, CardId.Solidarity);

            AddExecutor(ExecutorType.Summon, CardId.Artemis);
			AddExecutor(ExecutorType.Summon, CardId.Meltiel);
			AddExecutor(ExecutorType.Summon, CardId.Statue);

            AddExecutor(ExecutorType.Activate, CardId.Artemis);
            AddExecutor(ExecutorType.Activate, CardId.Meltiel);
			AddExecutor(ExecutorType.Activate, CardId.Honest, DefaultHonestEffect);
            AddExecutor(ExecutorType.Activate, CardId.Ariadne, AriadneEffect);
			AddExecutor(ExecutorType.Activate, CardId.Paladin1, Paladin1Effect1);
			AddExecutor(ExecutorType.Activate, CardId.Paladin1, Paladin1Effect2);

            AddExecutor(ExecutorType.Activate, CardId.CardOfDemise, CardOfDemiseeff);
			AddExecutor(ExecutorType.Activate, CardId.Parshath1, Parshath1eff);
			AddExecutor(ExecutorType.Activate, CardId.Ties);
			
			AddExecutor(ExecutorType.Activate, CardId.Powerless, DefaultUniqueTrap);
			AddExecutor(ExecutorType.Activate, CardId.SolemnStrike, DefaultSolemnStrike);
			AddExecutor(ExecutorType.Activate, CardId.SolemnWarning, DefaultSolemnWarning);
			AddExecutor(ExecutorType.Activate, CardId.SolemnJudgment, DefaultSolemnJudgment);
			AddExecutor(ExecutorType.Activate, CardId.Match, DefaultUniqueTrap);
			AddExecutor(ExecutorType.Activate, CardId.AntiSpellFragrance, DefaultUniqueTrap);
			AddExecutor(ExecutorType.Activate, CardId.Horn, DefaultUniqueTrap);
			AddExecutor(ExecutorType.Activate, CardId.Parshath2, Parshath2Effect);
			AddExecutor(ExecutorType.Activate, CardId.Warlords, DefaultUniqueTrap);
			
			AddExecutor(ExecutorType.Activate, CardId.Paladin2);
			AddExecutor(ExecutorType.Activate, CardId.Paladin3, Paladin3Effect);
			
            AddExecutor(ExecutorType.Repos, DefaultMonsterRepos);
			AddExecutor(ExecutorType.SpellSet, SpellSet);
        }

        public override bool OnSelectHand()
        {
            return true;
        }

		private bool SpellSet()
        {
            if (Card.IsCode(CardId.Warlords) && Bot.HasInSpellZone(CardId.Warlords)) return false;
            if (Card.IsCode(CardId.Match) && Bot.HasInSpellZone(CardId.Match)) return false;
            if (Card.IsCode(CardId.AntiSpellFragrance) && Bot.HasInSpellZone(CardId.AntiSpellFragrance)) return false;
            if (Card.HasType(CardType.Trap))
                return Bot.GetSpellCountWithoutField() < 4;
            return false;
        }
        private bool CardOfDemiseeff()
        {
            if (Bot.HasInHand(CardId.Honest)) return false;
			return true;
        }
		private bool AriadneScaleActivate()
        {
            if (Bot.HasInSpellZone(CardId.Ariadne)) return false;
            return true;
        }
		private bool Parshath1eff()
        {
            if (Bot.HasInSpellZone(CardId.Parshath1)) return false;
            return true;
        }
		private bool Paladin1Effect1()
        {           
            foreach (ClientCard e_c in Bot.Graveyard)
                    AI.SelectCard(CardId.Honest, CardId.Statue, CardId.Ariadne, CardId.Artemis, CardId.Meltiel);
					AI.SelectNextCard(CardId.Honest, CardId.Statue, CardId.Ariadne, CardId.Artemis, CardId.Meltiel);
					AI.SelectPosition(CardPosition.FaceUpAttack);
            return true;
        }
		private bool Paladin1Effect2()
        {
			AI.SelectCard(CardId.Parshath2, CardId.SolemnStrike, CardId.SolemnWarning, CardId.SolemnJudgment, CardId.Horn, CardId.Parshath1);
			return true;
        }
		private bool AriadneEffect()
        {
			AI.SelectCard(CardId.Parshath2, CardId.SolemnStrike, CardId.SolemnWarning, CardId.SolemnJudgment, CardId.Horn, CardId.Powerless);
			return true;
        }
		private bool Paladin3Effect()
        {
		if (Bot.HasInSpellZone(CardId.Sanctuary))
		{
			AI.SelectCard(CardId.Honest, CardId.Parshath1, CardId.Ariadne, CardId.Artemis, CardId.Meltiel);
			return true;
		} else {
			AI.SelectCard(CardId.Parshath1, CardId.Ariadne, CardId.Artemis, CardId.Meltiel);
			return true;
		}
            return false;
        }
		private bool Parshath2Effect()
        {
            return !(Duel.Player == 0 && Duel.LastChainPlayer == -1);
			
        }
		public override bool OnSelectYesNo(int desc)
        {
            if (desc == Util.GetStringId(CardId.Parshath2, 0))
                return true;
            return base.OnSelectYesNo(desc);
			AI.SelectCard(CardId.Paladin1, CardId.Paladin2, CardId.Paladin3);
			return true;
        }
    }
}