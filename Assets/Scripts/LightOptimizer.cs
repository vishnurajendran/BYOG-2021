using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOptimizer : MonoBehaviour
{
    [SerializeField] List<Light> toEnable;
    [SerializeField] List<Light> toDisable;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name.Equals("Player"))
        {
            foreach (Light light in toEnable)
            {
                light.enabled = true;
            }

            foreach (Light light in toDisable)
            {
                light.enabled = false;
            }
        }
    }
}
