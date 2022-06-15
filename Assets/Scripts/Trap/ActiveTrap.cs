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


            if (SetupTrap.instance.Traps[isInNumber].FallType == FallType.None)
            {
                return;
            }
            if (SetupTrap.instance.Traps[isInNumber].FallType == FallType.Down )
            {
                SetupTrap.instance.Traps[isInNumber].Trap.GetComponent<Rigidbody>().isKinematic = false;
            }
            else if (SetupTrap.instance.Traps[isInNumber].FallType == FallType.Up)
            {
                //SetupTrap.instance.Traps[isInNumber].Trap.GetComponent<Animation>().Play();
            }

            foreach (GameObject DeadZone in SetupTrap.instance.Traps[isInNumber].DeathZone)
            {
                DeadZone.SetActive(true);
            }

        }

        

       


    }

    public IEnumerator Spawning(TrapClass trap)
    {
        ParticleSystem[] trapParticles = new ParticleSystem[trap.positions.Length];
        yield return new WaitForSeconds(trap.TimeBeforeSpawn); //Delay before apararing

        int i = 0;
        foreach (Transform pos in trap.positions)
        {
            trapParticles[i] = Instantiate(trap.particles, pos);
            i++;
        }
        foreach (GameObject DeadZone in trap.DeathZone)
        {
            DeadZone.SetActive(true);
        }
        foreach (ParticleSystem particle in trapParticles)
        {
            particle.Play();
        }
        if (trap.Trap.GetComponent<Animator>())
        {
            trap.Trap.GetComponent<Animator>().SetBool("up", true);
        }


        yield return new WaitForSeconds(trap.TimeToStopAnimation);


        foreach (ParticleSystem particle in trapParticles)
        {
            particle.Pause();
        }
        yield return new WaitForSeconds(trap.TimeParticleVisible);

        foreach (ParticleSystem particle in trapParticles)
        {
            particle.Play();
        }
        if (trap.Trap.GetComponent<Animator>())
        {
            trap.Trap.GetComponent<Animator>()?.SetBool("down", true);
        }

        yield return new WaitForSeconds(trap.TimeParticleDisappear);

        foreach (ParticleSystem particle in trapParticles)
        {
            particle.Pause();
            Destroy(particle.gameObject);
        }
        foreach (GameObject DeadZone in trap.DeathZone)
        {
            DeadZone.SetActive(false);
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

            }
        }

        if (other.tag=="FinishZone")
        {

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
