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
        public List<Account> following=new List<Account>(); // A list of all the accounts this account follows
        public List<Account> followers=new List<Account>();

        public List<Post> page= new List<Post>(); // A list of posts this account has shared
        public List<Post> feed= new List<Post>(); // A list of posts that the account's follows have shared
        public List<News> seen = new List<News>(); // a list of all the posts the user has seen
        private Random random= new Random();
        private int staticTime = 0; // temp variable to handle time

        public Person person;

        public Account(MainWindow window,OSN osn, Person person)
        {
            this.osn = osn;
            this.ID = osn.IDCount;
            this.window = window;
            this.person = person;
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
            News news = new News(ID, true);
            ShareNews(news, hourOfDay);
            return news;
        }

        private void ShareNews(News news, int hourOfDay)
        { 
            // can only be called by CreateFakeNews(), CreateTrueNews() and ViewFeed()
                Post newPost = new Post(news, hourOfDay,this);
                this.page.Add(newPost);
        }

        public void ViewFeed()
        {
            foreach( Account account in following)
            {
                foreach( Post post in account.page)
                {
                    //Console.WriteLine(post.news.ID + " is viewed by "+this.person.name+" from "+account.person.name+"'s post, has seen: "+ post.news.HasSeen(this));
                    // TODO: Maybe put all the viewing logic into a function
                    post.totalViews++;
                    post.news.totalViews++;
                    if (post.news.HasSeen(this)==false)
                    {
                        post.news.viewers.Add(this);
                        this.seen.Add(post.news);
                    }
                    if (random.NextDouble() < person.freqUse & (this.HasPosted(post.news)==false)) // TODO: some way of determining if it's on the page already
                    {
                        Console.WriteLine(this.person.name + " shared the news");
                        this.ShareNews(post.news, 0);
                    }
                }
            }
        }
        public void OutputPage()
        {
                this.window.GUIAccount.Content = this.person.name + "'s Page \n";
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

        public bool HasSeen(News news)
        {
            return this.seen.Contains(news);
        }

        public bool HasPosted(News news)
        {
            foreach(Post post in this.page)
            {
                if (post.news.ID == news.ID)
                {
                    return true;
                }
            }
            return false;
        }
    }
}