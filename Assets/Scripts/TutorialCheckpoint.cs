using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TutorialCheckpoint : MonoBehaviour
{
    public Material material = null;
    public int CheckPointNumber = 0;


    private void OnTriggerEnter(Collider other)
    {
        GameObject player = other.transform.root.gameObject;
        if (player.tag == "Player" && player.GetComponent<NetworkBehaviour>().isLocalPlayer)
        {
            player.GetComponent<CarUIController>().AfterReachingCheckpoint(CheckPointNumber);
            GetComponent<MeshRenderer>().material = material;
            IEnumerator coroutine = RemoveBoundary(CheckPointNumber);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator RemoveBoundary(int id)
    {
        if(id == 1)
        {
            yield return new WaitForSeconds(38f);
        }
        else
        {
            yield return new WaitForSeconds(20f);
        }
        GameObject Boundary = transform.Find("Plane").gameObject;
        Destroy(Boundary);
    }
}
