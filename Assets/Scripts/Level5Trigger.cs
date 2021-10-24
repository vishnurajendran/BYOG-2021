using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5Trigger : MonoBehaviour
{
    [SerializeField]
    private Level5Controller level5Controller;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name.Equals("Player"))
        {
            level5Controller.ShowHideKeypad(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.name.Equals("Player"))
        {
            level5Controller.ShowHideKeypad(false);
        }
    }

    void update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            level5Controller.ShowHideKeypad(false);
        }
    }
}