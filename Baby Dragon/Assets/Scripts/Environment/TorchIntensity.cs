using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchIntensity : MonoBehaviour
{

    private Light lightSource;
    private float lightIntensity;
    private void Start()
    {
        lightSource = GetComponent<Light>();
    }

    private void Update()
    {
        lightIntensity = UnityEngine.Random.Range(4.5f, 6.5f);
        lightSource.intensity = lightIntensity;
    }
}
