using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    public Image blackScreen;
    // Start is called before the first frame update
    void Start()
    {
        FadeFromBlack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FadeFromBlack ()
    {
        blackScreen.color = Color.black;
        blackScreen.canvasRenderer.SetAlpha(1.0f);
        blackScreen.CrossFadeAlpha (0.0f, 1, false);
    }
}
