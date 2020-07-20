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
    public double sessionLength; // A measure of how many posts a person sees in 1 view of their feed, 0-1
    public double largeNetwork; // A measure of how likely someone is to have a large network group, 0-1

    public double emotionalState; // Represents how emotional someone feels, 0-1

    public string name;
    public Person(string name,double o, double c, double e, double a, double n)
	{
        this.name = name;
        this.o = o;
        this.c = c;
        this.e = e;
        this.a = a;
        this.n = n;

        emotionalState = 0.5; // emotional state starts average
        this.DetermineBehaviours(); // set the behavioural parameters based on the personality traits
	}

    public void DetermineBehaviours()
    {
        // This would really lend itself to fuzzy logic

        // Behaviours based on findings from Caci et al. 2014 and Amichai-Hamburger & Vinitzsky 2010

        // neuroticism --> frequent use, conscientiousness --> infrequent use (Caci) 
        this.freqUse = this.n * 0.75 - (this.c * 0.5); // coefficients are just best guess

        // extraversion --> long sessions (Caci) 
        this.sessionLength = this.e;

        // extraversion --> larger network (Caci)
        // conscientiousness --> larger network (AH&V)
        this.largeNetwork = Math.Min(1, this.e + this.c); // sum the e and the c, but cannot be greater than 1
    }
}
