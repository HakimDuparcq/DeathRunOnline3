using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Fight : NetworkBehaviour
{
    public Slider HealthBar;
    public LayerMask layerBlood;
    public LayerMask layerOtherPlayer;
    public ParticleSystem blood;
    public ParticleSystem particleHitGround;
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
        if (!isLocalPlayer)
        {
            return; 
        }
        if (Compteur>= TimeBetweenClick )
        {
            AllowToClick = true;
            Compteur = 0;
        }
        else
        {
            Compteur += Time.deltaTime;
        }

        var rayy = Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitt;
        //gameObject.GetComponent<CrossHairs>().CrossHairsActivationColor(false);
        if (Physics.Raycast(rayy, out hitt, maxDistanceRay, layerOtherPlayer))
        {
            if (hitt.transform.gameObject.layer == 8) // 6 Ground , 8 OtherPlayer
            {
                gameObject.GetComponent<CrossHairs>().CrossHairsActivationColor(true);

            }
        }
        else
        {
            gameObject.GetComponent<CrossHairs>().CrossHairsActivationColor(false);
        }


        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 2, Color.red);
        if (Input.GetMouseButtonDown(0) && AllowToClick)
        {
            AllowToClick = false;
            Compteur = 0; 
            var ray = Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistanceRay, layerBlood))
            {
                /*if (false)//hit.transform.gameObject == this.gameObject || hit.transform.parent.gameObject == this.gameObject )
                {
                    //Debug.Log("return");
                    return;
                }*/
                if (MainGame.instance.playersIsAliveServer[MainGame.instance.playersIdServeur.IndexOf(MainGame.instance.LocalPlayerId)])
                {
                    //Debug.Log(hit.transform.name);
                    if (hit.transform.gameObject.layer == 6) // 6 Ground , 8 OtherPlayer
                    {
                        CmdHit(100, hit.point);
                    }
                    if (hit.transform.gameObject.layer == 8)
                    {
                        //Debug.Log("Remove life to " + hit.transform.parent.name);
                        hit.transform.parent.GetComponent<Fight>().CmdHit(hit.transform.parent.GetComponent<NetworkIdentity>().netId, hit.point);

                    }
                }
                
            }
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdHit(uint playerId, Vector3 point)
    {
        if (playerId!=100)
        {
            if (MainGame.instance.GameState == 2)
            {
                MainGame.instance.playersHealth[MainGame.instance.playersIdServeur.IndexOf(playerId)] -= 40;
            }
            if (MainGame.instance.playersHealth[MainGame.instance.playersIdServeur.IndexOf(playerId)]<=0)
            {
                //Spectator.instance.Players[MainGame.instance.playersIdServeur.IndexOf(playerId)].GetComponent<ActiveTrap>().RpcOnDied();
                MainGame.instance.playersIsAliveServer[MainGame.instance.playersIdServeur.IndexOf(playerId)] = false;
                gameObject.GetComponent<ActiveTrap>().RpcOnDied();
                gameObject.GetComponent<ActiveTrap>().RpcOnDiedSound();
            }
            RpcHit(point, false );
        }
        else
        {
            RpcHit(point, true);
        }
    }


    [ClientRpc]
    public void RpcHit(Vector3 point, bool isGround)
    {
        ParticleSystem _blood = null;
        if (isGround)
        {
            _blood = Instantiate(particleHitGround, BloodEffectContainer);  // particle
            Debug.Log("HitGround" + gameObject);
            gameObject.GetComponent<PlayerReferences>().Audio.GetComponent<AudioPlayerManager>().Play("HitGround");  //Audio
        }
        else
        {
            _blood = Instantiate(blood, BloodEffectContainer);
            gameObject.GetComponent<PlayerReferences>().Audio.GetComponent<AudioPlayerManager>().Play("HitBody");
        }
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
