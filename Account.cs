using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ModelAttemptWPF
{
    
    public class Account
    {
        private readonly MainWindow window;
        public OSN osn;
        public int ID;
        public string name; // An ID to identify the account
        public List<Account> following=new List<Account>(); // A list of all the accounts this account follows
        public List<Account> followers=new List<Account>();

        public double freqUse; // The % likelihood that a user is online in a single 12 hour block
        public List<Post> page= new List<Post>(); // A list of posts this account has shared
        public List<Post> feed= new List<Post>(); // A list of posts that the account sees
        private Random random= new Random();
        private int staticTime = 0; // temp variable to handle time
        public Account(MainWindow window,OSN osn, string name, double freqUse)
        {
            this.osn = osn;
            this.ID = osn.IDCount;
            this.window = window;
            this.name = name;
            this.freqUse = freqUse;
        }

        public News CreateFakeNews(string ID, int hourOfDay)
        {
            News news = new News(ID, false);
            ShareNews(news, hourOfDay);
            return news;
        }
        // Could make these one function?
        public News CreateTrueNews(string ID, int hourOfDay)
        {
            News news = new News(ID, false);
            ShareNews(news, hourOfDay);
            return news;
        }

        private void ShareNews(News news, int hourOfDay)
        { 
            // can only be called by CreateFakeNews(), CreateTrueNews() and ViewFeed()
                Post newPost = new Post(news, hourOfDay,this);
                page.Add(newPost);
        }

        public void ViewFeed()
        {
            foreach( Account account in following)
            {
                foreach( Post post in account.page)
                {
                    if (random.Next(101) < freqUse)
                    {
                        ShareNews(post.news, staticTime);
                    }
                }
            }
        }
        public void OutputPage()
        {
                this.window.GUIAccount.Content = this.name + "'s Page \n";
                foreach(Post post in this.page)
                {
                    this.window.GUIAccount.Content += "post:"+post.news.ID +"\n";
                }
        }

        public void Follow(Account followingAccount)
        {
            followingAccount.followers.Add(this);
            this.following.Add(followingAccount);
        }
    }
}