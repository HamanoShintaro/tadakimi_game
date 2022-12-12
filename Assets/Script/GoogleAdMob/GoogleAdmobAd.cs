using GoogleMobileAds.Api;
using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GoogleAdmobAd : MonoBehaviour
{
#if UNITY_ANDROID
    private const string AdUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
    private const string AdUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
    private const string AdUnitId = "unexpected_platform";
#endif

    private RewardedAd rewardedAd;

    public void Start()
    {
        this.rewardedAd = new RewardedAd("unexpected_platform");

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void UserChoseToWatchAd()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }

}