using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class UpdateStats : NetworkBehaviour
{

    public int Speed = 0;
    public int CheckpointsReached = 0;
    public int TotalCheckpoints = 5;
    public bool IsWinner = false;
    private bool UIDestroyed = false;
    private float LapTime = 0.0f;
    private string FinishTime = "";

    private void UpdateSpeed()
    {

        transform.Find("UI").Find("Speedometer").gameObject.GetComponent<TMP_Text>().text = Speed.ToString() + "km/h";
    }

    private void UpdateTime()
    {
        if (CheckpointsReached != 5)
        {
            string time = string.Format("{0:0}:{1:00}.{2:0}",
                            Mathf.Floor(LapTime / 60),
                            Mathf.Floor(LapTime) % 60,
                            Mathf.Floor((LapTime * 10) % 10));

            transform.Find("UI").Find("Timer").gameObject.GetComponent<TMP_Text>().text = "Lap: " + time;
        } else if (CheckpointsReached == 5 && FinishTime == "")
        {
            FinishTime = string.Format("{0:0}:{1:00}.{2:0}",
                            Mathf.Floor(LapTime / 60),
                            Mathf.Floor(LapTime) % 60,
                            Mathf.Floor((LapTime * 10) % 10));
        }
    }


    //[Command(requiresAuthority = false)]
    //public void UpdateProgress()
    //{
    //    Debug.Log("Client updating progress...");

    //    // CheckpointsReached += ;

    //    TotalCheckpoints = GameObject.FindGameObjectsWithTag("Checkpoint").Length;
    //    int playerID = GetComponent<NetworkInfo>().PlayerID;
    //    if (playerID == 1)
    //    {
    //        Debug.Log("Player1 updating progress..." + CheckpointsReached.ToString());
    //        GameObject.FindGameObjectWithTag("UIProgress1").GetComponent<DisplayStats>().PlayerProgress = "Player1: " + CheckpointsReached.ToString() + "/" + TotalCheckpoints.ToString();
    //    } else if (playerID == 2)
    //    {
    //        Debug.Log("Player2 updating progress..." + CheckpointsReached.ToString());
    //        GameObject.FindGameObjectWithTag("UIProgress2").GetComponent<DisplayStats>().PlayerProgress = "Player2: " + CheckpointsReached.ToString() + "/" + TotalCheckpoints.ToString();
    //    }
    //}


    [Command(requiresAuthority = false)]
    private void SetMyProgress(int playerID, int checkpoints)
    {
        if (playerID == 1) {
            GameObject.FindGameObjectWithTag("ProgressTracker").GetComponent<ProgressTracker>().player1progress = checkpoints;
        }
        else
        {
            GameObject.FindGameObjectWithTag("ProgressTracker").GetComponent<ProgressTracker>().player2progress = checkpoints;
        }
    }



    private void UpdateProgressUI()
    {
        int playerID = GetComponent<NetworkInfo>().PlayerID;

        SetMyProgress(playerID, CheckpointsReached);


        if (playerID == 1)
        {
            int opponentProgress = GameObject.FindGameObjectWithTag("ProgressTracker").GetComponent<ProgressTracker>().player2progress;
            GameObject.FindGameObjectWithTag("UIProgress1").GetComponent<TMP_Text>().text = "Player1: " + CheckpointsReached.ToString() + "/" + TotalCheckpoints.ToString();
            GameObject.FindGameObjectWithTag("UIProgress2").GetComponent<TMP_Text>().text = "Player2: " + opponentProgress.ToString() + "/" + TotalCheckpoints.ToString();

            if (CheckpointsReached == 5 && opponentProgress < 5 && !IsWinner)
            {
                GameObject.FindGameObjectWithTag("ProgressTracker").GetComponent<ProgressTracker>().winnerID = 1;
                IsWinner = true;
            }

            if (CheckpointsReached == 5)
            {
                GameObject.FindGameObjectWithTag("UIProgress1").GetComponent<TMP_Text>().text = "Player1: " + FinishTime;
            }

        } else
        {
            int opponentProgress = GameObject.FindGameObjectWithTag("ProgressTracker").GetComponent<ProgressTracker>().player1progress;
            GameObject.FindGameObjectWithTag("UIProgress2").GetComponent<TMP_Text>().text = "Player2: " + CheckpointsReached.ToString() + "/" + TotalCheckpoints.ToString();
            GameObject.FindGameObjectWithTag("UIProgress1").GetComponent<TMP_Text>().text = "Player1: " + opponentProgress.ToString() + "/" + TotalCheckpoints.ToString();

            if (CheckpointsReached == 5 && opponentProgress < 5 == !IsWinner)
            {
                GameObject.FindGameObjectWithTag("ProgressTracker").GetComponent<ProgressTracker>().winnerID = 2;
                IsWinner = true;
            }

            if (CheckpointsReached == 5)
            {
                GameObject.FindGameObjectWithTag("UIProgress1").GetComponent<TMP_Text>().text = "Player1: " + FinishTime;
            }
        }

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

            UpdateProgressUI();

        } else if (isLocalPlayer == false && UIDestroyed == false)
        {
            Destroy(transform.Find("UI").Find("Speedometer").gameObject);
            Destroy(transform.Find("UI").Find("ModeIndicator").gameObject);
            Destroy(transform.Find("UI").Find("Timer").gameObject);
            Destroy(transform.Find("UI").Find("DefenseModeTimeLeft").Find("TimeLeftBackground").gameObject);
            Destroy(transform.Find("UI").Find("DefenseModeTimeLeft").Find("TimeLeft").gameObject);
            Destroy(transform.Find("UI").Find("DefenseModeTimeLeft").gameObject);
            Destroy(transform.Find("UI").gameObject);
            UIDestroyed = true;
        }
    }
}
