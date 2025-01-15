using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAdjuster : MonoBehaviour
{
    private void OnEnable()
    {
#if UNITY_ANDROID
#elif UNITY_IOS
#endif
    }
}
