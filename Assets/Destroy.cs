using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField] private float destroyTime = 0.5f;
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
