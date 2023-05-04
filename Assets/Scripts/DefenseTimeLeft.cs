using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DefenseTimeLeft : MonoBehaviour
{

    private float TimeLeft = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GetComponent<PrometeoCarController>().isLocalPlayer)
        {
            if (GetComponent<PrometeoCarController>().isDrifting)
            {
                TimeLeft = TimeLeft + 5*Time.deltaTime;
            }

            if (GetComponent<SwitchMode>().mode == "Defense Mode")
            {
                if (TimeLeft <= 0.0f)
                {
                    GetComponent<SwitchMode>().changeMode("Attack Mode");
                    TimeLeft = 0.0f;
                } else
                {
                    TimeLeft -= Time.deltaTime;
                }
            }

            TimeLeft = TimeLeft >= 10.0f ? 10.0f : TimeLeft;

            float UIScale = TimeLeft / 10.0f;
            float UIOffset = (-5) * UIScale + 5.0f;
            GameObject UITimeLeft = GameObject.FindGameObjectWithTag("TimeLeft");
            UITimeLeft.transform.localPosition = new Vector3(UIOffset, 0.01f, 0);
            UITimeLeft.transform.localScale = new Vector3(UIScale, 1, 1);
        }
    }
}
