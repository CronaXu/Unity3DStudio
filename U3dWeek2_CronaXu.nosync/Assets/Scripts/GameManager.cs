using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class GameManager : MonoBehaviour
{
    // This is private, so that we can show an error if its not set up yets
    public List<GameObject> Apples = new List<GameObject>();
    private int CurrentIndex = -1;

    private static GameManager staticInstance;

    public float NumberOfPoints = 0;

    public GameObject door;
    public GameObject terrain;
    //private int timer = 100;

    public AudioSource playerAudio;
    public AudioClip doorOpenSound;


    public bool currentPickupValue;


    public static GameManager Instance
    {
        get
        {
            // If the static instance isn't set yet, throw an error
            if (staticInstance is null)
            {
                Debug.LogError("Game Manager is NULL");
            }

            return staticInstance;
        }
    }

    private void Awake()
    {
        // Set the static instance to this instance
        staticInstance = this;
        
    }

    public void AddPoints(float PointsToAdd)
    {
        Debug.Log("Points Added: " + PointsToAdd);
        
        NumberOfPoints += PointsToAdd;
        Debug.Log("Current Score: " + NumberOfPoints);

        if (NumberOfPoints == 49)
        {
            door.AddComponent<Rigidbody>();
            door.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            door.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
            door.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;

            terrain.GetComponent<TerrainCollider>().enabled = false;

            playerAudio.clip = doorOpenSound;
            playerAudio.Play();

        }
    }

    public void OnArchEnter(GameObject appleObject)
    {
        Debug.Log(Apples.IndexOf(appleObject));
        CurrentIndex = Apples.IndexOf(appleObject);
    }

    public void OnPickup(InputAction.CallbackContext context)
    {
        currentPickupValue = context.ReadValue<float>() == 1;
    }

    void Update()
    {
        
        //if (NumberOfPoints >= 49)
        //{
            
        //    timer--;
            
        //}

        //if (timer < 1)
        //{
        //    door.GetComponent<MeshCollider>().convex = true;
        //}
    }
}
