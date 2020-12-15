using System;

namespace SavedCardAdminTool
{
    public class Card
    {
        public Card(string type, string lastFourDigit, string expiryDate,  string nameOnCard, string cardId )
        {
            CardType = type;
            LastFourDigit = lastFourDigit;
            ExpiryDate = expiryDate;
            NameOnCard = nameOnCard;
            DateAdded = DateTime.Now;
            CardId = Guid.NewGuid().ToString();
        }

        public string CardType { get; }
        public string LastFourDigit { get; }
        public string ExpiryDate { get;  }
        public string CardId { get;  }
        public DateTime DateAdded { get;  }
        public string NameOnCard { get;  }

        public void PrintDetails()
        {
            Console.WriteLine( $"Type: {CardType}\t added on {DateAdded}\t last 4 digits: {LastFourDigit}.");
           
        }
    }
}