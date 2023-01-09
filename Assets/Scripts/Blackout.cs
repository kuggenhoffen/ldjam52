using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Blackout : MonoBehaviour
{
    
    Animator anim;
    public delegate void BlackoutCallback();
    BlackoutCallback finishCb;
    BlackoutCallback intermediateCb;
    public TMPro.TMP_Text blackoutText;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();   
        gameObject.SetActive(false);
        anim.StopPlayback();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string text)
    {
        blackoutText.SetText(text);
    }

    public void DoFadeIn(BlackoutCallback intermediateCallback, BlackoutCallback finishCallback, bool waitForInput = false)
    {
        gameObject.SetActive(true);
        anim.SetTrigger("In");
        anim.SetBool("WaitForOut", waitForInput);
        intermediateCb = intermediateCallback;
        finishCb = finishCallback;
    }

    public void DoFadeOut(BlackoutCallback intermediateCallback, BlackoutCallback finishCallback)
    {
        gameObject.SetActive(true);
        anim.SetTrigger("Out");
        anim.SetBool("WaitForOut", false);
        intermediateCb = intermediateCallback;
        finishCb = finishCallback;
    }

    public void OnBlackoutIntermediate()
    {
        if (intermediateCb != null) {
            intermediateCb();
        }
    }

    public void OnBlackoutFinish()
    {
        gameObject.SetActive(false);
        if (finishCb != null) {
            finishCb();
        }
    }
}
