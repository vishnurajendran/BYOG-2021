using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    [SerializeField] int bgClipName = 1;

    private void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.name.Equals("Player"))
        {
            Debug.Log("Entered Trigger");
            AudioManager.instance.PlayBG(bgClipName);
        }
    }

}
