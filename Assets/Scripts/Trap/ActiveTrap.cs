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
        if (Input.GetKeyDown(KeyCode.E) && isInNumber!=-1)
        {
            //Debug.Log("trap");
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
        }

        foreach (GameObject DeadZone in trap.DeathZone)
        {
            DeadZone.SetActive(true);
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
                ChatBehaviour.instance.CmdSendMessage("Died in the flames", MainGame.instance.LocalPlayerName);
                gameObject.GetComponent<Animator>().SetBool("death", true);
                Spectator.instance.ActiveSpectatorMode();
                gameObject.GetComponent<NewPlayerMovement>().canMove = false;
                gameObject.GetComponent<MouseLook>().canRotate = false;
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }
        }

        if (other.tag=="FinishZone")
        {
            Debug.Log("Fight");
            MainGame.instance.GameState = 2;
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
