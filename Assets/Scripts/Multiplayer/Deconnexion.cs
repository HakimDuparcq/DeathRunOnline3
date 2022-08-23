using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deconnexion : MonoBehaviour
{
    public static Deconnexion instance;

    public bool wantTodeco = false;
    void Start()
    {
        Application.wantsToQuit += WantsToQuit;
    }

    void Update()
    {
        
    }

    bool WantsToQuit()
    {
        Debug.Log("Player prevented from quitting.");

        if (wantTodeco)
        {
            return true;
        }
        else
        {
            StartCoroutine(GoQuit());
        }

        return false;
    }

    IEnumerator GoQuit()
    {
        Debug.Log("GoQuit");

        
        yield return new WaitForSeconds(2);
        wantTodeco = true;
        Application.Quit();
    }

   

}
