using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ActiveTrap : NetworkBehaviour
{

    //public TrapClass[] Trapss;
    

    public int isInNumber =-1; // 0 for nothing



    void Start()
    {
        //Trapss = SetupTrap.instance.Traps;
    }

    void Update()
    {
        if (Input.GetButtonDown("ActiveTrap") && isInNumber!=-1)
        {
            CmdActiveTrapNumber(isInNumber);
        }

    }


    [Command(requiresAuthority = false)]
    public void CmdActiveTrapNumber(int isInNumber)
    {
        RpcActiveTrapNumber(isInNumber);
    }


    [ClientRpc]
    public void RpcActiveTrapNumber(int isInNumber)
    {
        if (SetupTrap.instance.Traps[isInNumber].isActivable)
        {
            StartCoroutine(Spawning(SetupTrap.instance.Traps[isInNumber]));
            SetupTrap.instance.Traps[isInNumber].isActivable = false;
            SetupTrap.instance.Traps[isInNumber].Trigger.transform.GetChild(0).gameObject.SetActive(false); // Disable light
            SetupTrap.instance.HelpText.gameObject.SetActive(false);
        }
    }

    public IEnumerator Spawning(TrapClass trap)
    {
        ParticleSystem[] trapParticles = new ParticleSystem[trap.positions.Length];
        yield return new WaitForSeconds(trap.TimeBeforeSpawn); //Delay before apararing

        if (trap.TrapType == TrapType.Particle)
        {
            int i = 0;
            foreach (Transform pos in trap.positions)
            {
                trapParticles[i] = Instantiate(trap.particles, pos);
                trapParticles[i].Play();
                i++;
            }
        }
        else if (trap.TrapType == TrapType.Fall)
        {
            trap.Trap.GetComponent<Rigidbody>().isKinematic = false;
        }
        else if (trap.TrapType == TrapType.Stand)//trap.Trap.GetComponent<Animator>())
        {
            trap.Trap.GetComponent<Animator>()?.SetBool("up", true);
            trap.Trap.GetComponent<Animator>()?.SetBool("down", false);
        }

        foreach (GameObject DeadZone in trap.DeathZone)
        {
            DeadZone.SetActive(true);
            trap.AudioSource.Play();
        }
        yield return new WaitForSeconds(trap.TimeToStopAnimation);

        if (trap.TrapType == TrapType.Particle)
        {
            foreach (ParticleSystem particle in trapParticles)
            {
                particle.Pause();
            }
        }
        
        yield return new WaitForSeconds(trap.TimeParticleVisible);

        if (trap.TrapType == TrapType.Particle)
        {
            foreach (ParticleSystem particle in trapParticles)
            {
                particle.Play();
            }
        }
        else if (trap.TrapType == TrapType.Stand)//trap.Trap.GetComponent<Animator>())
        {
            trap.Trap.GetComponent<Animator>()?.SetBool("up", false);
            trap.Trap.GetComponent<Animator>()?.SetBool("down", true);
        }

        yield return new WaitForSeconds(trap.TimeParticleDisappear);

        if (trap.TrapType == TrapType.Particle)
        {
            foreach (ParticleSystem particle in trapParticles)
            {
                particle.Stop();
                Destroy(particle);
            }
        }
       
        foreach (GameObject DeadZone in trap.DeathZone)
        {
            DeadZone.SetActive(false);
            Debug.Log("inactive");
        }

    }

    [ClientRpc]
    public void RpcOnDied()
    {
        if (isLocalPlayer)
        {
            ChatBehaviour.instance.CmdSendMessage("Died in Combat", MainGame.instance.LocalPlayerName, 100);
            gameObject.GetComponent<Animator>().SetBool("death", true);
            StartCoroutine(Spectator.instance.ActiveSpectatorMode());
            gameObject.GetComponent<NewPlayerMovement>().canMove = false;
            gameObject.GetComponent<MouseLook>().canRotate = false;
            gameObject.GetComponent<PlayerReferences>().Character.SetActive(true);
            gameObject.GetComponent<PlayerReferences>().Capsule.SetActive(false);


        }
    }

    [Command(requiresAuthority = false)]
    public void CmdSendDied(uint id)
    {
        MainGame.instance.playersIsAliveServer[MainGame.instance.playersIdServeur.IndexOf(id)] = false;
        RpcOnDied();
        RpcOnDiedSound();
    }

    [ClientRpc]
    public void RpcOnDiedSound()
    {
        gameObject.GetComponent<PlayerReferences>().Audio.GetComponent<AudioPlayerManager>().Play("Death2");
    }

    [Command(requiresAuthority = false)]
    public void CmdSendFight()
    {
        MainGame.instance.GameState = 2;
        RpcSendFight();

        //EndGame.instance.TpPlayerMirror(MainGame.instance.isTrapper, MainGame.instance.GameState);
        //gameObject.GetComponent<PlayerSetup>().TPlayerGameStateChange(2);
    }

    [ClientRpc]
    public void RpcSendFight()
    {
        EndGame.instance.TpPlayerMirror(MainGame.instance.isTrapper, 2);
    }

    public void PlayerFall(bool isTrapper)
    {
        gameObject.GetComponent<CharacterController>().enabled = false;
        if (isTrapper)
        {
            gameObject.GetComponent<NetworkTransform>().CmdTeleport(new Vector3(4.46f, 10.62f, -5.5f), Quaternion.identity);
        }
        else
        {
            gameObject.GetComponent<NetworkTransform>().CmdTeleport(new Vector3(-0.39f, 9.98f, -10.23f), Quaternion.identity);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Trigger")
        {
            isInNumber = other.gameObject.GetComponent<TriggerNumber>().TriggerNumberr;
            if (SetupTrap.instance.Traps[isInNumber].isActivable)
            {
                SetupTrap.instance.HelpText.gameObject.SetActive(true);
            }
        }
        else
        {
            isInNumber = -1;
        }


        if (other.tag == "DeathZone")
        {
            Debug.Log("YourAreDead");
            if (isLocalPlayer)
            {
                CmdSendDied(MainGame.instance.LocalPlayerId);
            }
        }

        if (other.tag=="FinishZone")
        {
            Debug.Log("Fight");
            if (isLocalPlayer)
            {
                CmdSendFight();
            }
        }

        if (other.tag == "Fall")
        {
            if (isLocalPlayer)
            {
                // PlayerFall(gameObject.GetComponent<PlayerSetup>().isTrapper);
                EndGame.instance.TpPlayerMirror(gameObject.GetComponent<PlayerSetup>().isTrapper, 1);
                Debug.Log("Fall");

                
            }
            
        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Trigger")
        {
            SetupTrap.instance.HelpText.gameObject.SetActive(false);
        }
    }


}
