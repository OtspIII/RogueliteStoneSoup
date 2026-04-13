using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Quiz_JuliusP : QuizScript
{
    public Quiz_JuliusP()
    {
        Author = Authors.JuliusP;
    }

    public override List<int> FilterEven(List<int> l)
    {
        List<int> r = new List<int>();
        //Insert Filter Code Here

        foreach(int num in l)
        {
            
            if(num % 2 == 0)
            {
                
              r.Add(num); 


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

            if(n > r)
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
        while (safety > 0) //Needs an &&
        {
            safety--;
            //Insert Sort Code Here
        }
        return r;
    }

    public override int ReturnRandom(List<int> l)
    {
       
        
        return Random.Range(0, l.Count);


    
    }

    public override void RandomForEach(List<int> l)
    {
        int safety = 999;
        while (safety > 0 && l.Count > 0)
        {
            safety--;

            
            int RandomIndex = Random.Range(0, l.Count);

            int value = l[RandomIndex];

            Debug.Log(value);

            l.RemoveAt(RandomIndex);





        }
    }

    public override List<int> Shuffle(List<int> l)
    {
        List<int> r = new List<int>();
       
        int safety = 999;

        while (safety > 0 && l.Count > 0)
        {
            safety--;

            //int index = rand.Next(l.Count);
       
            //r.Add(l[index]);
        
            //l.RemoveAt(index);
        }
        return r;
    }

    public override string WeightedRandom(Dictionary<string, float> d)
    {
        //This one requires a lot more lines of code than the others
        foreach (string k in d.Keys)
        {

           // float v = k[s];


            //for(int i = 0; i < v; i++)
            {
                




            }
            return k;
        }
        return "";
    }
}
