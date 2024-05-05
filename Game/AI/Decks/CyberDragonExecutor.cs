using YGOSharp.OCGWrapper.Enums;
using System.Collections.Generic;
using System.Linq;
using WindBot;
using WindBot.Game;
using WindBot.Game.AI;

namespace WindBot.Game.AI.Decks
{
    // NOT FINISHED YET
    [Deck("CyberDragon", "AI_CyberDragon", "NotFinished")]
    public class CyberDragonExecutor : DefaultExecutor
    {
        bool PowerBondUsed = false;

        public class CardId
        {
            public const int AshBlossomAndJoyousSpring = 14558127;
            public const int MaxxC = 23434538;
			
			public const int NaturalExterio = 99916754;
            public const int NaturalBeast = 33198837;
            public const int SwordsmanLV7 = 37267041;
            public const int RoyalDecreel = 51452091;
			public const int ImperialOrder = 61740673;
			
            public const int CyberDragon = 70095154;
            public const int CyberDragonVier = 29975188;
            public const int CyberDragonCore = 23893227;
			public const int CyberDragonHerz = 56364287;
			public const int CyberDragonNachster = 1142880;
			
			public const int HarpieFeatherDuster = 18144506;
			public const int CalledByTheGrave = 24224830;
			public const int ReadyFusion = 63854005;
            public const int Raigeki = 12580477;
			public const int lightningStorm = 14532163;
            public const int Polymerization = 24094653;
            public const int PowerBond = 37630732;
			public const int OverloadFusion = 3659803;
			public const int MachineDuplication = 63995093;
			public const int CyberEmergency = 60600126;
			public const int CyberRevsystem = 33041277;
			public const int LimiterRemoval = 23171610;
			
            public const int CyberTwinDragon = 74157028;
			public const int CyberEndDragon = 1546123;
			public const int ChimeratechRampageDragon = 84058253;
            public const int CyberDragonInfinity = 10443957;
            public const int CyberDragonNova = 58069384;
			public const int CyberDragonSieger = 46724542;
        }

        public CyberDragonExecutor(GameAI ai, Duel duel)
            : base(ai, duel)
        { 
			//first do
			AddExecutor(ExecutorType.Activate, CardId.lightningStorm, DefaultLightingStorm);
            AddExecutor(ExecutorType.Activate, CardId.HarpieFeatherDuster, DefaultHarpiesFeatherDusterFirst);
			AddExecutor(ExecutorType.Activate, CardId.Raigeki, DefaultRaigeki);			
			//counter
            AddExecutor(ExecutorType.Activate, CardId.CalledByTheGrave, DefaultCalledByTheGrave);
			//first do	
			AddExecutor(ExecutorType.SpSummon, CardId.CyberDragon);
            AddExecutor(ExecutorType.Summon, CardId.CyberDragonCore);
			AddExecutor(ExecutorType.Activate, CardId.CyberDragonVier);
			AddExecutor(ExecutorType.Activate, CardId.CyberDragonNachster);
			
			AddExecutor(ExecutorType.Summon, CardId.CyberDragonVier);
			AddExecutor(ExecutorType.MonsterSet, CardId.CyberDragonHerz);
			AddExecutor(ExecutorType.Summon, CardId.CyberDragonNachster);
			
			AddExecutor(ExecutorType.SpSummon, CardId.CyberDragonNova, CyberDragonNovaSummon);
            AddExecutor(ExecutorType.Activate, CardId.CyberDragonNova, CyberDragonNovaEffect);
            AddExecutor(ExecutorType.SpSummon, CardId.CyberDragonInfinity, CyberDragonInfinitySummon);
            AddExecutor(ExecutorType.Activate, CardId.CyberDragonInfinity, CyberDragonInfinityEffect);
			AddExecutor(ExecutorType.SpSummon, CardId.CyberDragonSieger, CyberDragonSiegerSummon);
			AddExecutor(ExecutorType.Activate, CardId.CyberDragonSieger);
			
            AddExecutor(ExecutorType.Activate, CardId.Raigeki, DefaultRaigeki);
            AddExecutor(ExecutorType.Activate, CardId.Polymerization, PolymerizationEffect);
            AddExecutor(ExecutorType.Activate, CardId.PowerBond, PowerBondEffect);
			AddExecutor(ExecutorType.Activate, CardId.OverloadFusion, OverloadFusionEffect);
			AddExecutor(ExecutorType.Activate, CardId.MachineDuplication, MachineDuplicationEffect);
			AddExecutor(ExecutorType.Activate, CardId.CyberEmergency, CyberEmergencyEffect);
			AddExecutor(ExecutorType.Activate, CardId.CyberRevsystem);
			AddExecutor(ExecutorType.Activate, CardId.LimiterRemoval, LimiterRemovalEffect);
			
			AddExecutor(ExecutorType.Activate, CardId.AshBlossomAndJoyousSpring, DefaultAshBlossomAndJoyousSpring);
            AddExecutor(ExecutorType.Activate, CardId.MaxxC, DefaultMaxxC);
			
            AddExecutor(ExecutorType.Activate, CardId.CyberDragonCore, CyberDragonCoreEffect);
			AddExecutor(ExecutorType.Activate, CardId.CyberDragonHerz, CyberDragonHerzEffect);
			AddExecutor(ExecutorType.Activate, CardId.ChimeratechRampageDragon, ChimeratechRampageDragonEffect1);
			AddExecutor(ExecutorType.Activate, CardId.ChimeratechRampageDragon, ChimeratechRampageDragonEffect2);

            AddExecutor(ExecutorType.SpSummon, CardId.CyberEndDragon);
            AddExecutor(ExecutorType.SpSummon, CardId.CyberTwinDragon);
			AddExecutor(ExecutorType.SpSummon, CardId.ChimeratechRampageDragon);
			
			AddExecutor(ExecutorType.SpellSet, DefaultSpellSet);
        }

