using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link : MonoBehaviour
{
    public float boarderThickness = 0.06f;
    public float delay = 0.2f;
    public float scaleTime = 0.5f;
    public iTween.EaseType easeType = iTween.EaseType.easeOutExpo;

    public void DrawLink(Vector3 startPos, Vector3 endPos)
    {
        transform.localScale = new Vector3(0.5f, 1f, 0);
        Vector3 dir = endPos - startPos;
        var zScale = Vector3.Magnitude(dir) - 2 * boarderThickness;
        Vector3 newScale = new Vector3(0.5f, 1f, zScale);
        transform.rotation = Quaternion.LookRotation(dir);

        transform.position = startPos + transform.forward * boarderThickness;
        iTween.ScaleTo(gameObject, iTween.Hash("time", scaleTime, "scale", newScale, "delay", delay, "easetype", easeType));
    }

}
