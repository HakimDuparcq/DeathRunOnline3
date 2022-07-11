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
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }

    public void Start()
    {
        DisableDeathZone();
        HelpText.gameObject.SetActive(false);
    }
    /*
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

        foreach (ParticleSystem particle in trapParticles)
        {
            particle.Play();
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
        yield return new WaitForSeconds(trap.TimeParticleDisappear);

        foreach (ParticleSystem particle in trapParticles)
        {
            particle.Pause();
            Destroy(particle.gameObject);
        }

    }*/

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
