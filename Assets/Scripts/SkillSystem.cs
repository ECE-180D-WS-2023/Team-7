using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class SkillSystem : NetworkBehaviour
{

    private string[] MySkills = new string[3] { null, null, null };

    private int OriginalMaxSpeed;
    private int OriginalAccelMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
            string uiText = string.Format("1. {0}\n2. {1}\n3. {2}", MySkills[0], MySkills[1], MySkills[2]);
            GameObject.FindGameObjectWithTag("UISkill").GetComponent<TMP_Text>().text = uiText;

            OriginalMaxSpeed = GetComponent<PrometeoCarController>().maxSpeed;
            OriginalAccelMultiplier = GetComponent<PrometeoCarController>().accelerationMultiplier;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // THIS SECTION WILL BE DEPRECATED AFTER MQTT INTEGRATION
        if (isLocalPlayer)
        {
            string playerMode = GetComponent<SwitchMode>().mode;

            if (Input.GetKeyDown(KeyCode.Alpha1) && MySkills[0] != null && playerMode == "Attack Mode")
            {
                ReleaseSkill(MySkills[0]);
                MySkills[0] = null;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && MySkills[1] != null && playerMode == "Attack Mode")
            {
                ReleaseSkill(MySkills[1]);
                MySkills[1] = null;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && MySkills[2] != null && playerMode == "Attack Mode")
            {
                ReleaseSkill(MySkills[2]);
                MySkills[2] = null;
            }
            string uiText = string.Format("1. {0}\n2. {1}\n3. {2}", MySkills[0], MySkills[1], MySkills[2]);
            GameObject.FindGameObjectWithTag("UISkill").GetComponent<TMP_Text>().text = uiText;
        }
    }


    public void SelectSkill(int selection)
    {
        if (isLocalPlayer)
        {
            string playerMode = GetComponent<SwitchMode>().mode;

            if (selection == 1 && MySkills[0] != null && playerMode == "Attack Mode")
            {
                ReleaseSkill(MySkills[0]);
                MySkills[0] = null;
            }
            if (selection == 2 && MySkills[1] != null && playerMode == "Attack Mode")
            {
                ReleaseSkill(MySkills[1]);
                MySkills[1] = null;
            }
            if (selection == 3 && MySkills[2] != null && playerMode == "Attack Mode")
            {
                ReleaseSkill(MySkills[2]);
                MySkills[2] = null;
            }
            string uiText = string.Format("1. {0}\n2. {1}\n3. {2}", MySkills[0], MySkills[1], MySkills[2]);
            GameObject.FindGameObjectWithTag("UISkill").GetComponent<TMP_Text>().text = uiText;
        }
    }


    public void GetSkill(string skill)
    {
        for (int i = 0; i < MySkills.Length; i++)
        {
            if (MySkills[i] == null)
            {
                MySkills[i] = skill;
                break;
            }
        }

        // Update Skills UI as well
        string uiText = string.Format("1. {0}\n2. {1}\n3. {2}", MySkills[0], MySkills[1], MySkills[2]);
        GameObject.FindGameObjectWithTag("UISkill").GetComponent<TMP_Text>().text = uiText;
    }


    private void ReleaseSkill(string skill)
    {
        switch (skill)
        {
            case "speed_up":
                Debug.Log("Client: speed up");
                SpeedUp();
                break;
            case "slow_opponent_down":
                Debug.Log("Client: slow opponent down");
                SlowOpponentDown();
                break;
            case "invert_opponent_control":
                Debug.Log("Client: invert opponent control");
                InvertOpponnetCtrl();
                break;
        }
    }

    private void SpeedUp()
    {
        GetComponent<PrometeoCarController>().maxSpeed = 180;
        GetComponent<PrometeoCarController>().accelerationMultiplier = 10;

        IEnumerator coroutine = SkillEffectTimeout();
        StartCoroutine(coroutine);
    }

    private IEnumerator SkillEffectTimeout()
    {
        yield return new WaitForSeconds(5.0f);
        GetComponent<PrometeoCarController>().maxSpeed = OriginalMaxSpeed;
        GetComponent<PrometeoCarController>().accelerationMultiplier = OriginalAccelMultiplier;
    }



    /// <summary>
    /// SLOWING DOWN OPPONENT RELATED
    /// </summary>
    [Command(requiresAuthority = false)]
    private void SlowOpponentDown()
    {
        GameObject target = null;
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            int networkId = player.GetComponent<NetworkInfo>().PlayerID;
            Debug.Log(networkId);
            if (networkId != GetComponent<NetworkInfo>().PlayerID )
            {
                target = player;
            }
        }
        if (target == null)
        {
            Debug.Log("Opponent not found!");
            return;
        }

        if(target.GetComponent<SwitchMode>().mode == "Defense Mode")
        {
            return;
        }

        target.GetComponent<PrometeoCarController>().maxSpeed = 50;
        target.GetComponent<PrometeoCarController>().accelerationMultiplier = 2;

        IEnumerator coroutine = OpponnetSlowDownEffectTimeout(target);
        StartCoroutine(coroutine);
    }

    private IEnumerator OpponnetSlowDownEffectTimeout(GameObject target)
    {
        yield return new WaitForSeconds(5.0f);
        ChangeBackOpponentSpeed(target);
    }

    [Command(requiresAuthority = false)]
    private void ChangeBackOpponentSpeed(GameObject target)
    {
        target.GetComponent<PrometeoCarController>().maxSpeed = OriginalMaxSpeed;
        target.GetComponent<PrometeoCarController>().accelerationMultiplier = OriginalAccelMultiplier;
    }


    /// <summary>
    /// INVERTING OPPONENT CONTROL RELATED
    /// </summary>
    [TargetRpc]
    [Command(requiresAuthority = false)]
    private void InvertOpponnetCtrl()
    {
        GameObject target = null;
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            int networkId = player.GetComponent<NetworkInfo>().PlayerID;
            Debug.Log(networkId);
            if (networkId != GetComponent<NetworkInfo>().PlayerID)
            {
                target = player;
            }
        }
        if (target == null)
        {
            Debug.Log("Opponent not found!");
            return;
        }

        if (target.GetComponent<SwitchMode>().mode == "Defense Mode")
        {
            return;
        }

        target.GetComponent<PrometeoCarController>().SteeringInverter *= -1;

        IEnumerator coroutine = InvertOpponnetCtrlEffectTimeout();
        StartCoroutine(coroutine);
    }

    private IEnumerator InvertOpponnetCtrlEffectTimeout()
    {
        yield return new WaitForSeconds(5.0f);
        InvertOpponnetCtrl();
    }

}
