using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using System; 

public class Quiz_YuChen : QuizScript
{
    public Quiz_YuChen()
    {
        Author = Authors.YuChen;
    }

    public override List<int> FilterEven(List<int> l)
    {
        List<int> r = new List<int>();
        foreach (int n in l)
        {
            if(n % 2 == 0)
            {
                r.Add(n);
            }
        }
        return r;
    }
    
    public override int FindBest(List<int> l)
    {
        int r = -999;
        foreach (int n in l)
        {
            if (n > r)
            {
                r = n;
            }
        }
        return r;
    }

    public override List<int> SortHighToLow(List<int> l)
    {
        List<int> r = new List<int>();
        int safety = 999;
        while (safety > 0 && l.Count > 0) //Needs an &&
        {
           
            safety--;
            r.Sort((a, b) => b.CompareTo(a));

        }
        return r;
    }


    public override int ReturnRandom(List<int> l)
    {
        return 0;
        // Random num = new Random();
        // return l[num.Next(l.Count)];
    }

    public override void RandomForEach(List<int> l)
    {
        int safety = 999;
        while (safety > 0 && l.Count > 0)
        {
            safety--;
        }
    }

    public override List<int> Shuffle(List<int> l)
    {
        List<int> r = new List<int>();
        int safety = 999;
        while (safety > 0 && l.Count > 0)
        {
            safety--;
        }
        return r;
    }

    public override string WeightedRandom(Dictionary<string, float> d)
    {
        //This one requires a lot more lines of code than the others
        foreach (string k in d.Keys)
        {
            return k;
        }
        return "";
    }
}
