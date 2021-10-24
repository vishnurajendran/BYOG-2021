using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

namespace BYOG2021
{
    public class Level2Controller : MonoBehaviour
    {
        [SerializeField]
        private Transform levelEndTransform;
        [SerializeField]
        private string correctSequence;
        private string currentSequence;
        private List<PlatformCubeView> platformCubeViews = null;

        private void Start()
        {
            currentSequence = "";

            this.platformCubeViews = this.gameObject.GetComponentsInChildren<PlatformCubeView>().ToList();

            foreach (PlatformCubeView item in platformCubeViews)
            {
                item.Init(this);
            }
        }

        public void OnEnterPlatform(PlatformCubeView platform)
        {
            if (!currentSequence.Contains(platform.gameObject.name))
            {
                currentSequence += platform.gameObject.name;
                if (currentSequence.Length == correctSequence.Length)
                {
                    StartCoroutine(AllCubesDoneNowCheck());
                }
            }

            Debug.Log("updated sequence " + currentSequence);
        }

        private IEnumerator AllCubesDoneNowCheck()
        {
            yield return new WaitForSecondsRealtime(1f);
            if (currentSequence != correctSequence)
            {
                // reset the platforms
                foreach (PlatformCubeView item in platformCubeViews)
                {
                    item.Reset();
                }
                currentSequence = "";
            }
            else
            {
                levelEndTransform.DOMoveY(2, 2f);
            }
        }
    }
}