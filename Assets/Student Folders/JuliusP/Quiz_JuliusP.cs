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
                
              //ADDS THE EVEN NUMBERS TO THE LIST//

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

   
        while (safety > 0 && l.Count > 0) //Needs an &&
        {
            safety--;
            //Insert Sort Code Here

            int Max = l[0];

            int MaxIndex = 0;

           
            for(int i = 0; i<l.Count; i++)
            {
                

              //START WITH FIRST NUMBER FOR COMPARING//
               int max = l[0];


               if(l[i] > Max)
                {
                    
                    //SET THE MAX TO BE THE CURRENT MAX//
                    l[i] = Max;

                    //SAVE THE INDXE OF WHERE THE MAX NUMBER IS IN//
                    MaxIndex = i;





                }

            }


        //ADD THE MAX VALUE TO THE LIST//
        r.Add(Max);


        //REMOVE THE VALUE FROM THE L LIST OF WHERE THE MAX VALUE APPEARED//
        l.RemoveAt(MaxIndex);

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

            int RandomNum = Random.Range(0, l.Count);

            Debug.Log(l[RandomNum]);

            l.RemoveAt(RandomNum);





        }
    }

    public override List<int> Shuffle(List<int> l)
    {
        List<int> r = new List<int>();
       
        int safety = 999;

        while (safety > 0 && l.Count > 0)
        {
            safety--;

            int RandIndex = Random.Range(0, l.Count);
       
            r.Add(l[RandIndex]);
        
            l.RemoveAt(RandIndex);
        }
        return r;
    }

    public override string WeightedRandom(Dictionary<string, float> d)
    {
        List<string> WeightedList= new List<string>();
        //This one requires a lot more lines of code than the others
        foreach (string k in d.Keys)
        {

           float v = d[k];


            for(int i = 0; i < v; i++)
            {
                

                WeightedList.Add(k);



            }
           
        
        }


         int index = Random.Range(0, WeightedList.Count);
         return WeightedList[index];
    
    }
}
