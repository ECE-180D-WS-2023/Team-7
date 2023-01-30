using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItem : MonoBehaviour
{

    private bool Available = true;
    
    // THIS LIST SHOULD BE EXPANDED ON NEED
    private string[] Skills = new string[3]{"speed_up", "slow_opponent_down", "invert_opponent_control"};

// Start is called before the first frame update
void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(1, 5, 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        int rnd = Random.Range(0, Skills.Length);

        if (Available)
        {
            GameObject player = other.transform.root.gameObject;
            if (player.tag == "Player")
            {
                player.GetComponent<SkillSystem>().GetSkill(Skills[rnd]);
            }
        }
    }
}
