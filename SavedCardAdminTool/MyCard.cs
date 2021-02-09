using System;

namespace SavedCardAdminTool
{
    public class MyCard
    {
        public MyCard(string id)
        {
            CardId = id;
            DateAdded = DateTime.Now;
        }

        public string CardId { get;}
        public DateTime DateAdded { get;  }
    }
}