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
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 2, Color.red);
        if (Input.GetMouseButtonDown(0) && AllowToClick)
        {
            AllowToClick = false;
            
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
                    Debug.Log(hit.transform.name);
                    if (hit.transform.gameObject.layer == 6) // 6 Ground , 8 OtherPlayer
                    {
                        CmdHit(null, hit.point);
                    }
                    if (hit.transform.gameObject.layer == 8)
                    {
                        Debug.Log("Remove life to " + hit.transform.parent.name);
                        CmdHit(hit.transform.parent.GetComponent<PlayerSetup>().netId, hit.point);

                    }
                }
                
            }
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdHit(string playerId, Vector3 point)
    {
        if (playerId!=null)
        {
            if (true)//MainGame.instance.GameState == 2)
            {
                MainGame.instance.playersHealth[MainGame.instance.playersIdServeur.IndexOf(playerId)] -= 40;
            }
            if (MainGame.instance.playersHealth[MainGame.instance.playersIdServeur.IndexOf(playerId)]<=0)
            {
                Spectator.instance.Players[MainGame.instance.playersIdServeur.IndexOf(playerId)].GetComponent<ActiveTrap>().RpcOnDied();
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
            _blood = Instantiate(particleHitGround, BloodEffectContainer);
        }
        else
        {
            _blood = Instantiate(blood, BloodEffectContainer);
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
