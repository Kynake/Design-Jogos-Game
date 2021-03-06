using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool {
  private List<GameObject> _pool;
  public const int defaultPoolSize = 10;

  private Vector3 _farAwayPosition = new Vector3(-10000, -10000, -10000);

  // Constructor
  public ObjectPool(GameObject original, int poolSize = defaultPoolSize) {
    _pool = new List<GameObject>(poolSize);

    GameObject tmp;
    for(int i = 0; i < poolSize; i++) {
      tmp = Object.Instantiate(original);
      tmp.transform.position =_farAwayPosition;
      tmp.SetActive(false);
      _pool.Add(tmp);
    }
  }

  // Methods
  public GameObject getPooledGameObject() => _pool.Find(obj => !obj.activeInHierarchy);
  public int getUsedAmount() => _pool.Count(obj => obj.activeInHierarchy);
  public int getAvailableAmount() => _pool.Count(obj => !obj.activeInHierarchy);

  public T getPooledComponent<T>() where T: class => getPooledGameObject()?.GetComponentInChildren<T>();
}