        private bool CyberDragonInHand()  { return Bot.HasInHand(CardId.CyberDragon); }
        private bool CyberDragonInGraveyard()  { return Bot.HasInGraveyard(CardId.CyberDragon); }
        private bool CyberDragonInMonsterZone() { return Bot.HasInMonstersZone(CardId.CyberDragon); }
        private bool CyberDragonIsBanished() { return Bot.HasInBanished(CardId.CyberDragon); }
		private bool CyberDragonInfinitySummoned = false;

        public override void OnNewTurn()
        {
            CyberDragonInfinitySummoned = false;
			base.OnNewTurn();
        }
		
        private bool PolymerizationEffect()
        {
            if (Bot.GetCountCardInZone(Bot.MonsterZone, CardId.CyberDragon) + Bot.GetCountCardInZone(Bot.MonsterZone, CardId.CyberDragonVier) + Bot.GetCountCardInZone(Bot.MonsterZone, CardId.CyberDragonCore) + Bot.GetCountCardInZone(Bot.MonsterZone, CardId.CyberDragonHerz) + Bot.GetCountCardInZone(Bot.MonsterZone, CardId.CyberDragonNachster) + Bot.GetCountCardInZone(Bot.Hand, CardId.CyberDragon) >= 3)
                AI.SelectCard(CardId.ChimeratechRampageDragon);
            else 
				AI.SelectCard(CardId.CyberTwinDragon);
            return true;
        }

        private bool PowerBondEffect()
        {
            PowerBondUsed = true;
            if ((Bot.GetCountCardInZone(Bot.MonsterZone, CardId.CyberDragon) + Bot.GetCountCardInZone(Bot.MonsterZone, CardId.CyberDragonVier) + Bot.GetCountCardInZone(Bot.MonsterZone, CardId.CyberDragonCore) + Bot.GetCountCardInZone(Bot.MonsterZone, CardId.CyberDragonHerz) + Bot.GetCountCardInZone(Bot.MonsterZone, CardId.CyberDragonNachster) + Bot.GetCountCardInZone(Bot.Hand, CardId.CyberDragon) >= 4))
                AI.SelectCard(CardId.CyberTwinDragon);
			else 
				AI.SelectCard(CardId.ChimeratechRampageDragon);
            return true;
        }
		
		private bool CyberDragonHerzEffect()
        {
            if (!Bot.HasInHand(CardId.CyberDragonCore))
            {
                AI.SelectCard(CardId.CyberDragonCore);
                return true;
            }
            else if (!Bot.HasInHand(CardId.CyberDragon))
            {
                AI.SelectCard(CardId.CyberDragon);
                return true;
            }
            else if (!Bot.HasInHand(CardId.CyberDragonVier))
            {
                AI.SelectCard(CardId.CyberDragonVier);
                return true;
            }
            else if (!Bot.HasInHand(CardId.CyberDragonHerz))
            {
                AI.SelectCard(CardId.CyberDragonHerz);
                return true;
            }
            else if (!Bot.HasInHand(CardId.CyberDragonNachster))
            {
                AI.SelectCard(CardId.CyberDragonNachster);
                return true;
            }
            return true;
        }
		
		private bool CyberDragonCoreEffect()
        {
            if (!Bot.HasInHand(CardId.CyberEmergency))
            {
                AI.SelectCard(CardId.CyberEmergency);
                return true;
            }
            else if (!Bot.HasInHand(CardId.CyberRevsystem))
            {
                AI.SelectCard(CardId.CyberRevsystem);
                return true;
            }
            return true;
        }
		
