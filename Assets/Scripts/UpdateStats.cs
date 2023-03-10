using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class UpdateStats : NetworkBehaviour
{

    public int Speed = 0;
    public int CheckpointsReached = 0;
    public int TotalCheckpoints = 0;
    private bool IsWinner = false;
    private bool UIDestroyed = false;
    private float LapTime = 0.0f;

    private void UpdateSpeed()
    {

        transform.Find("UI").Find("Speedometer").gameObject.GetComponent<TMP_Text>().text = Speed.ToString() + "km/h";
    }

    private void UpdateTime()
    {
        string time = string.Format("{0:0}:{1:00}.{2:0}",
                        Mathf.Floor(LapTime / 60),
                        Mathf.Floor(LapTime) % 60,
                        Mathf.Floor((LapTime * 10) % 10));

        transform.Find("UI").Find("Timer").gameObject.GetComponent<TMP_Text>().text = "Lap: " + time + "s";
    }


    [Command(requiresAuthority = false)]
    public void UpdateProgress()
    {
        Debug.Log("Client updating progress...");

        CheckpointsReached += 1;

        TotalCheckpoints = GameObject.FindGameObjectsWithTag("Checkpoint").Length;
        int playerID = GetComponent<NetworkInfo>().PlayerID;
        if (playerID == 1)
        {
            Debug.Log("Player1 updating progress..." + CheckpointsReached.ToString());
            GameObject.FindGameObjectWithTag("UIProgress1").GetComponent<DisplayStats>().PlayerProgress = "Player1: " + CheckpointsReached.ToString() + "/" + TotalCheckpoints.ToString();
        } else if (playerID == 2)
        {
            Debug.Log("Player2 updating progress..." + CheckpointsReached.ToString());
            GameObject.FindGameObjectWithTag("UIProgress2").GetComponent<DisplayStats>().PlayerProgress = "Player2: " + CheckpointsReached.ToString() + "/" + TotalCheckpoints.ToString();
        }
    }

    public bool CheckIfIsWinner()
    {
        if (IsWinner)
        {
            return true;
        }

        int playerID = GetComponent<NetworkInfo>().PlayerID;
        string player1Stats = GameObject.FindGameObjectWithTag("UIProgress1").GetComponent<DisplayStats>().PlayerProgress;
        string player2Stats = GameObject.FindGameObjectWithTag("UIProgress2").GetComponent<DisplayStats>().PlayerProgress;

        if (playerID == 1 && player1Stats == "Player1: 5/5" && player2Stats != "Player2: 5/5")
        {
            IsWinner = true;
            return true;
        }
        if (playerID == 2 && player1Stats == "Player2: 5/5" && player2Stats != "Player1: 5/5")
        {
            IsWinner = true;
            return true;
        }

        return false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            Speed = (int)GetComponent<PrometeoCarController>().carSpeed;
            UpdateSpeed();

            if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().GameStarted && !IsWinner)
            {
                LapTime += Time.deltaTime;
            }
            UpdateTime();
        } else if (isLocalPlayer == false && UIDestroyed == false)
        {
            Destroy(transform.Find("UI").Find("Speedometer").gameObject);
            Destroy(transform.Find("UI").Find("ModeIndicator").gameObject);
            Destroy(transform.Find("UI"));
            UIDestroyed = true;
        }
    }
}
