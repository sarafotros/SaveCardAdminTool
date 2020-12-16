using System;
using System.Collections.Generic;

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

    }
    
}