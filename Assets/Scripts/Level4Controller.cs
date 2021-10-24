using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Level4Controller : MonoBehaviour
{
    [SerializeField]
    private Transform levelEndTransform;


    public void EndLevel(Collider collider)
    {
        float signedAngle = Vector3.SignedAngle(collider.transform.forward, transform.forward, Vector3.up);
        if (Mathf.Abs(signedAngle) < 10)
        {
            levelEndTransform.DOMoveX(-5f, 3f);
        }
    }
}