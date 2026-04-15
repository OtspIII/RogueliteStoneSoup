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
        return l[Random.Range(0,l.Count)];
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
        float totat = 0f;
        foreach (string k in d.Keys)
        {
            totat += d[k];
        }

        float r = Random.Range(0f, totat);
        float current = 0f;
        foreach (string k in d.Keys)
        {
            current += d[k];
            if (r <= current)
            {
                return k;
            }
        }
        return "";
    }
}
// my computer broke down during class 