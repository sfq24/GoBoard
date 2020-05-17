using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public float fadeOffDelay = 0.5f;
    public float fadeInDelay = 0.5f;
    public float fadeTime = 3f;
    public iTween.EaseType easeType = iTween.EaseType.easeOutExpo;

    private Color solidColor = new Color(1, 1, 1, 1);
    private Color clearColor = new Color(1, 1, 1, 0);
    private MaskableGraphic graphic;
    private void Awake()
    {
        graphic = GetComponent<MaskableGraphic>();
    }
    private void UpdateColor(Color newColor)
    {
        graphic.color = newColor;
    }

    public void FadeOff()
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", solidColor,
            "to", clearColor,
            "time", fadeTime,
            "delay", fadeOffDelay,
            "easetype",easeType,
            "onupdatetarget", gameObject,
            "onupdate", "UpdateColor"
            ));
    }

    public void FadeOn()
    {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", clearColor,
            "to", solidColor,
            "time", fadeTime,
            "delay", fadeInDelay,
            "easetype", easeType,
            "onupdatetarget", gameObject,
            "onupdate", "UpdateColor"
            ));
    }

}
