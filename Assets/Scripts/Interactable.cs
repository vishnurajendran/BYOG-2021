using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
    [SerializeField] private float throwForce = 600;
    [SerializeField] LayerMask pickupLayer;
    [SerializeField] LayerMask defaultLayer;

    private GameObject player;
    private GameObject objectParent;
    private bool isHolding = false;

    //// Start is called before the first frame update
    void Awake()
    {
        pickupLayer = 1 << LayerMask.NameToLayer("Clipping Helper");
        defaultLayer = 1 << LayerMask.NameToLayer("Default");

        player = GameObject.Find("Player");
        objectParent = FindChild(player.transform, "ObjectHolder").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance != null && GameManager.Instance.isPaused)
            this.GetComponent<Rigidbody>().isKinematic = true;
        else if(isHolding)
            this.GetComponent<Rigidbody>().isKinematic = true;
        else
            this.GetComponent<Rigidbody>().isKinematic = false;


        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    //distance = Vector3.Distance(this.transform.position, player.transform.position);
        //    if (isHolding)
        //    {
        //        isHolding = false;
        //        if (this.transform.parent == objectParent.transform)
        //        {
        //            this.transform.parent = null;
        //            if(this.transform.position.y < player.transform.position.y)
        //                this.transform.position = player.transform.position + new Vector3(0, 0, 2f);
        //            this.GetComponent<Rigidbody>().isKinematic = false;
        //        }
        //    }
        //    else
        //    {
        //        if (distance <= 2f && objectParent != this.transform.parent)
        //        {
        //            isHolding = true;
        //            this.GetComponent<Rigidbody>().isKinematic = true;
        //            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        //            this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        //            this.transform.parent = objectParent.transform;
        //            //this.transform.localPosition = Vector3.zero;

        //        }
        //    }
        //}


        if (isHolding && this.transform.localPosition != Vector3.zero)
        {
            //Debug.Log("hello");
            this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, Vector3.zero, 4f * Time.deltaTime);
        }
    }

    public void Hold(bool hold)
    {
        isHolding = hold;

        if (isHolding)
        {
            this.GetComponent<Rigidbody>().isKinematic = true;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            this.transform.parent = objectParent.transform;
            this.transform.gameObject.layer = LayerMask.NameToLayer("Clipping Helper");
            SetChildLayer(this.transform, "Clipping Helper");
        }
        else if(!isHolding && this.transform.parent == objectParent.transform)
        {
            this.transform.parent = null;
            if (this.transform.position.y < player.transform.position.y)
                this.transform.position = player.transform.position + new Vector3(0, 0, 2f);
            this.GetComponent<Rigidbody>().isKinematic = false;
            this.transform.gameObject.layer = LayerMask.NameToLayer("Default");
            SetChildLayer(this.transform, "Default");
        }
    }

    public void Throw()
    {
        isHolding = false;
        this.transform.parent = null;
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<Rigidbody>().AddForce(player.transform.forward * throwForce);
    }

    public static Transform FindChild(Transform parentObject, string name, bool matchCase = true)
    {
        if (parentObject.name == name)
            return parentObject;

        Transform temp = null;
        foreach (Transform child in parentObject)
        {
            temp = FindChild(child, name, matchCase);
            if (temp != null)
                return temp;
        }
        return null;
    }

    public static Transform SetChildLayer(Transform parentObject, string layerName)
    {
        Transform temp = null;
        foreach (Transform child in parentObject)
        {
            child.transform.gameObject.layer = LayerMask.NameToLayer(layerName);
            temp = FindChild(child, layerName);
            if (temp != null)
                return temp;
        }
        return null;
    }
}
