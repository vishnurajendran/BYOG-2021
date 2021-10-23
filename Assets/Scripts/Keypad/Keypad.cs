using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class KeypadSubmitEvent : UnityEvent<string>{

}

public class Keypad : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text display;
    [SerializeField] List<Button> numButtons;
    [SerializeField] Button deleteButton;
    [SerializeField] Button submitButton;
    [SerializeField] CanvasGroup cg;
    [SerializeField] AudioSource clickSFX;


    public KeypadSubmitEvent OnKeypadSubmit = null;

    Coroutine fadeRoutine = null;
    bool autoCloseOnSubmit;


    public void ShowKeypad(bool autoCloseOnSubmit=true)
    {
        this.autoCloseOnSubmit = autoCloseOnSubmit;

        for (int i = 0; i < numButtons.Count; i++)
        {
            int num = i;
            numButtons[i].onClick.AddListener(() =>
            {
                OnPress(num);
            });
        }

        deleteButton.onClick.AddListener(OnPressDelete);
        submitButton.onClick.AddListener(OnPressSubmit);

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        cg.alpha = 0;
        fadeRoutine = StartCoroutine(Fade());
    }

    public void CloseKeypad()
    {
        OnKeypadSubmit?.RemoveAllListeners();
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(Fade(fadeIn:false));
    }

    IEnumerator Fade(bool fadeIn=true, float dur = 0.5f, bool destroyOnFadeOut = true)
    {
        yield return new WaitForEndOfFrame();
        float currAlpha = cg.alpha;
        float timeStep = 0;
        float newAlpha = fadeIn ? 1 : 0;
        while(timeStep <= 1)
        {
            timeStep += Time.deltaTime / dur;
            cg.alpha = Mathf.Lerp(currAlpha, newAlpha, timeStep);
            yield return new WaitForEndOfFrame();
        }

        if (!fadeIn && destroyOnFadeOut)
            Destroy(this.gameObject);
    }

    public void RegisterToSubmit(UnityAction<string> action)
    {
        if (OnKeypadSubmit == null)
            OnKeypadSubmit = new KeypadSubmitEvent();

        OnKeypadSubmit.AddListener(action);
    }

    void OnPress(int num)
    {
        clickSFX.Play();

        if (display.text.Length < 4)
            display.text += num.ToString();
    }

    void OnPressDelete()
    {
        clickSFX.Play();

        if (display.text.Length > 1)
            display.text = display.text.Substring(0, display.text.Length - 1);
        else
            display.text = string.Empty;
    }

    void OnPressSubmit()
    {
        clickSFX.Play();

        OnKeypadSubmit?.Invoke(display.text);
        display.text = "";

        if (autoCloseOnSubmit)
            CloseKeypad();
    }

    private void Start()
    {
        ShowKeypad();
        RegisterToSubmit((string data) =>
        {
            Debug.Log(data);
        });
    }
}
