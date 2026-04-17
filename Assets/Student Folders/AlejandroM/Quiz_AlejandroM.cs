using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Quiz_AlejandroM : QuizScript
{
    public Quiz_AlejandroM()
    {
        Author = Authors.AlejandroM;
    }

    public override List<int> FilterEven(List<int> l)
    {
        List<int> r = new List<int>();
        //Insert Filter Code Here



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
            //Insert Find Best Code Here
            {
                if (n > r)
                {
                    r = n;
                }
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

            int best = -999;

            foreach (int n in l)
            {
                if (n > best)
                {
                    best = n;
                }
            }

            r.Add(best);
            l.Remove(best);
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

            int index = Random.Range(0, l.Count);
            Debug.Log(l[index]);
            l.RemoveAt(index);
        }
    }

    public override List<int> Shuffle(List<int> l)
    {
        List<int> r = new List<int>();
        int safety = 999;
        while (safety > 0 && l.Count > 0)
        {
            safety--;

            int index = Random.Range(0, l.Count);
            r.Add(l[index]); 
            l.RemoveAt(index);
        }
        return r;
    }

    public override string WeightedRandom(Dictionary<string, float> d)
    {
        //This one requires a lot more lines of code than the others

        float total = 0f;
        foreach (float v in d.Values)
        {
            total += v;
        }

        float rand = Random.Range(0f, total);

        float current = 0f;

        foreach (string k in d.Keys)
        {
            current += d[k];

            if (rand < current)
            {
                return k;
            }
        }
        return "";
    }
}
