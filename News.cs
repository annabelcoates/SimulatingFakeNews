using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAttemptWPF
{
    public class News
    {
        public int ID;
        public string name;
        public bool isTrue;
        public double emotionalLevel;
        public double politicalLeaning; // 0 represents 'the left', 1 represents 'the right'
        public double believability;

        public int totalViews;
        public int uniqueViews; // equal to the count of the viewers list so probably not needed
        public List<Account> viewers = new List<Account>();

        // Statistics
        public int nShared = 0;
        public List<int> nSharedList = new List<int>() { 0 };

        public Random random = new Random();

        public News(int ID,string name, bool isTrue, double emotionalLevel,double believability)
        {
            this.ID = ID;
            this.name=name;
            this.isTrue = isTrue;
            this.politicalLeaning = random.NextDouble();
            this.emotionalLevel = emotionalLevel;
            this.believability = believability;

        }

        public bool HasSeen(Account account)
        {
            int accountID = account.ID; //for speed
            // method to determine if news has been seen by a user before
            foreach(Account viewer in viewers)
            {
                if (viewer.ID == accountID)
                {
                    return true;
                }
            }
            return false;
        }
    }
}