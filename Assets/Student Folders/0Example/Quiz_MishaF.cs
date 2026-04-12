using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Quiz_MishaF : QuizScript
{
    public Quiz_MishaF()
    {
        Author = Authors.MishaF;
    }

    public override List<int> FilterEven(List<int> l)
    {
        List<int> r = new List<int>();
        //Insert Filter Code Here
        return r;
    }
    
    public override int FindBest(List<int> l)
    {
        int r = -999;
        foreach (int n in l)
        {
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
        foreach (string k in d.Keys)
        {
            return k;
        }
        return "";
    }
}
