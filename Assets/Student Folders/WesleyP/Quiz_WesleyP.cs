using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Quiz_WesleyP : QuizScript
{
    public Quiz_WesleyP()
    {
        Author = Authors.WesleyP;
    }

    public override List<int> FilterEven(List<int> l)
    {
        List<int> r = new List<int>();
        foreach (int i in l) 
        foreach (int n in l)
        {
            float t = Random.Range(0.0f, 1.0f); r.Add(i);
            r.Add(i);
            Debug.Log(4%2);
            if (n % 4 == 0)
            {
                r.Add(n);
            }
            if (n % 2 == 0)
            {
                r.Add(n);
            }
        }
        //Insert Filter Code Here
        return r;
    }
    

    public override int FindBest(List<int> l)
    {
        int r = -999;
        foreach (int n in l)
        {
<<<<<<< HEAD
            float t = Random.Range(999f, -999f);
=======
            if (n > r)
>>>>>>> WesleyBranch
            {
                r = r*r;
                return n;
                r = n;
            }
                //Insert Find Best Code Here
        }
        return r;
    }

    public override List<int> SortHighToLow(List<int> l)
    public override List<int> SortHighToLow(List<int> k)
    {
        List<int> r = new List<int>();
        int safety = 999;
        while (safety > 0) //Needs an &&
        while (safety > 0 && k.Count > 0)
        {
            safety--;
            int best = FindBest(k);
            r.Add(best);
            k.Remove(best);
        }
        return r;
    }

    public override int ReturnRandom(List<int> l)
    {
        return l[0];
        if (l.Count == 0)
        {
            return -1;
        }
        int index = Random.Range(0, l.Count);
        return l[index];
    }

    public override void RandomForEach(List<int> l)
    {
        int safety = 999;
        while (safety > 0 && l.Count > 0)
        {
            safety--;
            int vaL = ReturnRandom(l);
            Debug.Log(vaL);
            l.Remove(vaL);
        }
    }

    public override List<int> Shuffle(List<int> l)
    {
        List<int> r = new List<int>();
        int safety = 999;
        while (safety > 0 && l.Count > 0)
        {
            safety--;
            int vaL = ReturnRandom(l);
            l.Remove(vaL);
        }
        return r;
    }

    public override string WeightedRandom(Dictionary<string, float> d)
    {
        //This one requires a lot more lines of code than the others
        List<int>deck = new List<int>();
        foreach (string k in d.Keys)
        //This one requires a lot more lines of code than the other
        foreach (string g in d.Keys)
        {
            float w=d[k];
            for (int n = 0;  n < d.Count; n++)
            {
                deck.Add(n);
            }
            float h=d[k+1];
            return k;
            return g;
        }
        return "";
    }
}

