using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class CarUIController : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "Welcome!";
            IEnumerator coroutine = PrintNextLine(0);
            StartCoroutine(coroutine);
        }
    }

    public void AfterReachingCheckpoint(int num)
    {
        if (isLocalPlayer)
        {
            IEnumerator coroutine = PrintNextLine(num);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator PrintNextLine(int num)
    {
        if (num == 0)
        {
            yield return new WaitForSeconds(2.0f);
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "Welcome!\n Try to steer by the obstacles and reach the first checkpoint!";
            yield return new WaitForSeconds(4.0f);
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "";
        }
        else if (num == 1)
        {
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "Now, try to switch between Attack Mode and Defense Mode";
            yield return new WaitForSeconds(2f);
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "Attack Mode: you can use skills, but opponent's skill can take affect on you!\nDefense Mode: you cannot use skills, and opponent's skill cannot take affect on you!";
            yield return new WaitForSeconds(8f);
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "Move yourself LEFT to trigger Attack Mode.\nMove yourself RIGHT to trigger Defense Mode";
            yield return new WaitForSeconds(5f);
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "Keep going after you successfully switched modes!";
        } else if (num == 2)
        {
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "The spinning cube is a random skill prop. Hit it to pick up a skill!";
            yield return new WaitForSeconds(5f);
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "Say the skill ID to release a skill. E.g. Say: \"Skill 1\"";
            yield return new WaitForSeconds(5f);
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "You can only carry 3 skills at a time!";
            yield return new WaitForSeconds(2f);
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "Go forward to move to the starting line!";
            yield return new WaitForSeconds(2f);
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "";
        }
    }
}
