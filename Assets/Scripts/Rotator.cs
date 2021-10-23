using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BYOG2021
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private float angleRotateSpeed;
        [SerializeField] private Vector3 rotationAxis;

        private float rotationSoFar = 0f;
        private void Update()
        {
            rotationSoFar += Time.deltaTime * angleRotateSpeed;
            this.transform.rotation = Quaternion.AngleAxis(rotationSoFar, rotationAxis);
        }
    }
}
