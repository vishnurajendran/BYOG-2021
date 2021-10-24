using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level5Controller : MonoBehaviour
{
    [SerializeField]
    private Transform level5KeyPad;

    [SerializeField]
     private Transform level3KeyPad;

    [SerializeField]
    private TMPro.TMP_InputField inputField;

    // Start is called before the first frame update
    public void ShowHideKeypad(bool show)
    {
        level5KeyPad.gameObject.SetActive(show);
        if (show)
        {
            level3KeyPad.gameObject.SetActive(false);
            GameManager.Instance.isPaused = true;
            GameManager.Instance.ShowCursor();
        }
        else
        {
            GameManager.Instance.HideCursor();
        }
    }

    public void Confirm()
    {
        if (inputField.text.ToLower() == "reset")
        {
            level5KeyPad.gameObject.SetActive(false);
            GameManager.Instance.EndGame();
            Debug.Log("ho gaya");
        }
    }
}