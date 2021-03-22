using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;

    // private void Start()
    // {

    // }

    // Script temporario para camera seguir o foguete
    private void Update()
    {
        var position = player.transform.position;
        position.z = transform.position.z;
        transform.position = position;
    }
}
