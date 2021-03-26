using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamingAsteroidController : AsteroidController
{
    public GameObject itemPrefab;

    protected override IEnumerator destroyAfter()
    {
        yield return new WaitForSeconds(lifetime);
        gameObject.SetActive(false);
    }

    protected override void increaseDestructionStat() {
        GameController.flamingAsteroidsDestroyed++;
    }
}
