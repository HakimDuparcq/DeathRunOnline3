using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Fight : NetworkBehaviour
{
    public Slider HealthBar;
    public LayerMask layerBlood;
    public ParticleSystem blood;
    public Camera Camera;
    public Transform BloodEffectContainer;

    public float maxDistanceRay = 2;

    public float TimeBetweenClick = 1;
    public float Compteur=0;
    public bool AllowToClick = true;

    void Start()
    {
        BloodEffectContainer = GameObject.Find("BloodContainer").transform;
    }

    void Update()
    {
        if (Compteur>= TimeBetweenClick )
        {
            AllowToClick = true;
            Compteur = 0;
        }
        else
        {
            Compteur += Time.deltaTime;
        }
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 2, Color.red);
        if (Input.GetMouseButtonDown(0) && AllowToClick)
        {
            AllowToClick = false;
            
            var ray = Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistanceRay, layerBlood))
            {
                
                Debug.Log(hit.transform.name);
                if (hit.transform.gameObject.layer == 6 || hit.transform.gameObject.layer == 7) // 6 Ground , 7 player
                {
                    //Debug.Log("Hit");
                    CmdHit(hit.point);/*
                    ParticleSystem _blood = Instantiate(blood, BloodEffectContainer );
                    _blood.transform.position = hit.point;
                    _blood.Play();*/
                }
                if (hit.transform.gameObject.layer == 7)
                {
                    Debug.Log("Remove life to " + hit.transform.gameObject.name);
                }
            }
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdHit(Vector3 point)
    {
        RpcHit(point);
    }


    [ClientRpc]
    public void RpcHit(Vector3 point)
    {
        ParticleSystem _blood = Instantiate(blood, BloodEffectContainer);
        _blood.transform.position = point;
        _blood.Play();
    }        


    public void CleanSceneEffectBlood()
    {
        for (int i = 0; i < BloodEffectContainer.childCount; i++)
        {
            Destroy(BloodEffectContainer.GetChild(i).gameObject);
        }

    }


}
