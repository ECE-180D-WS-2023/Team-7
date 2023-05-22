using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class CarUIController : NetworkBehaviour
{

    private TMP_Text TutorialUI;

    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
            TutorialUI = GameObject.FindGameObjectWithTag("TutorialHint").GetComponent<TMP_Text>();
            TutorialUI.text = "Welcome!";
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
            TutorialUI.text = "Welcome!\nTry to steer by the obstacles and reach the first checkpoint!";
            yield return new WaitForSeconds(4.0f);
            TutorialUI.text = "";
        }
        else if (num == 1)
        {
            TutorialUI.text = "Now, try to switch between Attack Mode and Defense Mode";
            yield return new WaitForSeconds(4f);
            TutorialUI.text = "In ATTACK MODE, you CAN use skills.\nBut your opponent can slow you down or inverse your control.";
            yield return new WaitForSeconds(8f);
            TutorialUI.text = "In DEFENSE MODE, you CANNOT use skills and your opponent CANNOT affect you.";
            yield return new WaitForSeconds(6f);
            TutorialUI.text = "Moving yourself LEFT -> Attack Mode.\nMoving yourself RIGHT -> Defense Mode.";
            yield return new WaitForSeconds(6f);
            TutorialUI.text = "Staying in Defense Mode drains energy.\nDrift to charge!";
            yield return new WaitForSeconds(6f);
            TutorialUI.text = "Keep going after you successfully switched modes!";
            yield return new WaitForSeconds(5f);
            TutorialUI.text = "";
        }
        else if (num == 2)
        {
            TutorialUI.text = "The spinning cube is a random skill prop. Hit it to pick up a skill!";
            yield return new WaitForSeconds(5f);
            TutorialUI.text = "Say the skill ID to release a skill.\nE.g. Say: \"Skill 1\"";
            yield return new WaitForSeconds(5f);
            TutorialUI.text = "You can only carry 3 skills at a time!";
            yield return new WaitForSeconds(4f);
            TutorialUI.text = "Go forward to move to the starting line!";
            yield return new WaitForSeconds(4f);
            TutorialUI.text = "";
        }
    }

    public void DeleteTutorialUIPanel()
    {
        GameObject uiPanel = TutorialUI.gameObject.transform.parent.gameObject;
        Destroy(TutorialUI.gameObject);
        Destroy(uiPanel);
    }
}
