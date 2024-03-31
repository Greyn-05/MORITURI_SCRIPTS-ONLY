using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SettingOfBlackJack
{
    #region Card -----
    
    public class Card
    {
        public enum Suit { Club, Heart, Diamond, Spade }
        public enum Rank { Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace }
    
        public Suit suit { get; private set; }
        public Rank rank { get; private set; }
    
        public Card(Suit suit, Rank rank)
        {
            this.suit = suit;
            this.rank = rank;
        }
    
        public int GetValue()
        {
            if (rank >= Rank.Jack && rank <= Rank.King)
            {
                return 10;
            }
            else if (rank == Rank.Ace)
            {
                return 11;
            }
            else
            {
                return (int)rank;
            }
        }
    }
    
    #endregion
    
    #region Deck -----
    
    public class Deck
    {
        private List<Card> cards;

        public Deck()
        { 
            cards = new List<Card>();
        
            foreach (Card.Suit suit in Enum.GetValues(typeof(Card.Suit)))
            {
                foreach (Card.Rank rank in Enum.GetValues(typeof(Card.Rank)))
                {
                    cards.Add(new Card(suit, rank));
                }
            }

            Shuffle();
        }
    
        public void Shuffle()
        {
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                Card value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }

        // 덱에서 카드를 한 장 뽑습니다.
        public Card DrawCard()
        {
            if (cards.Count > 0)
            {
                Card card = cards[0];
                cards.RemoveAt(0);
                return card;
            }
            else
            {
                Debug.LogError("Deck is empty!");
                return null;
            }
        }
    }
    
    #endregion
    
    #region Hand -----
    
    public class Hand
    {
        private List<Card> cards = new();
        
        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        public int GetTotalValue()
        {
            int total = 0;
            int aceCount = 0;

            //전체 hand 점수 카운팅 함수인데
            //일부러 ace개수도 세는 중
            foreach (Card card in cards)
            {
                if (card.rank == Card.Rank.Ace)
                {
                    aceCount++;
                }
                total += card.GetValue();
            }

            while (total > 21 && aceCount > 0)
            {
                total -= 10;
                aceCount--;
            }

            return total;
        }
        
        public int GetCardsCount()
        {
            return cards.Count;
        }
        
        public void Clear()
        {
            cards.Clear();
        }
    }
    
    #endregion
}
