using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public float spinSpeed = 20f;
    void Start()
    {
        iTween.RotateBy(gameObject, iTween.Hash("y", 360f, "speed", spinSpeed, "looptype", iTween.LoopType.loop,"easetype",iTween.EaseType.linear));
    }

}
