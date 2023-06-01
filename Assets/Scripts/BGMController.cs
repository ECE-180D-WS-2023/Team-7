using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{

    AudioSource Prep = null;
    AudioSource Game = null;

    // Start is called before the first frame update
    void Start()
    {
        Prep = transform.Find("Prep").gameObject.GetComponent<AudioSource>();
        Game = transform.Find("Game").gameObject.GetComponent<AudioSource>();
    }

    public void StopPrepBGM()
    {
        Prep.Stop();
    }

    public void StartGameBGM()
    {
        Game.Play();
    }
}
