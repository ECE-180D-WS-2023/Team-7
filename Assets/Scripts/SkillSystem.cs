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
        string uiText = string.Format("1. {0}\n2. {1}\n3. {2}", MySkills[0], MySkills[1], MySkills[2]);
        GameObject.FindGameObjectWithTag("UISkill").GetComponent<TMP_Text>().text = uiText;

        OriginalMaxSpeed = GetComponent<PrometeoCarController>().maxSpeed;
        OriginalAccelMultiplier = GetComponent<PrometeoCarController>().accelerationMultiplier;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1) && MySkills[0] != null)
        {
            ReleaseSkill(MySkills[0]);
            MySkills[0] = null;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && MySkills[1] != null)
        {
            ReleaseSkill(MySkills[1]);
            MySkills[1] = null;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && MySkills[2] != null)
        {
            ReleaseSkill(MySkills[2]);
            MySkills[2] = null;
        }
        string uiText = string.Format("1. {0}\n2. {1}\n3. {2}", MySkills[0], MySkills[1], MySkills[2]);
        GameObject.FindGameObjectWithTag("UISkill").GetComponent<TMP_Text>().text = uiText;

        Debug.Log(GetComponent<PrometeoCarController>().accelerationMultiplier);
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
                SpeedUp();
                break;
            case "invert_opponent_control":
                Debug.Log("Client: invert opponent control");
                SpeedUp();
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
        yield return new WaitForSeconds(3.0f);
        GetComponent<PrometeoCarController>().maxSpeed = OriginalMaxSpeed;
        GetComponent<PrometeoCarController>().accelerationMultiplier = OriginalAccelMultiplier;
    }


    [TargetRpc]
    private void SlowOpponentDown()
    {
        return;
    }

    [TargetRpc]
    private void InvertOpponentControl()
    {
        return;
    }
}
