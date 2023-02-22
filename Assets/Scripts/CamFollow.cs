using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class CamFollow : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = gameObject.transform;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>().LookAt = gameObject.transform.Find("CamTarget");

            RenderTexture rt = new RenderTexture(400, 100, 16);
            rt.name = "RearTexture" + GetComponent<NetworkInfo>().PlayerID.ToString();
            rt.Create();

            if (rt)
            {
                transform.Find("RearCamera").gameObject.GetComponent<Camera>().targetTexture = rt;
                transform.Find("Canvas").Find("RearView").gameObject.GetComponent<RawImage>().texture = rt;
            }

        } else
        {
            Destroy(transform.Find("RearCamera").gameObject);
            Destroy(transform.Find("Canvas").Find("RearView").gameObject);

        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
