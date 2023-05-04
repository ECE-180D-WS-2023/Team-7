using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameStarter : MonoBehaviour
{

    private GameObject UISystem = null;
    private NetworkManager Manager = null;
    private bool Started = false;

    public enum Connection // your custom enumeration
    {
        Host,
        Server,
        Client
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
        if (!Started && Input.anyKey)
        {
            UISystem.SetActive(true);


            if (ConnectionType == Connection.Host)
            {
                Manager.StartHost();
            } else if (ConnectionType == Connection.Client)
            {
                Manager.StartClient();
            } else
            {
                Manager.StartServer();
            }

            Started = true;
        }
    }
}
