using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarStatusUpdater : NetworkBehaviour
{


    private StatusTracker statusTracker;

    private void Start()
    {
        if (isLocalPlayer)
        {
            statusTracker = GameObject.FindGameObjectWithTag("StatusTracker").GetComponent<StatusTracker>();
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            if (GetComponent<NetworkInfo>().PlayerID == 1)
            {
                if (statusTracker.player1_speedup)
                {
                    GameObject.FindGameObjectWithTag("Status1").GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 1f);
                } else
                {
                    GameObject.FindGameObjectWithTag("Status1").GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 0f);
                }

                if (statusTracker.player1_slowdown)
                {
                    GameObject.FindGameObjectWithTag("Status2").GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 1f);
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Status2").GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 0f);
                }

                if (statusTracker.player1_inverse)
                {
                    GameObject.FindGameObjectWithTag("Status3").GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 1f);
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Status3").GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 0f);
                }

            } else
            {
                if (statusTracker.player2_speedup)
                {
                    GameObject.FindGameObjectWithTag("Status1").GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 1f);
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Status1").GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 0f);
                }

                if (statusTracker.player2_slowdown)
                {
                    GameObject.FindGameObjectWithTag("Status2").GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 1f);
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Status2").GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 0f);
                }

                if (statusTracker.player2_inverse)
                {
                    GameObject.FindGameObjectWithTag("Status3").GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 1f);
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Status3").GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 0f);
                }
            }

        }
    }
}
