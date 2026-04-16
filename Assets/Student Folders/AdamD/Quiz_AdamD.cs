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
            //i don't need "new" when declaring a new integer here btw
            if (r < n )
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
        int r = l[Random.Range(0, l.Count)]; //do not know why its printing multiple numbers
        return r; //done in class
    }

    public override void RandomForEach(List<int> l)
    {

        List<int> r = new List<int>();
        int safety = 999;
        while (safety > 0 && l.Count > 0)
        {
            safety--;
            int RandomInt = l[Random.Range(0, l.Count)]; ;
            r.Add(RandomInt);
            l.Remove(RandomInt);
            
            //should scramble the list's order in the debug log
        }
        Debug.Log(string.Join(",", r)); //should print out the stuff in a list, inserting a comma between. Without it, it won't show, showing only some weird unity msg. 
 
    }

    public override List<int> Shuffle(List<int> l)
    {
        List<int> r = new List<int>();
        int safety = 999;
        while (safety > 0 && l.Count > 0)
        {
            safety--;
            int RandomInt= l[Random.Range(0, l.Count)]; ;
            r.Add(RandomInt);
            l.Remove(RandomInt);
        }
        return r;
        //should be done..?
    }

    public override string WeightedRandom(Dictionary<string, float> d)
    {
        //This one requires a lot more lines of code than the others

        List<float> r = new List<float>(); //empty copy of an list, to be used later
        string String="If you can read this the code did not work :(";
        foreach (string k in d.Keys)
        {
            for (float i=0; i < d[k]; i++)
            {
                r.Add(d[k]); //adds the number to a list each times it runs. However, floats will "add an extra" iteration. need to fix later
            }   
            //x is set to whatever is tied to the string value k in the dictionary called d, which should be a number. 
            //return k; //returns the string key. dont know what to use it for yet though
        }//currently have a list of numbers so...i gotta select one out of the list of numbers
        float Numb = r[Random.Range(0,r.Count)]; //selects a random float between the list of floats in list r. floats will be included 1 more than usual, unfortunately

        foreach (string k in d.Keys)
        {
            if (Numb== d[k]) { String = k; break; } //if Numb is equal to the current key's definition, set String to be called the key's string, then exit the loops
        }
        //this is currently a float rn, need to change this to a string. currently this gives me a random number from a list of NUMBERSR
        return String;

        //EC: if you can feed it a 1.5, where it's 50% more likely than 1, you can get EC

        //EC: instead of adding a list of floats to a list, add a list of dictionaries to that list..?
    }
}
