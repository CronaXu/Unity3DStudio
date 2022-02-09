using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // This is private, so that we can show an error if its not set up yets
    public List<GameObject> Arches = new List<GameObject>();
    private int CurrentIndex = 0;

    private static GameManager staticInstance;

    public float NumberOfPoints = 0;

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
        Debug.Log("Added" + PointsToAdd + "Points!");
        NumberOfPoints += PointsToAdd;
    }

    public void OnArchEnter(GameObject archObject)
    {
        Debug.Log(Arches.IndexOf(archObject));
        CurrentIndex = Arches.IndexOf(archObject);
    }
}
