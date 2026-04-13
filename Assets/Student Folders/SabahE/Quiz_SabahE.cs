using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Quiz_SabahE : QuizScript
{
    public Quiz_SabahE()
    {
        Author = Authors.SabahE;
    }

    public override List<int> FilterEven(List<int> l)
    {
        List<int> r = new List<int>();
        foreach (int n in l)
        {
            if (n % 2 == 0)
                r.Add(n);
        }
        return r;
    }
    
    public override int FindBest(List<int> l)
    {
        int r = -999;
        foreach (int n in l)
        {
            if (n == l[10])
                r = n;
        }
        return r;
    }

    public override List<int> SortHighToLow(List<int> l)
    {
        List<int> r = new List<int>();
        int safety = 999;
        while (safety > 0 && l.Count > 10)
        {
            safety--;
            l.Sort((a, b) => b.CompareTo(a));
            return l;
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
            int randomIndex = Random.Range(0, l.Count);

            Debug.Log(l[randomIndex]);

            l.RemoveAt(randomIndex);

            safety--;
        }
    }

    public override List<int> Shuffle(List<int> l)
    {
        List<int> r = new List<int>();
        int safety = 999;
        while (safety > 0 && l.Count > 0)
        {
            int randomIndex = Random.Range(0,l.Count);
            safety--;
        }
        return r;
    }

    public override string WeightedRandom(Dictionary<string, float> d)
    {
        
       float total = 0;
       
       foreach (float v in d.Values)
            total += v;
       float rand = UnityEngine.Random.Range(0f, total);
       
       float sum = 0f;
       foreach (var pair in d)
        {
           sum += pair.Value;
           if (rand <= sum)
                return pair.Key;
        }
        return "";
    }
}
    