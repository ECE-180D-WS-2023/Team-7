using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkInfo : NetworkBehaviour
{

    [SyncVar]
    public int PlayerID = -1;
    public int ConnID = -1;

    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
            GetSelfPlayerID();
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().OnPlayerJoining();
        }
    }


    [Command(requiresAuthority = false)]
    private void GetSelfPlayerID()
    {
        var returnedTuple = GameObject.FindGameObjectWithTag("IDManager").GetComponent<DistributePlayerID>().ReturnPlayerID();
        PlayerID = returnedTuple.Item1;
        ConnID = returnedTuple.Item2;
        Debug.Log("Client: " + PlayerID + " -- " + ConnID);
    }
}
