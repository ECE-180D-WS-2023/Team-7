using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TutorialCheckpoint : MonoBehaviour
{

    public int CheckPointNumber = 0;
    private void OnTriggerEnter(Collider other)
    {
        GameObject player = other.transform.root.gameObject;
        if (player.tag == "Player" && player.GetComponent<NetworkBehaviour>().isLocalPlayer)
        {
            player.GetComponent<CarUIController>().AfterReachingCheckpoint(CheckPointNumber);
            IEnumerator coroutine = RemoveBoundary();
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator RemoveBoundary()
    {
        yield return new WaitForSeconds(15f);
        GameObject Boundary = transform.Find("Plane").gameObject;
        Destroy(Boundary);
    }
}
