using System;

namespace ModelAttemptWPF
{
    public class Post
    {
        public News news;
        public Account poster;
        public double time;
        public int popularity;
        public int uniqueViews;
        public int totalViews;

        public Post(News news, double time, Account poster)
        {
            this.news = news;
            this.time = time;
            this.poster = poster;
        }
    }
}