using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    private Transform _currentCamera;

    private void Start()
    {
        transform.position = new Vector3(-5, 90, 15);
        var rotation = new Vector3(90, 0, 180);
        transform.rotation = Quaternion.Euler(rotation);
    }
}
