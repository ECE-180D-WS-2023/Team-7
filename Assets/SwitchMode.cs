using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class SwitchMode : NetworkBehaviour
{

    [SyncVar]
    public string mode = "Attack Mode";

    // Start is called before the first frame update

    [Command(requiresAuthority = false)]
    public void changeMode(string newMode)
    {
        mode = newMode;
    }
}
