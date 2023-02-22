using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TeleportPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject player = other.transform.root.gameObject;
        if (player.tag == "Player")
        {
            player.GetComponent<PrometeoCarController>().moveToStartPosition();
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().OnPlayerJoining();
        }
    }
}
