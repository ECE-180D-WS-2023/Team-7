using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class GameStarter : MonoBehaviour
{

    public GameObject UISystem = null;
    private NetworkManager Manager = null;
    private bool Started = false;

    public enum Connection // your custom enumeration
    {
        Client,
        Server,
        Host
    };
    public Connection ConnectionType;

    // Start is called before the first frame update
    void Start()
    {
        UISystem = GameObject.FindGameObjectWithTag("UIMain");
        UISystem.SetActive(false);

        Manager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Started && Input.GetKeyDown(KeyCode.Return))
        {
            // UISystem.SetActive(true);


            if (ConnectionType == Connection.Host)
            {
                Manager.StartHost();
            } else if (ConnectionType == Connection.Client)
            {
                Manager.StartClient("34.94.115.129");
            } else
            {
                Manager.StartServer();
            }

            Started = true;
        }
    }
}
