using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameLimiter : MonoBehaviour
{
    [SerializeField] int target = 60;
    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = target;
    }

}
