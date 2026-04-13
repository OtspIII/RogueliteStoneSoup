using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Quiz_SamsonW : QuizScript
{
    public Quiz_SamsonW()
    {
        Author = Authors.SamsonW;
    }

    public override List<int> FilterEven(List<int> l)
    {
        List<int> r = new List<int>();
        foreach (int n in l)
        {
            if(n % 2 == 0)
                r.Add(n);
        }
        return r;
    }
    
    public override int FindBest(List<int> l)
    {
        int r = -999;
        foreach (int n in l)
        {
            if (n > r)
                r = n;
        }
        return r;
    }

    public override List<int> SortHighToLow(List<int> l)
    {
        List<int> remaining = new List<int>(l); // copy of input
        List<int> r = new List<int>();
        int safety = 999;
        while (safety > 0 && remaining.Count > 0) //Needs an &&
        {
            safety--;

            int highest = int.MinValue;
            foreach(int n in remaining)
            {
                if(n > highest)
                    highest = n;
            }
            r.Add(highest);
            remaining.Remove(highest);
        }
        return r;
    }

    public override int ReturnRandom(List<int> l)
    {
        return l[Random.Range(0, l.Count)];
    }

    public override void RandomForEach(List<int> l)
    {
        List<int> remaining = new List<int>(l); // make a copy
        int safety = 999;
        while (safety > 0 && remaining.Count > 0)
        {
            safety--;

            int randomValue = ReturnRandom(remaining);
            Debug.Log(randomValue);
            remaining.Remove(randomValue);
        }
    }

    public override List<int> Shuffle(List<int> l)
    {
        List<int> remaining = new List<int>(l); // make a copy
        List<int> r = new List<int>();
        int safety = 999;
        while (safety > 0 && remaining.Count > 0)
        {
            safety--;
            
            int randomValue = ReturnRandom(remaining);
            r.Add(randomValue);
            remaining.Remove(randomValue);
        }
        return r;
    }

    public override string WeightedRandom(Dictionary<string, float> d)
    {
        float totalWeight = 0;
        foreach(float weight in d.Values)
            totalWeight += weight;
        
        float randomTargetWeight = Random.Range(0, totalWeight);
        float currWeight = 0;
        foreach (var kvp in d)
        {
            currWeight += kvp.Value;
            
            if(currWeight > randomTargetWeight)
                return kvp.Key; // found the item
        }
        
        return "";
    }
}
