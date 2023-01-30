using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkInfo : NetworkBehaviour
{

    public int PlayerID = -1;
    public int ConnID = -1;

    // Start is called before the first frame update
    void Start()
    {
        GetSelfPlayerID();
    }


    [Command(requiresAuthority = true)]
    private void GetSelfPlayerID()
    {
        var returnedTuple = GameObject.FindGameObjectWithTag("IDManager").GetComponent<DistributePlayerID>().ReturnPlayerID();
        PlayerID = returnedTuple.Item1;
        ConnID = returnedTuple.Item2;
        Debug.Log("Client: " + PlayerID + " -- " + ConnID);

        // UpdateProgress must be called once here so that UI gets initialized
        GetComponent<UpdateStats>().UpdateProgress();
    }
}
