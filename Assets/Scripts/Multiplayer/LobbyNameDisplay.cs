using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class LobbyNameDisplay : MonoBehaviour
{
    public Text[] DisplayNames = new Text[3];
    public GameObject Role;
    public Text InputName;

    public GameObject StartButton;



    public void Start()

    {
        MainGame.instance = GameObject.Find("MainGame").GetComponent<MainGame>();
        /*
        for (int i = 0; i < DisplayNames.Length; i++)
        {
            DisplayNames[i].gameObject.SetActive(true);
        }*/
    }

    void Update()
    {
        if (MainGame.instance!=null)
        {
            for (int i = 0; i < DisplayNames.Length; i++)
            {
                DisplayNames[i].gameObject.SetActive(true);
            }
            Role.SetActive(true);

            if (!MainGame.instance.GameOnServer ) //tant que la game n'a pas commencé   affiche les noms
            {
                for (int i = 0; i < MainGame.instance.playersNameServeur.Count; i++)
                {
                    DisplayNames[i].text = MainGame.instance.playersNameServeur[i];
                }
                for (int i = MainGame.instance.playersNameServeur.Count; i < DisplayNames.Length; i++)
                {
                    DisplayNames[i].text = "Waiting for players..";
                }
                //Debug.Log("ok");
            }
        }
        else
        {
            for (int i = 0; i < DisplayNames.Length; i++)
            {
                DisplayNames[i].gameObject.SetActive(false);
            }
            //StartButton.transform.position = new Vector3(StartButton.transform.position.x - 500, StartButton.transform.position.y, StartButton.transform.position.z);
            //StartButton.SetActive(false);


        }

        if (MainGame.instance.GameOnServer)
        {
            for (int i = 0; i < DisplayNames.Length; i++)
            {
                DisplayNames[i].gameObject.SetActive(false);
                Role.SetActive(false);
                StartButton.SetActive(false);
            }
        }



    }

    private void OnDisable()
    {
        for (int i = 0; i < DisplayNames.Length; i++)
        {
            DisplayNames[i].gameObject.SetActive(false);
        }
        //StartButton.SetActive(false);

    }


}
