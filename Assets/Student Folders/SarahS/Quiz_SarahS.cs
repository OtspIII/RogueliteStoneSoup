using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Quiz_SarahS : QuizScript
{
    public Quiz_SarahS()
    {
        Author = Authors.SarahS;
    }

    public override List<int> FilterEven(List<int> l)
    {
        List<int> r = new List<int>();
        //Insert Filter Code Here
        foreach (int t in l)
        {
            if (t % 2 == 0)
            {
                r.Add(t);
            }
        }
        return r;
    }
    
    public override int FindBest(List<int> l)
    {
        int r = -999;
        foreach (int n in l)
        {
            //Insert Find Best Code Here
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
        List<int> temp = new List<int>(l);
        int safety = 999;
        while (safety > 0 && temp.Count > 0) //Needs an &&
        {
            safety--;
            //Insert Sort Code Here
            int bestVal = int.MinValue;
            foreach (int t in temp)
            {
                if (t > bestVal)
                {
                    bestVal = t;
                }
            }
            r.Add(bestVal);
            temp.RemoveAt(temp.Count - 1);
        }
        return r;
    }

    public override int ReturnRandom(List<int> l)
    {
        return l[Random.Range(0, l.Count)];
    }

    public override void RandomForEach(List<int> l)
    {
        List<int> temp = new List<int>(l);
        int safety = 999;
        
        while (safety > 0 && temp.Count > 0)
        {
            safety--;
            int randomIdx = Random.Range(0, temp.Count);
            Debug.Log(temp[randomIdx]);
            temp.RemoveAt(temp.Count - 1);
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
