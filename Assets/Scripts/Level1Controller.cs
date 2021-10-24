using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Level1Controller : MonoBehaviour
{
    [SerializeField]
    private Transform levelEndTransform;

    [SerializeField]
    private Transform levelEndKeyTransform;
    // Start is called before the first frame update
    public void EndLevel(Collider collider)
    {
        Transform key = collider.transform.Find("FirstPersonCharacter/ObjectHolder/FloorKey");
        if (key != null)
        {
            Destroy(key.gameObject);
            levelEndKeyTransform.gameObject.SetActive(true);
            levelEndKeyTransform.DORotate(new Vector3(170, -90, 180), 3f).OnComplete(() =>
            {
                levelEndTransform.DOMoveX(-5f, 3f);
            });
        }
    }
}
