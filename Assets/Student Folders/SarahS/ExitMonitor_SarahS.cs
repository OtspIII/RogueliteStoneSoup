using UnityEngine;

public class ExitMonitor_SarahS : MonoBehaviour
{
    public GameObject levelExit;
    public int currentLevel = 1;

    public GameObject keyObject;
    public int initialDotCount = 0;
    public bool keyRevealed = false;
    public int dotTriggerThreshold = 15;
    
    private bool isInitialized = false;

    void Start()
    {
        if (levelExit != null) levelExit.SetActive(false);
        
        InvokeRepeating(nameof(MonitorLoop), 0.5f, 0.5f);
    }

    int CountActiveDots()
    {
        int count = 0;
        foreach (Transform obj in FindObjectsOfType<Transform>())
        {
            if (obj.name == "Dot" || obj.name == "Dot(Clone)") 
            {
                count++;
            }
        }
        return count;
    }

    void MonitorLoop()
    {
        int currentDots = CountActiveDots();
        
        if (!isInitialized)
        {
            if (currentDots > 0)
            {
                initialDotCount = currentDots;
                isInitialized = true;
                Debug.Log("dots spawned: " + initialDotCount);

                if (currentLevel >= 2)
                {
                    keyObject = GameObject.Find("Key");
                    if (keyObject == null) keyObject = GameObject.Find("Key(Clone)");
                    if (keyObject != null) keyObject.SetActive(false);
                }
            }
            
            return; 
        }
        
        if (currentLevel == 1)
        {
            if (currentDots == 0) 
            {
                UnlockExit();
            }
        }
        else if (currentLevel >= 2)
        {
            if (!keyRevealed)
            {
                int dotsGathered = initialDotCount - currentDots;

                if (dotsGathered >= dotTriggerThreshold || currentDots == 0)
                {
                    if (keyObject != null)
                    {
                        keyObject.SetActive(true);
                        keyRevealed = true;
                        
                    }
                    else
                    {
                        UnlockExit();
                    }
                }
            }
            else
            {
                if (keyObject == null)
                {
                    UnlockExit();
                }
            }
        }
    }

    void UnlockExit()
    {
        if (levelExit != null) levelExit.SetActive(true);
       
        CancelInvoke(nameof(MonitorLoop));
    }
}