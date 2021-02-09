using System;
using System.Collections.Generic;
using System.Linq;

namespace SavedCardAdminTool
{
    public class Customer
    {
        public Customer( string fullName, string emailAddress)
        {
            Id = Guid.NewGuid().ToString();
            FullName = fullName;
            EmailAddress = emailAddress;
            SaveCards = new List<Card>();
            JoinDate = DateTime.Now;
        }

        public string Id { get;  }
        public string FullName { get;  }
        public DateTime JoinDate { get;  }
        public string EmailAddress { get;  }
        
        public List<Card> SaveCards { get; }

        public void SaveCard(Card card )
        {
           SaveCards.Add(card); 
        }

        public void RemoveCard(Card card)
        {
            SaveCards.Remove(card);
        }

        public void PrintAllSaveCards()
        {
            foreach (var card in SaveCards)
            {
                card.PrintDetails();
            }
        }

        public List<Card> RemoveExpiredCards()
        {
            foreach (var card in SaveCards)
            {
                var expiryMonth = Int32.Parse(card.ExpiryDate.Split("/").First());
                var expiryYear = Int32.Parse("20" + card.ExpiryDate.Split("/").Last());
                var cardExpiryDate = new DateTime(expiryYear, expiryMonth,DateTime.DaysInMonth(expiryYear, expiryMonth));
                if (cardExpiryDate < DateTime.Now.Date)
                { 
                    SaveCards.Remove(card);
                }
            }
            return SaveCards;
        }

    }
    
}