using System.Collections.Generic;
using System.Linq;
using Unity.Multiplayer.PlayMode;
using UnityEngine;
using Random = UnityEngine.Random;

public class Quiz_AdamD : QuizScript
{
    public Quiz_AdamD()
    {
        Author = Authors.AdamD;
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
        //Insert Filter Code Here
        return r;
    }
    
    public override int FindBest(List<int> l)
    {
        int r = -999;
        foreach (int n in l)
        {
            //Insert Find Best Code Here
            int CurrentInt = -1; //i don't need "new" when declaring a new integer here
            if (CurrentInt < n )
            {
                CurrentInt = n;
            }
            r= CurrentInt; //could've just used r but already finished
        }
        return r;
    }

    public override List<int> SortHighToLow(List<int> l)
    {
        List<int> r = new List<int>();
        int safety = 999;
        while (safety > 0 && l.Count>0) //Needs an &&. While statements should always have an exit ramp or Unity will crash.
            //if you modify the list while its running a foreach loop, you'll get an error, unlike a while loop
        {
            
            safety--; //makes it run 999 times, which is nothing to a computer
                      //Insert Sort Code Here
            int CurrentInt=-1; 
            foreach (int n in l)
            {
                if (CurrentInt < n)
                {
                    CurrentInt = n;
                }
            }
            r.Add(CurrentInt);
            l.Remove(CurrentInt);
            //you can also have it compute each 2 sets of numbers to switch places if higher, then run this multiple times until its all sorted
            //there's also a sort function but not allowed to use it here
        }
        return r;
        //l.ToArray will make a copy of a list that you can change
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
            safety--;/*
            l.Add[Random.Range(0, l.Count)];*/
            //should scramble the list's order in the debug log
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

        //EC: if you can feed it a 1.5, where it's 50% more likely than 1, you can get EC
    }
}
