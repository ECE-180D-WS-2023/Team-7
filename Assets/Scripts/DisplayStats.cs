using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class DisplayStats : NetworkBehaviour
{

    [SyncVar(hook = nameof(UpdatePlayer1Progress))]
    public string Player1Progress = null;
    private void UpdatePlayer1Progress(string oldStr, string newStr)
    {
        GameObject.FindGameObjectWithTag("UIProgress").GetComponent<TMP_Text>().text = Player1Progress + "\n" + Player2Progress;
    }


    [SyncVar(hook = nameof(UpdatePlayer1Progress))]
    public string Player2Progress = null;
    private void UpdatePlayer2Progress(string oldStr, string newStr)
    {
        GameObject.FindGameObjectWithTag("UIProgress").GetComponent<TMP_Text>().text = Player1Progress + "\n" + Player2Progress;
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
