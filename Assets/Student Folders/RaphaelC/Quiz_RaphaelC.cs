using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Quiz_RaphaelC : QuizScript
{
    public Quiz_RaphaelC()
    {
        Author = Authors.RaphaelC;
    }

    public override List<int> FilterEven(List<int> l)
    {
        List<int> r = new List<int>();
        //Insert Filter Code Here

        foreach (int n in l) 
        {
            if (n % 2 <= 0)
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
            //Insert Sort Code Here

            int highest = int.MinValue;
            foreach (int n in l)
            {
                if (n > highest)
                {
                    highest = n;
                }
            }

            r.Add(highest);
            l.Remove(highest);
        }
        return r;
    }

    public override int ReturnRandom(List<int> l)
    {
        return l[Random.Range(0, l.Count)];
    }

    public override void RandomForEach(List<int> l)
    {
        int safety = 999;
        while (safety > 0 && l.Count > 0)
        {
            safety--;
            //foreach (r in l.Count)
            //{

            //}
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
