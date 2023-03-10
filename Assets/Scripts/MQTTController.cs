using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

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
        // Debug.Log("Event Fired. The message, from Object " + nameController + " is = " + newMsg);

        if (playerToControl != null)
        {
            if (newMsg == "Attack Mode" || newMsg == "Defense Mode")
            {
                playerToControl.GetComponent<SwitchMode>().changeMode(newMsg);
                // GameObject.FindGameObjectWithTag("UIMode").GetComponent<TMP_Text>().text = newMsg;
                // playerToControl.GetComponent<SwitchMode>().changeModeUI(newMsg);
            }
            if (newMsg == "skill 1" || newMsg == "skill 2" || newMsg == "skill 3")
            {
                playerToControl.GetComponent<SkillSystem>().SelectSkill(newMsg);
            }
            else // steering info & accel info
            {
                if (playerToControl.GetComponent<PrometeoCarController>().UsingIMUInput && playerToControl.GetComponent<PrometeoCarController>().isLocalPlayer)
                {
                    string[] dataString = newMsg.Split(',');
                    int angle = Int32.Parse(dataString[0]);
                    float throttle = Single.Parse(dataString[1]);

                    if (angle > 0)
                    {
                        playerToControl.GetComponent<PrometeoCarController>().TurnRightIMU(angle);
                    }
                    else
                    {
                        playerToControl.GetComponent<PrometeoCarController>().TurnLeftIMU(angle);
                    }

                    playerToControl.GetComponent<PrometeoCarController>().GoForwardIMU(throttle);

                }
            }
        }
    }
}