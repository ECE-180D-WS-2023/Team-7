using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class DisplayStats : NetworkBehaviour
{

    [SyncVar(hook = nameof(UpdatePlayerProgress))]
    public string PlayerProgress = null;
    private void UpdatePlayerProgress(string oldStr, string newStr)
    {
        GetComponent<TMP_Text>().text = PlayerProgress;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }
}
