using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.OnArchEnter(this.gameObject);

        Renderer render = gameObject.GetComponent<Renderer>();
        render.material.color = Color.green;
    }

    void OnTriggerExit(Collider other)
    {

    }
}
