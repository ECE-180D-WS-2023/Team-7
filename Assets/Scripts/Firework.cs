using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firework : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject player = other.transform.root.gameObject;
        if (player.tag == "Player" && player.GetComponent<UpdateStats>().isLocalPlayer && player.GetComponent<UpdateStats>().CheckIfIsWinner())
        {
            transform.Find("Particles").gameObject.GetComponent<ParticleSystem>().Play();
        }
    }
}
