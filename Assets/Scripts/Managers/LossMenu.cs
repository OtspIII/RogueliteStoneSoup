using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LossMenu : MonoBehaviour
{
    public TextMeshPro InfoTxt;

    private void Start()
    {
        if(God.Session != null)
            InfoTxt.text = God.Session.GameOverInfo;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            God.Session = null;
            SceneManager.LoadScene("Gameplay");
        }
    }
}
