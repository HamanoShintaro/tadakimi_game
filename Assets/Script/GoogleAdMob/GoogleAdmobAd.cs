using GoogleMobileAds.Api;
using System;
using UnityEngine;

public class GoogleAdmobAd : MonoBehaviour
{
    /*
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
        UserChoseToWatchAd();
        //this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
    }

    public void UserChoseToWatchAd()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }

    public void UserAdClosed()
    {
        //rewardedAd.OnAdClosed();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
    }
    */
}