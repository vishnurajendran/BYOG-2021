using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Trigger : MonoBehaviour
{
    [SerializeField]
    private Level1Controller level1Controller;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name.Equals("Player"))
        {
            level1Controller.EndLevel(collider);
        }
    }
}
