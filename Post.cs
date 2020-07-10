using System;

namespace ModelAttemptWPF
{
    public class Post
    {
        public News news;
        public Account poster;
        public int hourOfDay;
        public int popularity;
        public int uniqueViews;
        public int totalViews;

        public Post(News news, int hourOfDay, Account poster)
        {
            this.news = news;
            this.hourOfDay = hourOfDay;
            this.poster = poster;
        }
    }
}