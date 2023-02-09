using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MQTTController : MonoBehaviour
{
    public string nameController = "Controller 1";
    public string tagOfTheMQTTReceiver = "";
    public MQTTReceiver _eventSender;
    public GameObject playerToControl = null;

    void Start()
    {
        _eventSender.OnMessageArrived += OnMessageArrivedHandler;
    }

    private void OnMessageArrivedHandler(string newMsg)
    {
        Debug.Log("Event Fired. The message, from Object " + nameController + " is = " + newMsg);

        if (playerToControl == null)
        {
            Debug.LogError("MQTT initialized but player not set (FATAL)");
        } else
        {
            if (newMsg == "Attack Mode" || newMsg == "Defense Mode")
            {
                playerToControl.GetComponent<SwitchMode>().changeMode(newMsg);
                GameObject.FindGameObjectWithTag("UIMode").GetComponent<TMP_Text>().text = newMsg;
            }

        }
    }
}