using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamingAsteroidController : AsteroidController
{
    public GameObject itemPrefab;
    private bool disabledByLifetime = false;

    private static ObjectPool _itemPool = null;


    protected override IEnumerator destroyAfter()
    {
        yield return new WaitForSeconds(lifetime);
        disabledByLifetime = true;
        gameObject.SetActive(false);
    }

    protected override void increaseDestructionStat() {
        GameController.flamingAsteroidsDestroyed++;
    }
}