		private bool CyberEmergencyEffect()
        {
            if (!Bot.HasInHand(CardId.CyberDragonCore))
            {
                AI.SelectCard(CardId.CyberDragonCore);
                return true;
            }
            else if (!Bot.HasInHand(CardId.CyberDragon))
            {
                AI.SelectCard(CardId.CyberDragon);
                return true;
            }
            else if (!Bot.HasInHand(CardId.CyberDragonVier))
            {
                AI.SelectCard(CardId.CyberDragonVier);
                return true;
            }
            else if (!Bot.HasInHand(CardId.CyberDragonHerz))
            {
                AI.SelectCard(CardId.CyberDragonHerz);
                return true;
            }
            else if (!Bot.HasInHand(CardId.CyberDragonNachster))
            {
                AI.SelectCard(CardId.CyberDragonNachster);
                return true;
            }
            return true;
        }
		
		private bool OverloadFusionEffect()
        {
			if (Duel.Turn == 1) return false;
            if ((Bot.Graveyard.GetMatchingCardsCount(card => (card.Race & (int)CardRace.Machine) > 0) >= 0) && !Bot.HasInMonstersZone(CardId.CyberDragonSieger))
				AI.SelectCard(CardId.ChimeratechRampageDragon);
            return true;
        }
		
		private bool ChimeratechRampageDragonEffect1()
        {
            IList<ClientCard> targets = Enemy.GetSpells();
            if (targets.Count > 0) {
				AI.SelectCard(targets);
                return true;
            }
            return false;
        }
		private bool ChimeratechRampageDragonEffect2()
        {
            if((Bot.GetRemainingCount(CardId.CyberDragonHerz, 3) + Bot.GetRemainingCount(CardId.CyberDragon, 3) + Bot.GetRemainingCount(CardId.CyberDragonCore, 3) + Bot.GetRemainingCount(CardId.CyberDragonNachster, 3) + Bot.GetRemainingCount(CardId.CyberDragonVier, 3)) > 1)
            {
                AI.SelectCard(CardId.CyberDragonCore, CardId.CyberDragonHerz, CardId.CyberDragon, CardId.CyberDragonNachster, CardId.CyberDragonVier);
                return true;
            }
            return false;
        }

		private bool MachineDuplicationEffect()
        {
            if(Bot.GetRemainingCount(CardId.CyberDragon, 3) > 0)
            {
                AI.SelectCard(CardId.CyberDragonCore, CardId.CyberDragonHerz, CardId.CyberDragonNachster);
                AI.SelectNextCard(CardId.CyberDragon, CardId.CyberDragon, CardId.CyberDragon);
                return true;
            }
            return false;
        }
		
        private bool CyberDragonNovaSummon()
        {
			AI.SelectMaterials(new List<int>() {
                        CardId.CyberDragon,
                        CardId.CyberDragonHerz
            });
            return !CyberDragonInfinitySummoned;
        }

        private bool CyberDragonNovaEffect()
        {
            if (ActivateDescription == Util.GetStringId(CardId.CyberDragonNova, 0))
            {
                return true;
            }
            else if (Card.Location == CardLocation.Grave)
            {
				AI.SelectCard(CardId.CyberEndDragon);
                AI.SelectPosition(CardPosition.FaceUpAttack);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CyberDragonInfinitySummon()
        {
            CyberDragonInfinitySummoned = true;
            return true;
        }

        private bool CyberDragonInfinityEffect()
        {
            if (Duel.CurrentChain.Count > 0)
            {
                return Duel.LastChainPlayer == 1;
            }
            else
            {
                ClientCard bestmonster = null;
                foreach (ClientCard monster in Enemy.GetMonsters())
                {
                    if (monster.IsAttack() && (bestmonster == null || monster.Attack >= bestmonster.Attack))
                        bestmonster = monster;
                }
                if (bestmonster != null)
                {
                    AI.SelectCard(bestmonster);
                    return true;
                }
            }
            return false;
        }
		
		private bool LimiterRemovalEffect()
        {
            if (Duel.Player == 0 && Duel.Phase == DuelPhase.BattleStart) return true;
			else 
				return false;
        }
		
		private bool CyberDragonSiegerSummon()
        {
            IList<ClientCard> material_list = new List<ClientCard>();
            foreach (ClientCard monster in Bot.GetMonsters())
            {
                if (monster.IsCode(CardId.CyberDragonCore, CardId.CyberDragonHerz, CardId.CyberDragonNachster, CardId.CyberDragonVier, CardId.CyberDragon))
                    material_list.Add(monster);
                if (material_list.Count == 2) break;
            }
            if (material_list.Count < 2) return false;
            if (Bot.HasInMonstersZone(CardId.CyberDragonSieger)) return false;
            AI.SelectMaterials(material_list);
            return true;
        }
    }
}