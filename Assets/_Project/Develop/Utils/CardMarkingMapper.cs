using Gameplay;
using GameplayServices;
using Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Utils
{
    public sealed class CardMarkingMapper
    {
        public static Dictionary<Ranks, string> RanksMap { get; private set; } = new();

        private static List<Ranks> _ranks = new();
        private static List<Suits> _suits = new();

        static CardMarkingMapper()
        {
            InitRanksMap();
            InitRanks();
            InitSuits();
        }

        public static Ranks GetRandomRank()
        {
            return _ranks[UnityEngine.Random.Range(0, _ranks.Count)];
        }

        public static Suits GetRandomSuits()
        {
            return _suits[UnityEngine.Random.Range(0, _suits.Count)];
        }

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
            _ranks.Capacity = 9;
            _ranks = ToList<Ranks>();
        }

        private static void InitSuits()
        {
            _suits.Capacity = 4;
            _suits = ToList<Suits>();
        }

        public static List<T> ToList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
    }
}
