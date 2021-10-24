using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Trigger : MonoBehaviour
{
    [SerializeField]
    private Level3Controller level3Controller;
    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name.Equals("Player"))
        {
            level3Controller.ShowHideKeypad(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name.Equals("Player"))
        {
            level3Controller.ShowHideKeypad(false);
        }
    }
}
