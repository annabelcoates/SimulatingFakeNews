using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelAttemptWPF
{
    public class News
    {
        public string ID;
        public bool isTrue;
        public int totalViews;
        public int uniqueViews; // equal to the count of the viewers list so probably not needed
        public List<Account> viewers = new List<Account>();
        public News(string ID, bool isTrue)
        {
            this.ID=ID;
            this.isTrue = isTrue;
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