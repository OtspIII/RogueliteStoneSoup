using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Quiz_MichaelT : QuizScript
{
    public Quiz_MichaelT()
    {
        Author = Authors.MichaelT;
    }

    public override List<int> FilterEven(List<int> l)
    {
        List<int> r = new List<int>();
        foreach (int n in l)
        {
            if (n % 2 == 0)
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
        while (safety > 0 && l.Count > 0)
        {
            safety--;
            int best = FindBest(l); 
            r.Add(best);
            l.Remove(best);
        }
        return r;
    }

    public override int ReturnRandom(List<int> l)
    {
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
            r.Add(vaL);
            l.Remove(vaL);
        }
        return r;
    }

    public override string WeightedRandom(Dictionary<string, float> d)
    {
        float totalWeight = 0f;
        foreach (float v in d.Values)
        {
            totalWeight += v;
        }
        float randomWeight = Random.Range(0f, totalWeight);
        float cumulativeWeight = 0f;
        foreach (string k in d.Keys)
        {
            cumulativeWeight += d[k];
            if (randomWeight <= cumulativeWeight)
            {
                return k;
            }
        }   
        return "";
    }
}
