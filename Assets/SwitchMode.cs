using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class SwitchMode : NetworkBehaviour
{

    public string mode = "Attack Mode";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (mode == "Attack Mode")
                {
                    mode = "Defense Mode";
                }
                else
                {
                    mode = "Attack Mode";
                }

                GameObject.FindGameObjectWithTag("UIMode").GetComponent<TMP_Text>().text = mode;
            }
        }

    }

}
