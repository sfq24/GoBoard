using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EndScreen : MonoBehaviour
{
    public PostProcessProfile normProfile;
    public PostProcessProfile blurProfile;
    public PostProcessVolume postProcessVolume;

    public void EnableCameraBlur(bool blur)
    {
        if(postProcessVolume !=null && normProfile != null && blurProfile != null)
        {
            postProcessVolume.profile = blur ? blurProfile : normProfile;
        }
    }

}
