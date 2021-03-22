using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ThrusterController : MonoBehaviour
{

    public LayerMask destructables;
    public GameObject spark;

    public delegate void ColisaoAcao();
    public static event ColisaoAcao colisaoAcao;


    private void Awake()
    {
        spark.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if ((collider.gameObject.layer.toLayerMask() & destructables) != 0)
        {
            // print($"Destroy {collider.gameObject.name}");
            colisaoAcao?.Invoke();
            spark.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if ((collider.gameObject.layer.toLayerMask() & destructables) != 0)
        {
            spark.SetActive(false);
        }
    }

}
