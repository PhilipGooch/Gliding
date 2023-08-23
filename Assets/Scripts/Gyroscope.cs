using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyroscope : MonoBehaviour
{
    [SerializeField]
    Glider glider;

    void Awake()
    {
        
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(glider.transform.eulerAngles.x, 0, 0);
    }
}
