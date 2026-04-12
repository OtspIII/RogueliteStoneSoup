using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    public QuizType Type;
    public Authors Author;
    public Dictionary<Authors, QuizScript> Quizzes = new Dictionary<Authors, QuizScript>();

    public List<string> OutputText = new List<string>();
    
    public TextMeshPro Text;

    void Start()
    {
        Quizzes.Add(Authors.MishaF,new Quiz_MishaF());
        Quizzes.Add(Authors.AdamD,new Quiz_AdamD());
        Quizzes.Add(Authors.AlejandroM,new Quiz_AlejandroM());
        Quizzes.Add(Authors.ElioR,new Quiz_ElioR());
        Quizzes.Add(Authors.JaidenB,new Quiz_JaidenB());
        Quizzes.Add(Authors.JuliusP,new Quiz_JuliusP());
        Quizzes.Add(Authors.MichaelT,new Quiz_MichaelT());
        Quizzes.Add(Authors.QixiangD,new Quiz_QixiangD());
        Quizzes.Add(Authors.RaphaelC,new Quiz_RaphaelC());
        Quizzes.Add(Authors.SabahE,new Quiz_SabahE());
        Quizzes.Add(Authors.SamsonW,new Quiz_SamsonW());
        Quizzes.Add(Authors.SarahS,new Quiz_SarahS());
        Quizzes.Add(Authors.TracyH,new Quiz_TracyH());
        Quizzes.Add(Authors.WesleyP,new Quiz_WesleyP());
        Quizzes.Add(Authors.YuChen,new Quiz_YuChen());
        
        RunQuiz(Type,Author);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            RunQuiz(Type,Author);
    }

    public void RunQuiz(QuizType t,Authors a,bool clear=true)
    {
        if(clear) OutputText.Clear();
        List<int> input = new List<int>() { 7,10,3,1,9,5,6,2,4,8 };
        switch (t)
        {
            case QuizType.RunQuiz:
            {
                RunQuiz(QuizType.Filter,a,false);
                Output("---");
                RunQuiz(QuizType.FindBest,a,false);
                Output("---");
                RunQuiz(QuizType.Sort,a,false);
                Output("---");
                RunQuiz(QuizType.Random,a,false);
                Output("---");
                RunQuiz(QuizType.RandomOrder,a,false);
                Output("---");
                RunQuiz(QuizType.Shuffle,a,false);
                Output("---");
                RunQuiz(QuizType.WRandom,a,false);
                break;
            }
            case QuizType.Filter:
            {
                List<int> answer = Quizzes[a].FilterEven(input);
                Output(a + " AUDIT FILTER: " + (AuditFilterEven(answer) ? "SUCCESS" : "FAILURE"));
                Describe(answer);
                break;
            }
            case QuizType.FindBest:
            {
                int answer = Quizzes[a].FindBest(input);
                Output(a + " AUDIT FIND BEST: " + (AuditFindBest(answer,input) ? "SUCCESS" : "FAILURE"));
                Output("Best: " + answer);
                Describe(input);
                break;
            }
            case QuizType.Sort:
            {
                
                List<int> answer = Quizzes[a].SortHighToLow(input);
                Output(a + " AUDIT SORT: " + (AuditSort(answer) ? "SUCCESS" : "FAILURE"));
                Describe(answer);
                break;
            }
            case QuizType.Random:
            {
                string answer = "";
                for (int n = 0; n < 10; n++)
                {
                    if (answer != "") answer += ", ";
                    answer += Quizzes[a].ReturnRandom(input);
                }
                Output(a + " Random: " + answer);
                break;
            }
            case QuizType.RandomOrder:
            {
                
                Quizzes[a].RandomForEach(input);
                Output(a + " Random Order: See Console");
                break;
            }
            case QuizType.Shuffle:
            {
                
                List<int> answer = Quizzes[a].Shuffle(input);
                Output(a + " SHUFFLE");
                Describe(answer);
                break;
            }
            case QuizType.WRandom:
            {
                Dictionary<string, float> inputD = new Dictionary<string, float>();
                inputD.Add("One",1);
                inputD.Add("Two",2);
                inputD.Add("Three",3);
                inputD.Add("Four",4);
                string answer = "";
                for (int n = 0; n < 10; n++)
                {
                    if (answer != "") answer += ", ";
                    answer += Quizzes[a].WeightedRandom(inputD);
                }
                Output(a + " WRandom: " + answer);
                break;
            }
        }

        Text.text = "";
        foreach (string str in OutputText) Text.text += str + "\n";
    }

    public void Output(string o)
    {
        OutputText.Add(o);
    }

    public void Describe<T>(List<T> l)
    {
        string txt = "";
        foreach (T o in l)
        {
            if (txt != "") txt += ", ";
            txt += o.ToString();
        }
        Output(txt);
    }

    public bool AuditFilterEven(List<int> l)
    {
        if (l.Count == 0) return false;
        foreach(int n in l)
            if (n % 2 != 0)
            {
                Output("NOT EVEN: " + n);
                return false;
            }
        return true;
    }
    public bool AuditFindBest(int ans,List<int> l)
    {
        foreach(int n in l)
            if (n > ans)
            {
                Output("NOT BIGGEST: " + ans + " < " + n);
                return false;
            }
        return true;
    }
    
    public bool AuditSort(List<int> l)
    {
        if (l.Count == 0) return false;
        for (int n = 0; n < l.Count - 1; n++)
        {
            if (l[n] < l[n + 1])
            {
                Output("NOT IN ORDER: " + l[n] + " < " + l[n+1]);
                return false;
            }
        }
        return true;
    }

    public int GetHP(ThingInfo o)
    {
        //If they don't have the health trait, their HP is 0
        if (!o.Has(Traits.Health)) return 0;
        //Get their main number from their health trait
        int r = o.Get(Traits.Health).GetInt();
        return r;
    }

    public bool IsVowel(char txt)
    {
        if (txt == 'a' || txt == 'A' || txt == 'e' || txt == 'E' 
            || txt == 'i' || txt == 'i' || txt == 'o' || txt == 'O' 
            || txt == 'U' || txt == 'U') return true;
        return false;
    }

    public ThingInfo GetMostHP(List<ThingInfo> i)
    {
        int best = 0;
        ThingInfo r = null;
        foreach (ThingInfo ti in i)
        {
            int value = GetHP(ti);
            if (value > best)
            {
                best = value;
                r = ti;
            }
        }
        return r;
    }
    
}

public class QuizScript
{
    public Authors Author;

    public QuizScript() { }
    
    public virtual List<int> FilterEven(List<int> l)
    {
        return l;
    }

    public virtual int FindBest(List<int> l)
    {
        return 0;
    }
    
    public virtual List<int> SortHighToLow(List<int> l)
    {
        return l;
    }
    
    public virtual int ReturnRandom(List<int> l)
    {
        return l[0];
    }

    public virtual void RandomForEach(List<int> l)
    {
        
    }
    
    public virtual List<int> Shuffle(List<int> l)
    {
        return l;
    }
    
    public virtual string WeightedRandom(Dictionary<string,float> d)
    {
        return "";
    }
}

public enum QuizType
{
    None=0,
    Filter=1,
    FindBest=2,
    Sort=3,
    Random=4,
    RandomOrder=5,
    Shuffle=6,
    WRandom=7,
    RunQuiz=10,
    TotalQuiz=11,
    
}