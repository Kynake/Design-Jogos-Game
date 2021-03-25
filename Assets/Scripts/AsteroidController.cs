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

    protected override void OnCollisionEnter2D(Collision2D collision) {
        var layer = collision.gameObject.layer.toLayerMask();
        if((layer & collideScoreLayers) != 0) {
            GameController.asteroidCollisions++;
        }
    }

    protected override void increaseDestructionStat() {
        GameController.asteroidsDestroyed++;
    }
}
