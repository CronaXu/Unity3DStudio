using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{

    public GameObject gameManager;
    public TextMeshProUGUI appleCount;
    public float NumberOfPoints = 0;
    // Start is called before the first frame update
    void Start()
    {
        appleCount = GetComponent<TextMeshProUGUI>();
        
    }

    // Update is called once per frame
    void Update()
    {
        NumberOfPoints = gameManager.GetComponent<GameManager>().NumberOfPoints;
        appleCount.text = NumberOfPoints + "/50";

    }
}
