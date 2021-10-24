using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Level1Controller : MonoBehaviour
{
    [SerializeField]
    private Transform levelEndTransform;
    // Start is called before the first frame update
    public void EndLevel(Collider collider)
    {
        Transform key = collider.transform.Find("FirstPersonCharacter/Camera/ObjectHolder/FloorKey");
        if (key != null)
        {
            Destroy(key.gameObject);
            levelEndTransform.DOMoveX(-5f, 3f);
        }
    }
}
