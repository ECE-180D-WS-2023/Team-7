using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class GameManager : NetworkBehaviour
{
    // Server to client SyncVar
    [SyncVar]
    public int NumberOfPlayers = 2;

    [SyncVar]
    private int PlayersJoined = 0;

    [SyncVar]
    private bool GameStarted = false;

    private float CountDown = 6;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameStarted && PlayersJoined == NumberOfPlayers)
        {
            // Doing countdown here
            GameObject.FindGameObjectWithTag("Status").GetComponent<TMP_Text>().text = ((int)CountDown).ToString();

            CountDown -= Time.deltaTime;

            if (CountDown <= 1)
            {
                GameStarted = true;
                GameObject.FindGameObjectWithTag("Status").GetComponent<TMP_Text>().text = "GO!";

                foreach(GameObject StartBoundingBox in GameObject.FindGameObjectsWithTag("StartLine"))
                {
                    Destroy(StartBoundingBox);
                }
                IEnumerator coroutine = CountDownFade();
                StartCoroutine(coroutine);
            }

        } 

    }


    [Command(requiresAuthority = false)]
    public void OnPlayerJoining()
    {
        PlayersJoined++;
    }

    private IEnumerator CountDownFade()
    {
        yield return new WaitForSeconds(2.0f);
        GameObject.FindGameObjectWithTag("Status").GetComponent<TMP_Text>().text = "";
    }

}
