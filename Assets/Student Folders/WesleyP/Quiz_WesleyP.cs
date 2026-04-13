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
        {
            float t = Random.Range(0.0f, 1.0f); r.Add(i);
            r.Add(i);
            Debug.Log(4%2);
            r.Add(5%2);
        }
        //Insert Filter Code Here
        return r;
    }
    
    public override int FindBest(List<int> l)
    {
        int r = -999;
        foreach (int n in l)
        {
        {
            float t = Random.Range(999f, -999f);
            {
                r = r*r;
                return n;
            }
                //Insert Find Best Code Here
        }
        return r;
    }

    public override List<int> SortHighToLow(List<int> l)
    {
        List<int> r = new List<int>();
        int safety = 999;
        while (safety > 0) //Needs an &&
        {
            safety--;
            //Insert Sort Code Here
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
        List<int>deck = new List<int>();
        foreach (string k in d.Keys)
        {
            float w=d[k];
            for (int n = 0;  n < d.Count; n++)
            {
                deck.Add(n);
            }
            float h=d[k+1];
            return k;
        }
        return "";
    }
}
