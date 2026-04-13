using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Quiz_QixiangD : QuizScript
{
    public Quiz_QixiangD()
    {
        Author = Authors.QixiangD;
    }

    public override List<int> FilterEven(List<int> l)
    {
        List<int> r = new List<int>();
        foreach (int i in l)
        {
            if (i % 2 == 0)
            {
                r.Add(i);
            }
        }
        return r;
    }
    
    public override int FindBest(List<int> l)
    {
        int r = -999;
        foreach (int n in l)
        {
            if (n >= r)
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
            int value = l[0];
            l.RemoveAt(0);

            int i= 0;
            while(i<r.Count && value <= r[i])
            {
                i++;
            }
            r.Insert(i, value);
                
            
        }
        return r;
    }

    public override int ReturnRandom(List<int> l)
    {
        return l[0];
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
// my computer broke down during class 