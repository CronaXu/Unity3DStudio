using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemPickup : MonoBehaviour
{
    public AudioSource playerAudio;
    public AudioClip pickupSound;
    public Animator playerAnimator;

    public bool canPick;
    public GameObject pickUI;

    public GameManager gameManager;

    void Update()
    {
        if (canPick && gameManager.GetComponent<GameManager>().currentPickupValue)
        {
            playerAnimator.SetTrigger("Pickup");
            GameManager.Instance.AddPoints(1f);

            playerAudio.clip = pickupSound;
            playerAudio.Play();
            pickUI.SetActive(false);

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canPick = true;
            pickUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canPick = false;
            pickUI.SetActive(false);
        }
    }

    
}
