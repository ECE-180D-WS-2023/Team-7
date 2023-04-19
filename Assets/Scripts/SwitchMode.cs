using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class SwitchMode : NetworkBehaviour
{

    [SyncVar(hook = nameof(changeModeUI))]
    public string mode = "Attack Mode";

    public AudioSource ShieldUp = null;

    private void Start()
    {
        transform.Find("Shield").gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(isLocalPlayer && Input.GetKeyDown(KeyCode.M))
        {
            if(mode == "Attack Mode")
            {
                changeMode("Defense Mode");
            }
            else
            {
                changeMode("Attack Mode");
            }
        }
        if (mode == "Attack Mode")
        {
            transform.Find("Shield").gameObject.SetActive(false);
        }
        else
        {
            transform.Find("Shield").gameObject.SetActive(true);
        }
    }


    [Command(requiresAuthority = false)]
    public void changeMode(string newMode)
    {
        mode = newMode;
    }

    public void changeModeUI(string oldStr, string newStr)
    {
        if (isLocalPlayer)
        {
            transform.Find("UI").Find("ModeIndicator").gameObject.GetComponent<TMP_Text>().text = newStr;

            if (newStr == "Defense Mode")
            {
                ShieldUp.Play();
            }
        }
    }
}
