using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    // void Start()
    // {

    // }

    // void Update()
    // {

    // }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        gameObject.SetActive(false);
    }
}
