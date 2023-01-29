using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class UpdateStats : NetworkBehaviour
{

    public int Speed = 0;
    public int CheckpointsReached = 0;

    private void UpdateSpeed()
    {
       GameObject.FindGameObjectWithTag("UISpeed").GetComponent<TMP_Text>().text = "Speed: " + Speed.ToString() + "km/h";
    }

    [Command]
    private void UpdateProgress()
    {
        int playerID = GetComponent<NetworkInfo>().PlayerID;
        if (playerID == 1)
        {
            GameObject.FindGameObjectWithTag("UIMain").GetComponent<DisplayStats>().Player1Progress = "Player1: " + CheckpointsReached.ToString();
        } else if (playerID == 2)
        {
            GameObject.FindGameObjectWithTag("UIMain").GetComponent<DisplayStats>().Player2Progress = "Player2: " + CheckpointsReached.ToString();
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            Speed = (int)GetComponent<PrometeoCarController>().carSpeed;
            UpdateSpeed();
            UpdateProgress();
        }
    }
}
