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
            correctAnswer = timer.GetCurrentTimeString();
            level3KeyPad.RegisterToSubmit(ConfirmAnswer);
            Debug.Log("3rd level timer " + correctAnswer);
            level3KeyPad.ShowKeypad();
        }
        else
        {
            level3KeyPad.CloseKeypad();
        }
    }

    public void ConfirmAnswer(string submittedAnswer)
    {
        Debug.Log("Submitted answer " + submittedAnswer + "correect " + correctAnswer);
        if (submittedAnswer == correctAnswer)
        {
            levelEndTransform.DOLocalMove(new Vector3(-15.72f,-19.8f,-21.8f),2f);
            level3KeyPad.CloseKeypad();
        }
    }
}
