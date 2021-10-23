using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyMovement : MonoBehaviour
{

    public Animator m_AnimControl;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse X") < 0)
        {
            m_AnimControl.SetBool("L_Turn", true);
        }
        else
            m_AnimControl.SetBool("L_Turn", false);
        if (Input.GetAxis("Mouse X") > 0)
        {
            m_AnimControl.SetBool("R_Turn", true);
        }
        else
            m_AnimControl.SetBool("R_Turn", false);
    }
}

