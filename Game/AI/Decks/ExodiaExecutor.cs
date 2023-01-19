using YGOSharp.OCGWrapper.Enums;
using System.Collections.Generic;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    [Deck("Exodia", "AI_Exodia")]
    public class ExodiaExecutor : DefaultExecutor
    {
        public class CardId
        {
            public const int CardcarD = 45812361;
            public const int BattleFader = 19665973;
			public const int Scarecrow = 19665973;
			public const int Lefthand = 7902349;
			public const int Righthand = 70903634;
			public const int Leftleg = 44519536;
			public const int Rightleg = 8124921;
			public const int Body = 33396948;

            public const int Peace = 33782437;
			public const int Greed = 55144522;
			public const int Upstart = 70368879;
			public const int Terraforming = 73628505;
			public const int Chicken = 67616300;
			public const int Hole = 76375976;

            public const int Waboku = 12607053;
            public const int ThreateningRoar = 36361633;
            public const int RecklessGreed = 37576645;
			public const int Escape = 80036543;

            public const int Linkuriboh = 41999284;
        }

        public ExodiaExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        {
            //first add
            AddExecutor(ExecutorType.Activate, CardId.Greed);
            //normal summon
            AddExecutor(ExecutorType.Summon, CardId.CardcarD);
            // Set traps
            AddExecutor(ExecutorType.SpellSet, CardId.Waboku);
            AddExecutor(ExecutorType.SpellSet, CardId.ThreateningRoar);
            AddExecutor(ExecutorType.SpellSet, CardId.RecklessGreed);
            AddExecutor(ExecutorType.SpellSet, CardId.Escape);
            //afer set
            AddExecutor(ExecutorType.Activate, CardId.CardcarD);
			//activate spell
			AddExecutor(ExecutorType.Activate, CardId.Peace);
            AddExecutor(ExecutorType.Activate, CardId.Upstart);
            AddExecutor(ExecutorType.Activate, CardId.Terraforming, TerraformingAct);
			AddExecutor(ExecutorType.Activate, CardId.Chicken, ChickenAct);
			AddExecutor(ExecutorType.Activate, CardId.Hole, HoleAct);
            //activate trap
            AddExecutor(ExecutorType.Activate, CardId.ThreateningRoar, ThreateningRoareff);
            AddExecutor(ExecutorType.Activate, CardId.Waboku, Wabokueff);
			AddExecutor(ExecutorType.Activate, CardId.RecklessGreed);
			AddExecutor(ExecutorType.Activate, CardId.BattleFader, BattleFadereff); 
			AddExecutor(ExecutorType.Activate, CardId.Scarecrow, Scarecroweff); 
			AddExecutor(ExecutorType.Activate, CardId.Escape, Escapeeff);

        }
        public bool Has_prevent_list_0(int id)
        {
            return (
                    id == CardId.Waboku ||
                    id == CardId.ThreateningRoar||
                    id == CardId.Escape
                   );
        }
        public bool Has_prevent_list_1(int id)
        {
            return (id == CardId.Scarecrow ||
                    id == CardId.BattleFader
					);
        }
        bool prevent_used = false;
        int preventcount = 0;        
        bool Linkuribohused = true;
        int Waboku_count = 0;
        int Roar_count = 0;
        public override bool OnSelectHand()
        {
            return true;
        }

        public override void OnNewTurn()
        {
            prevent_used = false;
            Linkuribohused = true;
        }
        public override void OnNewPhase()
        {
            preventcount = 0;          
            IList<ClientCard> trap = Bot.GetSpells();
            IList<ClientCard> monster = Bot.GetMonsters();

            foreach (ClientCard card in trap)
            {
                if (Has_prevent_list_0(card.Id))
                {
                    preventcount++;                    
                }
            }
            foreach (ClientCard card in monster)
            {
                if (Has_prevent_list_1(card.Id))
                {
                    preventcount++;                   
                }
            }
            Waboku_count = 0;
            Roar_count = 0;
			IList<ClientCard> check = Bot.GetSpells();
            foreach (ClientCard card in check)
            {
                if (card.IsCode(CardId.Waboku))
                    Waboku_count++;

            }
            foreach (ClientCard card in check)
            {
                if (card.IsCode(CardId.ThreateningRoar))
                    Roar_count++;

            }            
            if (Waboku_count >= 2) Waboku_count = 1;
            if (Roar_count >= 2) Roar_count = 1;
			if (Duel.Phase == DuelPhase.End) 
			{AI.SelectCard(CardId.CardcarD, CardId.Scarecrow, CardId.BattleFader);}
            }
        private bool ThreateningRoareff()
        {
            if (DefaultOnBecomeTarget())
            {
                prevent_used = true;
                return true;
            }
            if (prevent_used || Duel.Phase != DuelPhase.BattleStart) return false;
            prevent_used = true;
            return DefaultUniqueTrap();
        }
		private bool Wabokueff()
        {               
            if (DefaultOnBecomeTarget())
            {
                Linkuribohused = false;
                prevent_used = true;
                return true;
            }            
            if (prevent_used||Duel.Player == 0||Duel.Phase!=DuelPhase.BattleStart) return false;
            prevent_used = true;
            Linkuribohused = false;
            return DefaultUniqueTrap();
        }
		private bool ChickenAct()
        {
            if (Bot.LifePoints <= 1000) return false;
            if (Bot.HasInSpellZone(CardId.Hole)) return false;
            if (ActivateDescription == Util.GetStringId(CardId.Chicken, 0))
                return true;
            return false;
        }
		private bool HoleAct()
        {
			if (Bot.GetMonsterCount() >= Enemy.GetMonsterCount()) return false;
			if (Bot.HasInHand(CardId.Chicken)) return false;
			if (Bot.HasInSpellZone(CardId.Hole)) return false;
            return false;
        }
		private bool TerraformingAct()
        {
            if (Bot.GetCountCardInZone(Bot.SpellZone, CardId.Chicken) + Bot.GetCountCardInZone(Bot.Hand, CardId.Chicken) + Bot.GetCountCardInZone(Bot.Graveyard, CardId.Chicken) <= 2)
            {
                AI.SelectCard(CardId.Chicken);
                return true;
            }
			else {
			AI.SelectCard(CardId.Hole);
			return true;
			}
            return true;
        }
        private bool BattleFadereff()
        {
            if (Bot.HasInHand(CardId.Scarecrow))
                return false;
			if (prevent_used || Duel.Player == 0) return false;
			if (Bot.SpellZone[5].IsCode(CardId.Hole)) return false;
            AI.SelectPosition(CardPosition.FaceUpDefence);
            prevent_used = true;
            return true;
        }
		private bool Scarecroweff()
        {
            if (prevent_used || Duel.Player == 0) return false;
            prevent_used = true;
            return true;
        }
		protected bool Escapeeff()
        {
            if (Bot.LifePoints <= 1000)
                return false;
            if ((Enemy.LifePoints - Bot.LifePoints) >= 2000)
                return true;
            return false;
        }
    }
}