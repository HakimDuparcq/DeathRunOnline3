using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetupTrap : MonoBehaviour
{
    public static SetupTrap instance;

    [SerializeField]
    public TrapClass[] Traps;

    public TextMeshProUGUI HelpText;



    void Awake()
    {
        if (instance == null)
        {
            //DontDestroyOnLoad(gameObject);
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
            if (Traps[i].TrapType == TrapType.Fall)
            {
                Traps[i].Trap.GetComponent<Rigidbody>().isKinematic = true;   // reset chandelier 
                Traps[i].Trap.transform.localPosition = new Vector3(5.9f, 7.2613f, -46.7287f);
            }

            if (Traps[i].TrapType == TrapType.Stand)
            {
                
            }

            Traps[i].isActivable = true;
            Traps[i].Trigger.transform.GetChild(0).gameObject.SetActive(true);
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
