using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpTrack : MonoBehaviour
{
    public float Speed = 0.5f;  // The speed of the texture movement
    public Vector2 Direction = new Vector2(0, 1);  // The direction of the movement

    private Material Material;  // The material of the game object

    void Start()
    {
        Material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AnimateTexture();
    }

    private void AnimateTexture()
    {
        Vector2 offset = Direction * Time.time * Speed;  // Calculate the offset

        Material.mainTextureOffset = offset;  // Set the texture offset
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject player = other.transform.root.gameObject;
        if (player.tag == "Player" && player.GetComponent<UpdateStats>().isLocalPlayer)
        {
            player.GetComponent<PrometeoCarController>().maxSpeed = 180;
            player.GetComponent<PrometeoCarController>().accelerationMultiplier = 15;

            IEnumerator coroutine = SpeedUpTimeout(player);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator SpeedUpTimeout(GameObject player)
    {
        yield return new WaitForSeconds(1.0f);
        player.GetComponent<PrometeoCarController>().maxSpeed = player.GetComponent<PrometeoCarController>().OriginalMaxSpeed;
        player.GetComponent<PrometeoCarController>().accelerationMultiplier = player.GetComponent<PrometeoCarController>().OriginalAccelerationMultiplier;
    }
}
