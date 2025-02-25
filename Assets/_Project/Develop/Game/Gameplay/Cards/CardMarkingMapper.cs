using Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gameplay
{
    public sealed class CardMarkingMapper
    {
        private static Dictionary<Ranks, string> _ranksViewMap = new();
        private static Dictionary<Ranks, int> _ranksValueMap = new();

        private static List<Ranks> _ranks = new();
        private static List<Suits> _suits = new();
        private static List<Suits> _redSuits = new();
        private static List<Suits> _blackSuits = new();

        static CardMarkingMapper()
        {
            InitRanksMap();
            InitRanks();
            InitSuits();
        }

        public static string GetRankView(Ranks rank) => _ranksViewMap[rank];
        public static int GetRankValue(Ranks rank) => _ranksValueMap[rank];
        public static Ranks GetRandomRank() => _ranks[UnityEngine.Random.Range(0, _ranks.Count)];
        public static Suits GetRandomSuits() => _suits[UnityEngine.Random.Range(0, _suits.Count)];
        public static Suits GetRandomRedSuits() => _redSuits[UnityEngine.Random.Range(0, _redSuits.Count)];
        public static Suits GetRandomBlackSuits() => _blackSuits[UnityEngine.Random.Range(0, _blackSuits.Count)];
        public static Ranks GetRankByIndex(int index) => _ranks[index];
        public static int GetRankIndex(Ranks rank) => _ranks.IndexOf(rank);
        public static Suits GetSuitByIndex(int index) => _suits[index];
        public static int GetSuitIndex(Suits suit) => _suits.IndexOf(suit);

        private static void InitRanksMap()
        {
            _ranksViewMap[Ranks.Six] = "6";
            _ranksViewMap[Ranks.Seven] = "7";
            _ranksViewMap[Ranks.Eight] = "8";
            _ranksViewMap[Ranks.Nine] = "9";
            _ranksViewMap[Ranks.Ten] = "10";
            _ranksViewMap[Ranks.Jack] = "J";
            _ranksViewMap[Ranks.Queen] = "Q";
            _ranksViewMap[Ranks.King] = "K";
            _ranksViewMap[Ranks.Ace] = "T";

            _ranksValueMap[Ranks.Six] = 6;
            _ranksValueMap[Ranks.Seven] = 7;
            _ranksValueMap[Ranks.Eight] = 8;
            _ranksValueMap[Ranks.Nine] = 9;
            _ranksValueMap[Ranks.Ten] = 10;
            _ranksValueMap[Ranks.Jack] = 11;
            _ranksValueMap[Ranks.Queen] = 12;
            _ranksValueMap[Ranks.King] = 13;
            _ranksValueMap[Ranks.Ace] = 14;
        }

        private static void InitRanks()
        {
            _ranks = ToList<Ranks>();
        }

        private static void InitSuits()
        {
            _suits = ToList<Suits>();

            _redSuits = new List<Suits>(_suits);
            _redSuits.Remove(Suits.Club);
            _redSuits.Remove(Suits.Spade);

            _blackSuits = new List<Suits>(_suits);
            _blackSuits.Remove(Suits.Heart);
            _blackSuits.Remove(Suits.Diamonds);
        }

        private static List<T> ToList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
    }
}
