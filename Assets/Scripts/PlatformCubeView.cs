using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BYOG2021
{
    public class PlatformCubeView : MonoBehaviour
    {
        [SerializeField] private GameObject normalObj = null;
        [SerializeField] private GameObject highlightObj = null;

        private Level2Controller level2Controller = null;

        public int cubeId;

        private void Start()
        {
            Reset();
        }

        public void Init(Level2Controller level2Controller)
        {
            this.level2Controller = level2Controller;
            Reset();
        }

        public void Reset()
        {
            this.normalObj.SetActive(true);
            this.highlightObj.SetActive(false);
        }

        private void OnTriggerEnter(Collider collider)
        {
            Debug.LogFormat("OnTriggerEnter : {0}", collider.gameObject.name);

            if (collider.gameObject.name.Equals("Bottom"))
            {
                this.normalObj.SetActive(false);
                this.highlightObj.SetActive(true);

                this.level2Controller.OnEnterPlatform(this);
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            //Debug.LogFormat("OnTriggerExit : {0}", collider.gameObject.name);
        }
    }
}