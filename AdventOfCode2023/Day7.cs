using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2023
{
    internal class Day7
    {
        public async Task Run()
        {
            var path = @"C:\Users\matth\source\repos\AdventOfCode2023\AdventOfCode2023\data\day7.txt";
            var inputs = await System.IO.File.ReadAllLinesAsync(path);
            var hands = new List<Hand>();

            foreach (var input in inputs)
            {
                var game = input.Split(" ").ToList();
                var hand = new Hand(game.First(), long.Parse(game.Last()));
                hands.Add(hand);
            }

            hands.Sort();

            var totalWinnings = 0L;
            var handRank = 1L;
            foreach (var hand in hands)
            {
                totalWinnings += hand.Score * handRank;
                handRank++;
            }

            Console.WriteLine($"Total winnings are {totalWinnings}");

            hands.Sort(Hand.CompareToJokers);

            var totalWinningsJokers = 0L;
            handRank = 1L;
            foreach (var hand in hands)
            {
                totalWinningsJokers += hand.Score * handRank;
                handRank++;
            }

            Console.WriteLine($"Total winnings are {totalWinningsJokers}");
        }
    }

    internal class Hand : IComparable<Hand>
    {
        public Hand(string hand, long score)
        {
            HandString = hand;
            Score = score;
            ParseHand(hand);
            ParseHandJokers(hand);
            Strength = SetStrength();
            JokerStrength = SetJokerStrength();
        }

        public string HandString { get; set; }
        public List<CardValue> Cards { get; set; } = new List<CardValue>();
        public List<CardValueJokers> CardsJoker { get; set; } = new List<CardValueJokers>();
        public HandStrength Strength { get; set; }
        public HandStrength JokerStrength { get; set; }
        public long Score { get; set; }

        private HandStrength SetStrength()
        {
            var groupedCards = Cards.GroupBy(x => x);
            if (groupedCards.Any(x => x.Count() == 5))
                return HandStrength.FiveOfAKind;
            else if (groupedCards.Any(x => x.Count() == 4))
                return HandStrength.FourOfAKind;
            else if (groupedCards.Any(x => x.Count() == 3) && groupedCards.Any(x => x.Count() == 2))
                return HandStrength.FullHouse;
            else if (groupedCards.Any(x => x.Count() == 3))
                return HandStrength.ThreeOfAKind;
            else if (groupedCards.Where(x => x.Count() == 2).Count() > 1)
                return HandStrength.TwoPair;
            else if (groupedCards.Any(x => x.Count() == 2))
                return HandStrength.OnePair;
            else
                return HandStrength.HighCard;
        }

        private HandStrength SetJokerStrength()
        {
            var jokersCount = CardsJoker.Count(x => x == CardValueJokers.Joker);
            var localCardList = CardsJoker.Where(x => x != CardValueJokers.Joker).ToList();

            if (localCardList.Any())
            {
                var highCard = CardValueJokers.Joker;
                var count = 0;
                foreach (var card in localCardList.GroupBy(x => x))
                {
                    if (card.Count() > count)
                    {
                        count = card.Count();
                        highCard = card.Key;
                    }
                }

                for (var i = 0; i < jokersCount; i++)
                {
                    localCardList.Add(highCard);
                }
            }

            var groupedCards = localCardList.GroupBy(x => x);

            if (groupedCards.Any(x => x.Count() == 5) || jokersCount == 5)
                return HandStrength.FiveOfAKind;
            else if (groupedCards.Any(x => x.Count() == 4))
                return HandStrength.FourOfAKind;
            else if (groupedCards.Any(x => x.Count() == 3) && groupedCards.Any(x => x.Count() == 2))
                return HandStrength.FullHouse;
            else if (groupedCards.Any(x => x.Count() == 3))
                return HandStrength.ThreeOfAKind;
            else if (groupedCards.Where(x => x.Count() == 2).Count() > 1)
                return HandStrength.TwoPair;
            else if (groupedCards.Any(x => x.Count() == 2))
                return HandStrength.OnePair;
            else
                return HandStrength.HighCard;
        }

        private void ParseHand(string hand)
        {
            foreach (var card in hand.ToCharArray())
            {
                if(char.IsNumber(card))
                    Cards.Add(Enum.Parse<CardValue>(card.ToString()));
                else if(card == 'T')
                    Cards.Add(CardValue.Ten);
                else if(card == 'J')
                    Cards.Add(CardValue.Jack);
                else if(card == 'Q')
                    Cards.Add(CardValue.Queen);
                else if(card == 'K')
                    Cards.Add(CardValue.King);
                else if(card == 'A')
                    Cards.Add(CardValue.Ace);
            }
        }

        private void ParseHandJokers(string hand)
        {
            foreach (var card in hand.ToCharArray())
            {
                if (char.IsNumber(card))
                    CardsJoker.Add(Enum.Parse<CardValueJokers>(card.ToString()));
                else if (card == 'T')
                    CardsJoker.Add(CardValueJokers.Ten);
                else if (card == 'J')
                    CardsJoker.Add(CardValueJokers.Joker);
                else if (card == 'Q')
                    CardsJoker.Add(CardValueJokers.Queen);
                else if (card == 'K')
                    CardsJoker.Add(CardValueJokers.King);
                else if (card == 'A')
                    CardsJoker.Add(CardValueJokers.Ace);
            }
        }

        public int CompareTo(Hand? other)
        {
            if (Strength.CompareTo(other.Strength) != 0)
            {
                return Strength.CompareTo(other.Strength);
            }

            for (var i = 0; i < 5; i++)
            {
                if (Cards[i].CompareTo(other.Cards[i]) != 0)
                    return Cards[i].CompareTo(other.Cards[i]);
            }

            return 0;
        }

        public static int CompareToJokers(Hand? x, Hand? y)
        {
            if (x.JokerStrength.CompareTo(y.JokerStrength) != 0)
            {
                return x.JokerStrength.CompareTo(y.JokerStrength);
            }

            for (var i = 0; i < 5; i++)
            {
                if (x.CardsJoker[i].CompareTo(y.CardsJoker[i]) != 0)
                    return x.CardsJoker[i].CompareTo(y.CardsJoker[i]);
            }

            return 0;
        }
    }

    public enum HandStrength
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }

    [DataContract]
    public enum CardValue
    {
        [EnumMember(Value = "2")]
        Two = 2,
        [EnumMember(Value = "3")]
        Three,
        [EnumMember(Value = "4")]
        Four,
        [EnumMember(Value = "5")]
        Five,
        [EnumMember(Value = "6")]
        Six,
        [EnumMember(Value = "7")]
        Seven,
        [EnumMember(Value = "8")]
        Eight,
        [EnumMember(Value = "9")]
        Nine,
        [EnumMember(Value = "T")]
        Ten,
        [EnumMember(Value = "J")]
        Jack,
        [EnumMember(Value = "Q")]
        Queen,
        [EnumMember(Value = "K")]
        King,
        [EnumMember(Value = "A")]
        Ace
    }

    [DataContract]
    public enum CardValueJokers
    {
        [EnumMember(Value = "J")]
        Joker = 1,
        [EnumMember(Value = "2")]
        Two,
        [EnumMember(Value = "3")]
        Three,
        [EnumMember(Value = "4")]
        Four,
        [EnumMember(Value = "5")]
        Five,
        [EnumMember(Value = "6")]
        Six,
        [EnumMember(Value = "7")]
        Seven,
        [EnumMember(Value = "8")]
        Eight,
        [EnumMember(Value = "9")]
        Nine,
        [EnumMember(Value = "T")]
        Ten,
        [EnumMember(Value = "Q")]
        Queen,
        [EnumMember(Value = "K")]
        King,
        [EnumMember(Value = "A")]
        Ace
    }
}
