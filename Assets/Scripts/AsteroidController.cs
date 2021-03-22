using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : Destructable
{

    public float lifetime;


    protected override void OnEnable()
    {
        base.OnEnable();
        if(lifetime != 0) {
            StartCoroutine(destroyAfter());
        }
    }


    protected virtual void OnDisable()
    {
        // print($"Asteroid disabled {this.name}");
    }

    protected virtual IEnumerator destroyAfter() {
        yield return new WaitForSeconds(lifetime);
        gameObject.SetActive(false);
    }

}
