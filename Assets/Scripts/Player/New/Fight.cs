using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{
    public GameObject hitZone;
    public ParticleSystem blood;
    public Camera Camera;
    public Transform BloodEffectContainer;

    public float maxDistanceRay = 2;

    public float TimeBetweenClick = 1;
    public float Compteur=0;
    public bool AllowToClick = true;

    void Start()
    {

    }

    void Update()
    {
        if (Compteur>= TimeBetweenClick)
        {
            AllowToClick = true;
            Compteur = 0;
        }
        else
        {
            Compteur += Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0) && AllowToClick)
        {
            AllowToClick = false;
            var ray = Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistanceRay, 0))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log(hit.transform.name);
                if (hit.transform.gameObject.layer == 6 || hit.transform.gameObject.layer == 7) // 6 Player , 7 Ground
                {
                    //Debug.Log("Hit");
                    ParticleSystem _blood = Instantiate(blood, BloodEffectContainer );
                    _blood.transform.position = hit.point;
                    _blood.Play();
                }
                if (hit.transform.gameObject.layer == 6)
                {
                    Debug.Log("Remove life to " + hit.transform.gameObject.name);
                }
            }
        }

        
        
    }


    public void CleanSceneEffectBlood()
    {
        for (int i = 0; i < BloodEffectContainer.childCount; i++)
        {
            Destroy(BloodEffectContainer.GetChild(i).gameObject);
        }

    }


}
