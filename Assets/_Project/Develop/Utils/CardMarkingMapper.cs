using Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public sealed class CardMarkingMapper
    {
        public static Dictionary<Ranks, string> RanksMap { get; private set; } = new();

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

        public static Ranks GetRandomRank() => _ranks[UnityEngine.Random.Range(0, _ranks.Count)];
        public static Suits GetRandomSuits() => _suits[UnityEngine.Random.Range(0, _suits.Count)];
        public static Suits GetRandomRedSuits() => _redSuits[UnityEngine.Random.Range(0, _redSuits.Count)];
        public static Suits GetRandomBlackSuits() => _blackSuits[UnityEngine.Random.Range(0, _blackSuits.Count)];

        private static void InitRanksMap()
        {
            RanksMap[Ranks.Six] = "6";
            RanksMap[Ranks.Seven] = "7";
            RanksMap[Ranks.Eight] = "8";
            RanksMap[Ranks.Nine] = "9";
            RanksMap[Ranks.Ten] = "10";
            RanksMap[Ranks.Jack] = "J";
            RanksMap[Ranks.Queen] = "Q";
            RanksMap[Ranks.King] = "K";
            RanksMap[Ranks.Ace] = "T";
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

        public static List<T> ToList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
    }
}
