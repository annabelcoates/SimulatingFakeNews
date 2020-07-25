using ModelAttemptWPF;
using System;

public class Person
{
    // The 'Big 5' Personality traits
    public double o; // Openess
    public double c; // Conscientiousness
    public double e; // Extraversion
    public double a; // Agreeableness
    public double n; // Neuroticism

   

    public double freqUse; // A measure of how likely a user is to check social media in a 15 min timeslot, 0-1
    public double sessionLength; // A measure of how many posts a person sees in 1 view of their feed, 0-1, Maybe should be int?
    public double largeNetwork; // A measure of how likely someone is to have a large network group, 0-1

    public double emotionalState; // Represents how emotional someone feels, 0-1
    public double politicalLeaning; // 0 represents 'the left', 1 represents 'the right'
    public double onlineLiteracy; // how likely someone is to believe fake news
    public double sharingFreq;

    public string name;

    public Random random = new Random();
    public Person(string name,double o, double c, double e, double a, double n, double politicalLeaning, double onlineLiteracy)
	{
        this.name = name;
        // Make these normalised?
        this.o = o;
        this.c = c;
        this.e = e;
        this.a = a;
        this.n = n;

        this.politicalLeaning = politicalLeaning;
        this.onlineLiteracy = onlineLiteracy;

        this.emotionalState = 0.5; // emotional state starts average
        this.DetermineBehaviours(); // set the behavioural parameters based on the personality traits
	}

    public void DetermineBehaviours()
    {
        // Behaviours based on findings from Caci et al. 2014 and Amichai-Hamburger & Vinitzsky 2010

        // neuroticism --> frequent use, conscientiousness --> infrequent use (Caci) 
        this.freqUse = this.n * 0.75 - (this.c * 0.5); // coefficients are just best guess

        // extraversion --> long sessions (Caci) 
        this.sessionLength = this.e;

        // extraversion --> larger network (Caci)
        // conscientiousness --> larger network (AH&V)
        this.largeNetwork = Math.Min(1, this.e + this.c); // sum the e and the c, but cannot be greater than 1

        // research on likelihood of sharing needed
        this.sharingFreq = 1; //this.e*this.e;
    }

    public bool WillShare(News news)
    {
        // for now 50% chance
        return (random.NextDouble() < 0.5);
    }

    public double AssesNews(News news)
    {
        // Currently the news is assessed according to 3 factors equally, politics, emotional level and believability

        // political factor is higher if the political leanings are closer
        double politicalFactor = 1 - Math.Abs(news.politicalLeaning - this.politicalLeaning);

        // how much the news appeals emotionally increases with the person's emotional level and how emotional the news is
        double emotionalFactor = Math.Min(1, this.emotionalState + news.emotionalLevel);

        // The perceived believability is dependent on the believability of the article and the person's online literacy
        double believabilityFactor = Math.Max(0,news.believability-0.25*this.onlineLiteracy);

        // According to Pennycook & Rand (2018) failing to identify news is fake is the biggest affector of how likely a person is to believe and therefore share it (partisanship/ political factor is more minor)

        double shareProb = this.sharingFreq * (politicalFactor + emotionalFactor + 4*believabilityFactor) / 5;
        //Console.WriteLine(this.name+" probability of sharing "+news.name+": " + shareProb);
        // return the likelihood that someone will share the news
        return shareProb;

    }
}
