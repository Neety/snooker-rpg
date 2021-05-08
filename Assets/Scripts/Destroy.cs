using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField] private float destroyTime;
    void Start()
    {
        StartCoroutine(DestroySelf(destroyTime));
    }

    private IEnumerator DestroySelf(float destroyTime)
    {
        yield return new WaitForSeconds(destroyTime);
        this.gameObject.SetActive(false);
    }
}
