using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public AudioSource playerAudio;
    public AudioClip pickupSound;
    public Animator playerAnimator;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerAnimator.SetTrigger("Pickup");
            GameManager.Instance.AddPoints(1f);

            playerAudio.clip = pickupSound;
            playerAudio.Play();

            Destroy(gameObject);
        }
    }
}
