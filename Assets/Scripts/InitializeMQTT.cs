using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeMQTT : MonoBehaviour
{
    // Start is called before the first frame update
    bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (triggered)
        {
            return;
        }
        GameObject player = other.transform.root.gameObject;
        if (player.tag == "Player" && player.GetComponent<NetworkInfo>().isLocalPlayer)
        {
            Debug.Log("MQTT initializing...");
            GetComponent<MQTTController>().playerToControl = player;
            GetComponent<MQTTController>()._eventSender.topicSubscribe = "ECE180D/team7/" + player.GetComponent<NetworkInfo>().PlayerID.ToString();
            //GetComponent<MQTTController>()._eventSender.Connect();
            IEnumerator coroutine = AsyncConnect();
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator AsyncConnect()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<MQTTController>()._eventSender.Connect();
    }

}
