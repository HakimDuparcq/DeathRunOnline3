using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetupTrap : MonoBehaviour
{
    public static SetupTrap instance;

    [SerializeField]
    public TrapClass[] Traps;
    public TrapClass[] SauvegardeTrap;

    public TextMeshProUGUI HelpText;



    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    public void Start()
    {
        DisableDeathZone();
        
        HelpText.gameObject.SetActive(false);
    }
    
    public void ResetTraps()
    {
        for (int i = 0; i < Traps.Length; i++)
        {
            Traps[i].isActivable = true;
        }
    }

    public void DisableDeathZone()
    {
        for (int i = 0; i < Traps.Length; i++)
        {
            for (int ii = 0; ii < Traps[i].DeathZone.Length; ii++)
            {
                Traps[i].DeathZone[ii].SetActive(false);
            }
        }
    }

}
