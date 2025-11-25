using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        ThingController tc = other.gameObject.GetComponent<ThingController>();
        if (tc != null && tc.Stats.Name == "Player")
        {
            Debug.Log("TEMPORARY EXIT: Please Replace");
            SceneManager.LoadScene("YouWin");
        }
    }
}
