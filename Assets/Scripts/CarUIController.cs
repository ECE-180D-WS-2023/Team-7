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
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "Welcome!\nTry to steer by the obstacles and reach the first checkpoint!";
            yield return new WaitForSeconds(4.0f);
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "";
        }
        else if (num == 1)
        {
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "Now, try to switch between Attack Mode and Defense Mode";
            yield return new WaitForSeconds(3f);
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "In ATTACK MODE, you CAN use skills to speed up or affect your opponent's controls\nBut your opponent can do the same to you";
            yield return new WaitForSeconds(8f);
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "In DEFENSE MODE, you CANNOT use skills \and your opponent CANNOT affect your control.";
            yield return new WaitForSeconds(5f);
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "Moving yourself LEFT -> Attack Mode.\nMoving yourself RIGHT -> Defense Mode";
            yield return new WaitForSeconds(5f);
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "Keep going after you successfully switched modes!";
            yield return new WaitForSeconds(4f);
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "";
        }
        else if (num == 2)
        {
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "The spinning cube is a random skill prop. Hit it to pick up a skill!";
            yield return new WaitForSeconds(5f);
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "Say the skill ID to release a skill.\nE.g. Say: \"Skill 1\"";
            yield return new WaitForSeconds(5f);
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "You can only carry 3 skills at a time!";
            yield return new WaitForSeconds(2f);
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "Go forward to move to the starting line!";
            yield return new WaitForSeconds(2f);
            GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>().text = "";
        }
    }
}
