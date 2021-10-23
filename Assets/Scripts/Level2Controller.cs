using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BYOG2021
{
    public class Level2Controller : MonoBehaviour
    {
        private List<PlatformCubeView> platformCubeViews = null;

        private Queue<PlatformCubeView> enteredPlatforms = null;

        private void Start()
        {
            this.enteredPlatforms = new Queue<PlatformCubeView>();

            this.platformCubeViews = this.gameObject.GetComponentsInChildren<PlatformCubeView>().ToList();

            foreach (PlatformCubeView item in platformCubeViews)
            {
                item.Init(this);
            }
        }

        public void OnEnterPlatform(PlatformCubeView platform)
        {
            if (this.enteredPlatforms.Count < this.platformCubeViews.Count)
            {
                this.enteredPlatforms.Enqueue(platform);
                Debug.LogFormat("enteredPlatforms count : {0}", this.enteredPlatforms.Count);

                if (this.enteredPlatforms.Count == this.platformCubeViews.Count)
                {
                    StartCoroutine(AllCubesDoneNowCheck());
                }
            }
        }

        private IEnumerator AllCubesDoneNowCheck()
        {
            yield return new WaitForSecondsRealtime(4f);
            bool validPattern = false;
            foreach (PlatformCubeView item in platformCubeViews)
            {
                // logic to check the pattern, can be anything you like
                if (item.cubeId == 1)
                {

                }
            }

            Debug.LogFormat("made a validPattern?? : {0}", validPattern);

            if (!validPattern)
            {
                // reset the platforms
                foreach (PlatformCubeView item in platformCubeViews)
                {
                    item.Reset();
                }

                this.enteredPlatforms.Clear();
            }
            else
            {
                // unlock the new level and elivator 
            }
        }
    }
}