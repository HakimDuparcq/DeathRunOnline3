using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class NameUi : MonoBehaviour
{
    private Camera Cam;
    public TextMeshProUGUI NameUI;


    void Start()
    {
        StartCoroutine(GetName());
        Cam = MainGame.instance.LocalPlayer.GetComponent<PlayerReferences>().PlayerCamera.GetComponent<Camera>();
        if (gameObject.GetComponent<PlayerSetup>().isLocalPlayer)
        {
            NameUI.gameObject.SetActive(false);
            this.enabled = false;
        }
    }


    void Update()
    {
        if (Cam!=null && NameUI!=null)
        {
            NameUI.transform.LookAt(NameUI.transform.position + Cam.transform.rotation * new Vector3(0, 0, 1), Cam.transform.rotation * new Vector3(0, 1, 0));

        }
    }



    IEnumerator GetName()
    {
        if (gameObject.name.Contains("(")  || gameObject.name.Contains("["))
        {
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(GetName());
        }
        else
        {
            NameUI.text = gameObject.name;
        }
        

    }


}
