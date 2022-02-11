using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.SceneManagement;

public class LargeApplePickup : MonoBehaviour
{
    public AudioSource playerAudio;
    public AudioClip pickupSound;

    public GameObject player;

    public GameObject WinText;


    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.AddPoints(1f);

            playerAudio.clip = pickupSound;
            playerAudio.Play();

            player.GetComponent<ThirdPersonController>().enabled = false;
            player.GetComponent<Animator>().speed = 0;
            WinText.SetActive(true);


        }
    }

    


}
