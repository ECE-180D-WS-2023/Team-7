using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateProgressOnCollision : MonoBehaviour
{
    
    private bool Reached = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

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
            player.GetComponent<UpdateStats>().UpdateProgress();
        }
    }
}
