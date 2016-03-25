using UnityEngine;
using System.Collections;

public class WaterParticles : MonoBehaviour
{
    public ParticleSystem particle;
    public GameObject splash;

    private GameObject thisSplash;
    private GameObject player;
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }
    void OnTriggerEnter(Collider collide)
    {
        if (collide.gameObject == player)
        {
            //REMOVE THE COMMENTS AND FIX FOR WATER FOOTSTEPS
       //     thisSplash = (GameObject)Instantiate(splash, collide.transform.position, collide.transform.rotation);
       //     thisSplash.SetActive(true);
            particle.Play();
            particle.gameObject.SetActive(true);
        }
    }
    void OnTriggerExit(Collider collide)
    {
        particle.Stop();
    }
}