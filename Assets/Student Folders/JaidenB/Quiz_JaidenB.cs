using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Quiz_JaidenB : QuizScript
{
    public Quiz_JaidenB()
    {
        Author = Authors.JaidenB;
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
            if (n > r) 
            {
                r = n; // Replacing the r interger with the value from the list
            }
        }
        return r;
    }

    public override List<int> SortHighToLow(List<int> l)
    {
        List<int> r = new List<int>();
        int safety = 999;
        
        while (safety > 0 && l.Count > 0) //Needs an && || just used the && from the future questions
        {
            int SortCount = FindBest(l); // Calling the previous function
            safety--;
            //Insert Sort Code Here

            r.Add(SortCount);
            l.Remove(SortCount);
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
            //int RandomCount = l[Random.Range(0, l.Count)];
            //Debug.Log(RandomCount);
            //l.Remove(RandomCount);
        }
    }

    public override List<int> Shuffle(List<int> l)
    {
        List<int> r = new List<int>();
        int safety = 999;
        while (safety > 0 && l.Count > 0)
        {
            safety--;

            int RandomCount = l[Random.Range(0, l.Count)];

            r.Add(RandomCount);
            l.Remove(RandomCount);
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
