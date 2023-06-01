using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using System;
using UnityEngine.UI;

public class SkillSystem : NetworkBehaviour
{

    // private string[] MySkills = new string[3] { null, null, null };
    private string[] MySkills = new string[3] { null, null, null };

    public AudioSource SkillReleaseSuccess = null;
    public AudioSource SkillReleaseFailure = null;

    public Texture2D SpeedUpTex = null;
    public Texture2D SlowDownTex = null;
    public Texture2D InverseControlTex = null;

    //private void Start()
    //{
    //    if (isLocalPlayer)
    //    {
    //        SkillIcon1 = GameObject.FindGameObjectWithTag("SkillIcon1");
    //        SkillIcon2 = GameObject.FindGameObjectWithTag("SkillIcon2");
    //        SkillIcon2 = GameObject.FindGameObjectWithTag("SkillIcon2");

    //        if (SkillIcon1 != null)
    //        {
    //            Debug.Log("Checked");
    //        }
    //    }
    //}

    void UpdateSkillUI()
    {
        if (isLocalPlayer)
        {
            for (int i=0; i<3; i++) { 
                if (MySkills[i] != null)
                {
                    if (MySkills[i] == "speed_up")
                    {
                        GameObject.FindGameObjectWithTag("SkillIcon"+i.ToString()).GetComponent<RawImage>().texture = SpeedUpTex;
                        GameObject.FindGameObjectWithTag("SkillIcon" + i.ToString()).GetComponent<RawImage>().color = new Color(1f,1f, 1f, 0.7f);
                    }
                    else if (MySkills[i] == "slow_opponent_down")
                    {
                        GameObject.FindGameObjectWithTag("SkillIcon" + i.ToString()).GetComponent<RawImage>().texture = SlowDownTex;
                        GameObject.FindGameObjectWithTag("SkillIcon" + i.ToString()).GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 0.7f);
                    }
                    else if (MySkills[i] == "invert_opponent_control")
                    {
                        GameObject.FindGameObjectWithTag("SkillIcon" + i.ToString()).GetComponent<RawImage>().texture = InverseControlTex;
                        GameObject.FindGameObjectWithTag("SkillIcon" + i.ToString()).GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 0.7f);
                    }
                }
                else
                {
                    GameObject.FindGameObjectWithTag("SkillIcon" + i.ToString()).GetComponent<RawImage>().texture = null;
                    GameObject.FindGameObjectWithTag("SkillIcon" + i.ToString()).GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 0f);
                }
            }
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
            // string uiText = string.Format("1. {0}\n\n2. {1}\n\n3. {2}", MySkills[0], MySkills[1], MySkills[2]);
            // GameObject.FindGameObjectWithTag("UISkill").GetComponent<TMP_Text>().text = uiText;
            UpdateSkillUI();

            //Debug.Log("1:" + MySkills[0] + "2:" + MySkills[1] + "3:" + MySkills[2]);
        }
    }


    public void SelectSkill(string selection)
    {
        if (isLocalPlayer)
        {
            string playerMode = GetComponent<SwitchMode>().mode;

            if (selection == "skill 1" && MySkills[0] != null && playerMode == "Attack Mode")
            {
                ReleaseSkill(MySkills[0]);
                MySkills[0] = null;
                SkillReleaseSuccess.Play();
            }
            else if (selection == "skill 2" && MySkills[1] != null && playerMode == "Attack Mode")
            {
                ReleaseSkill(MySkills[1]);
                MySkills[1] = null;
                SkillReleaseSuccess.Play();
            }
            else if (selection == "skill 3" && MySkills[2] != null && playerMode == "Attack Mode")
            {
                ReleaseSkill(MySkills[2]);
                MySkills[2] = null;
                SkillReleaseSuccess.Play();
            } else
            {
                SkillReleaseFailure.Play();
            }
            //string uiText = string.Format("1. {0}\n\n2. {1}\n\n3. {2}", MySkills[0], MySkills[1], MySkills[2]);
            //GameObject.FindGameObjectWithTag("UISkill").GetComponent<TMP_Text>().text = uiText;
            UpdateSkillUI();
        }
    }


    public void GetSkill(string skill)
    {

        for (int i = 0; i < 3; i++)
        {
            if (MySkills[i] == null)
            {
                MySkills[i] = skill;
                break;
            }
        }

        // Update Skills UI as well
        //string uiText = string.Format("1. {0}\n\n2. {1}\n\n3. {2}", MySkills[0], MySkills[1], MySkills[2]);
        //GameObject.FindGameObjectWithTag("UISkill").GetComponent<TMP_Text>().text = uiText;
        UpdateSkillUI();
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
        GetComponent<PrometeoCarController>().maxSpeed = GetComponent<PrometeoCarController>().OriginalMaxSpeed;
        GetComponent<PrometeoCarController>().accelerationMultiplier = GetComponent<PrometeoCarController>().OriginalAccelerationMultiplier;
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
        target.GetComponent<PrometeoCarController>().maxSpeed = target.GetComponent<PrometeoCarController>().OriginalMaxSpeed;
        target.GetComponent<PrometeoCarController>().accelerationMultiplier = target.GetComponent<PrometeoCarController>().OriginalAccelerationMultiplier;
    }


    /// <summary>
    /// INVERTING OPPONENT CONTROL RELATED
    /// </summary>
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

        IEnumerator coroutine = InvertOpponnetCtrlEffectTimeout(target);
        StartCoroutine(coroutine);
    }

    private IEnumerator InvertOpponnetCtrlEffectTimeout(GameObject target)
    {
        yield return new WaitForSeconds(5.0f);
        target.GetComponent<PrometeoCarController>().SteeringInverter *= -1;
    }

}
