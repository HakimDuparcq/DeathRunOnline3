using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PersonaliseCharacter : MonoBehaviour
{
    public static PersonaliseCharacter instance;
    public GameObject PlayerCharacer;

    public Custom Character;
    public Custom PropsLeft;
    public Custom PropsRight;
    public List<int> playersCharacter = new List<int>();
    void Start()
    {
        instance = this;
        PlayerChange(PlayerCharacer);

        Character.actualProp = 1;
        Character.TextNumber.text = Character.actualProp.ToString() + "/" + Character.Props.Length;

        PropsLeft.actualProp = 1;
        PropsLeft.TextNumber.text = PropsLeft.actualProp.ToString() + "/" + PropsLeft.Props.Length;

        PropsRight.actualProp = 1;
        PropsRight.TextNumber.text = PropsRight.actualProp.ToString() + "/" + PropsRight.Props.Length;
        OnLoadPersonnalisation();
    }

    void Update()
    {

    }

    public void OnClickArrowsCharacters(int MagnusOneLeftandOneRight)
    {
        if (Character.actualProp + MagnusOneLeftandOneRight == -1)
        {
            Character.actualProp = Character.Props.Length;
        }
        else
        {
            Character.actualProp = (Character.actualProp + MagnusOneLeftandOneRight) % (Character.Props.Length + 1);
        }
        Character.TextNumber.text = Character.actualProp.ToString() + "/" + Character.Props.Length;
        foreach (GameObject prop in Character.Props)
        {
            prop.SetActive(false);
        }

        if (Character.actualProp - 1>=0)
        {
            Character.Props[Character.actualProp - 1].SetActive(true);
        }
    }


    public void OonClickArrowsCharacters(int MagnusOneLeftandOneRight)
    {
        if (Character.actualProp>1 && Character.actualProp< Character.Props.Length )
        {
            Character.actualProp += MagnusOneLeftandOneRight;
        }
        else if (Character.actualProp == 1 && MagnusOneLeftandOneRight == -1 )
        {
            Character.actualProp = Character.Props.Length;
        }
        else if (Character.actualProp == Character.Props.Length && MagnusOneLeftandOneRight == 1)
        {
            Character.actualProp =1 ;
        }
        else if (Character.actualProp == 1 && MagnusOneLeftandOneRight == 1)
        {
            Character.actualProp += MagnusOneLeftandOneRight;
        }
        else if (Character.actualProp == Character.Props.Length && MagnusOneLeftandOneRight ==-1)
        {
            Character.actualProp += MagnusOneLeftandOneRight;
        }

        Character.TextNumber.text = Character.actualProp.ToString() + "/" + Character.Props.Length;
        foreach (GameObject prop in Character.Props)
        {
            prop.SetActive(false);
        }
        Character.Props[Character.actualProp - 1].SetActive(true);
    }

    public void OnClickArrowsPropsLeft(int MagnusOneLeftandOneRight)
    {
        if (PropsLeft.actualProp + MagnusOneLeftandOneRight == -1)
        {
            PropsLeft.actualProp = PropsLeft.Props.Length;
        }
        else
        {
            PropsLeft.actualProp = (PropsLeft.actualProp + MagnusOneLeftandOneRight) % (PropsLeft.Props.Length + 1);
        }
        PropsLeft.TextNumber.text = PropsLeft.actualProp.ToString() + "/" + PropsLeft.Props.Length;
        foreach (GameObject prop in PropsLeft.Props)
        {
            prop.SetActive(false);
        }

        if (PropsLeft.actualProp - 1 >= 0)
        {
            PropsLeft.Props[PropsLeft.actualProp - 1].SetActive(true);
        }
    }



    public void OnClickArrowsPropsRight(int MagnusOneLeftandOneRight)
    {
        if (PropsRight.actualProp + MagnusOneLeftandOneRight == -1)
        {
            PropsRight.actualProp = PropsRight.Props.Length;
        }
        else
        {
            PropsRight.actualProp = (PropsRight.actualProp + MagnusOneLeftandOneRight) % (PropsRight.Props.Length + 1);
        }
        PropsRight.TextNumber.text = PropsRight.actualProp.ToString() + "/" + PropsRight.Props.Length;
        foreach (GameObject prop in PropsRight.Props)
        {
            prop.SetActive(false);
        }

        if (PropsRight.actualProp - 1 >= 0)
        {
            PropsRight.Props[PropsRight.actualProp - 1].SetActive(true);
        }
    }


    public void OnSavePersonnalisation()
    {
        PlayerPrefs.SetInt("Character", Character.actualProp);
        PlayerPrefs.SetInt("PropLeft", PropsLeft.actualProp);
        PlayerPrefs.SetInt("PropRight", PropsRight.actualProp);
        List<int> Config = new List<int>();/*
        Config.Add(Character.actualProp);
        Config.Add(PropsLeft.actualProp);
        Config.Add(PropsRight.actualProp);*/
        CharacterUpdate(Config);
        Debug.Log("Save");




    }

    public void OnLoadPersonnalisation()
    {
        if (PlayerPrefs.HasKey("Character"))  // Si y a deja eu un enregistrement
        {
            Character.actualProp = PlayerPrefs.GetInt("Character");
            PropsLeft.actualProp = PlayerPrefs.GetInt("PropLeft");
            PropsRight.actualProp = PlayerPrefs.GetInt("PropRight");
            CharacterUpdate(null);
        }
    }


    public void CharacterUpdate(List<int> ConfigCharacter)
    {/*
        if (ConfigCharacter == null)
        {
            playersCharacter = new List<int>();
            playersCharacter.Add(Character.actualProp);
            playersCharacter.Add(PropsLeft.actualProp);
            playersCharacter.Add(PropsRight.actualProp);
            //Debug.Log("ListNull");
        }
        else
        {
            playersCharacter = new List<int>();
            playersCharacter.Add(ConfigCharacter[0]);
            playersCharacter.Add(ConfigCharacter[1]);
            playersCharacter.Add(ConfigCharacter[2]);
        }*/
        playersCharacter = new List<int>();
        playersCharacter.Add(Character.actualProp);
        playersCharacter.Add(PropsLeft.actualProp);
        playersCharacter.Add(PropsRight.actualProp);

        Character.TextNumber.text = Character.actualProp.ToString() + "/" + Character.Props.Length;
        PropsLeft.TextNumber.text = PropsLeft.actualProp.ToString() + "/" + PropsLeft.Props.Length;
        PropsRight.TextNumber.text = PropsRight.actualProp.ToString() + "/" + PropsRight.Props.Length;

        foreach (GameObject prop in Character.Props)
        {
            prop.SetActive(false);
        }
        if (Character.actualProp - 1 >= 0)
        {
            Character.Props[playersCharacter[0] - 1].SetActive(true);
        }

        foreach (GameObject prop in PropsLeft.Props)
        {
            prop.SetActive(false);
        }
        if (PropsLeft.actualProp - 1 >= 0)
        {
            PropsLeft.Props[playersCharacter[1] - 1].SetActive(true);
        }

        foreach (GameObject prop in PropsRight.Props)
        {
            prop.SetActive(false);
        }
        if (PropsRight.actualProp - 1 >= 0)
        {
            PropsRight.Props[playersCharacter[2] - 1].SetActive(true);
        }
    }


    public void PlayerChange(GameObject PlayerCharacer)
    {
        for (int i = 0; i < 12; i++)
        {
            Character.Props[i] = PlayerCharacer.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < 4; i++)
        {
            PropsLeft.Props[i] = PlayerCharacer.transform.GetChild(12).GetChild(0).GetChild(i).gameObject;
        }
        for (int i = 0; i < 4; i++)
        {
            PropsRight.Props[i] = PlayerCharacer.transform.GetChild(12).GetChild(0).GetChild(i+4).gameObject;
        }
    }

    public void PlayerChange()
    {
        for (int i = 0; i < 12; i++)
        {
            Character.Props[i] = PlayerCharacer.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < 4; i++)
        {
            PropsLeft.Props[i] = PlayerCharacer.transform.GetChild(12).GetChild(0).GetChild(i).gameObject;
        }
        for (int i = 0; i < 4; i++)
        {
            PropsRight.Props[i] = PlayerCharacer.transform.GetChild(12).GetChild(0).GetChild(i + 4).gameObject;
        }
    }


    public void DisablePersonnalisationScene()
    {
        Character.TextNumber.gameObject.SetActive(false);
        Character.DirectionalArrowL.gameObject.SetActive(false);
        Character.DirectionalArrowR.gameObject.SetActive(false);

        PropsLeft.TextNumber.gameObject.SetActive(false);
        PropsLeft.DirectionalArrowL.gameObject.SetActive(false);
        PropsLeft.DirectionalArrowR.gameObject.SetActive(false);

        PropsRight.TextNumber.gameObject.SetActive(false);
        PropsRight.DirectionalArrowL.gameObject.SetActive(false);
        PropsRight.DirectionalArrowR.gameObject.SetActive(false);
    }

    public void OnMouseDrag()
    {
        Debug.Log("Drag");
        PlayerCharacer.transform.Rotate(Vector3.down, Input.GetAxis("Mouse X"));
        PlayerCharacer.transform.Rotate(Vector3.right, Input.GetAxis("Mouse Y"));
    }



}


[System.Serializable]
public class Custom
{
    public Button ButtonCategory;
    public TextMeshProUGUI TextNumber;
    public Button DirectionalArrowL;
    public Button DirectionalArrowR;
    public GameObject[] Props;
    public int actualProp;
}
