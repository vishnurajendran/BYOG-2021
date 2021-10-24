using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Level3Controller : MonoBehaviour
{
    [SerializeField]
    private Keypad level3KeyPad;
    [SerializeField]
    private CountdownTimer timer;

    [SerializeField]
    private Transform levelEndTransform;
    private string correctAnswer;

    public void ShowHideKeypad(bool show)
    {
        if (show)
        {
            level3KeyPad.gameObject.SetActive(true);
            correctAnswer = timer.GetCurrentTimeString();
            level3KeyPad.RegisterToSubmit(ConfirmAnswer);
            Debug.Log("3rd level timer " + correctAnswer);
            level3KeyPad.ShowKeypad();
            //GameManager.Instance.isPaused = true;
            GameManager.Instance.ShowCursor();
        }
        else
        {
            CloseKeypad();
        }
    }

    void update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseKeypad();
        }
    }


    public void CloseKeypad()
    {
        level3KeyPad.CloseKeypad();
        //GameManager.Instance.isPaused = false;
        GameManager.Instance.HideCursor();
    }

    public void ConfirmAnswer(string submittedAnswer)
    {
        Debug.Log("Submitted answer " + submittedAnswer + "correect " + correctAnswer);
        if (submittedAnswer == correctAnswer)
        {
            levelEndTransform.DOLocalMove(new Vector3(-15.72f, -19.8f, -21.8f), 2f);
            CloseKeypad();
        }
    }
}
