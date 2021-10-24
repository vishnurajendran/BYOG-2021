using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4Trigger : MonoBehaviour
{
   [SerializeField]
    private Level4Controller level4Controller;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name.Equals("Player"))
        {
            level4Controller.EndLevel(collider);
        }
    }
}
