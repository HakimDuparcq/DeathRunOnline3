using UnityEngine;
using System.Collections;

public class SpawnParticle : MonoBehaviour
{

    public ParticleSystem particles;
    public Transform[] positions;
    public float TimeBeforeSpawn = 1;
    public float TimeToStopAnimation = 2;
    public float TimeParticleVisible = 4;
    public float TimeParticleDisappear = 1;

    void Start()
    {
        StartCoroutine(Spawning());
        
    }


    IEnumerator Spawning()
    {
        ParticleSystem[] trapParticles = new ParticleSystem[positions.Length];
        yield return new WaitForSeconds(TimeBeforeSpawn); //Delay before apararing

        int i = 0;
        foreach (Transform pos in positions)
        {
            trapParticles[i] = Instantiate(particles, pos);
            i++;
        }

        foreach (ParticleSystem particle in trapParticles)
        {
            particle.Play();
        }
        yield return new WaitForSeconds(TimeToStopAnimation);


        foreach (ParticleSystem particle in trapParticles)
        {
            particle.Pause();
        }
        yield return new WaitForSeconds(TimeParticleVisible);

        foreach (ParticleSystem particle in trapParticles)
        {
            particle.Play();
        }
        yield return new WaitForSeconds(TimeParticleDisappear);

        foreach (ParticleSystem particle in trapParticles)
        {
            particle.Pause();
            Destroy(particle.gameObject);
        }
        
    }


    void Update()
    {
        
    }
}