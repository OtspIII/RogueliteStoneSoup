using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

public class Quiz_ElioR : QuizScript
{
    public Quiz_ElioR()
    {
        Author = Authors.ElioR;
    }

    public override List<int> FilterEven(List<int> l)
    {
        List<int> r = new List<int>();
        foreach(int i in l)
        {
            if(l[i-1]%2 == 0)
            {
                r.Add(l[i-1]);
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
            if(l[n - 1]>r)
            {
                r = l[n-1];
            }
        }
        return r;
    }

    public override List<int> SortHighToLow(List<int> l)
    {
        List<int> r = new List<int>();
        int safety = 999;
        while (safety > 0 && l.Count >0) //Needs an &&
        {
            int taget =FindBest(l);
            r.Add(taget);
            l.Remove(taget);
                        

            
            safety--;
            
            //Insert Sort Code Here
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
        }
    }

    public override List<int> Shuffle(List<int> l)
    {
        List<int> r = new List<int>();
        int safety = 999;
        int target = 0;
        while (safety > 0 && l.Count > 0)
        {
            foreach(int n in l)
            {
            target = Random.Range(0,l.Count);
            r.Add(target);
            l.Remove(target);
            }

            safety--;
        }
        return r;
    }

    public override string WeightedRandom(Dictionary<string, float> d)
    {
        //This one requires a lot more lines of code than the others
        List<string> Deck = new List<string>();
        foreach (string k in d.Keys)
        {
            return k;
        }
        return "";
    }
}
