using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level5Controller : MonoBehaviour
{
    [SerializeField]
    private Transform level5KeyPad;

    [SerializeField]
    private TMPro.TMP_InputField inputField; 

    // Start is called before the first frame update
    public void ShowHideKeypad(bool show)
    {
        level5KeyPad.gameObject.SetActive(show);
    }

    public void Confirm()
    {
        if(inputField.text.ToLower() == "reset")
        {
            Debug.Log("ho gaya");
        }
    }
}