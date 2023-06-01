using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateProgressOnCollision : MonoBehaviour
{
    
    private bool Reached = false;
    public Material Material = null;
    public AudioSource OnReached = null;

    void OnTriggerEnter(Collider other)
    {
        if (Reached)
        {
            return;
        }
        GameObject player = other.transform.root.gameObject;
        if (player.tag == "Player" && player.GetComponent<UpdateStats>().isLocalPlayer)
        {
            Debug.Log("checkpoint triggered");
            Reached = true;
            // player.GetComponent<UpdateStats>().UpdateProgress();
            player.GetComponent<UpdateStats>().CheckpointsReached += 1;
            GetComponent<MeshRenderer>().material = Material;

            OnReached.Play();
        }
    }
}
