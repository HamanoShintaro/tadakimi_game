using System;
using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleMobileAdsDemoScript : MonoBehaviour
{
    [SerializeField]
    private int reward = 100;

    [SerializeField]
    private BattleController battleController;

    [SerializeField]
    private Toggle[] toggles;

    private RewardedAd rewardedAd;

    private void Start()
    {
        if (PlayerPrefs.GetInt(PlayerPrefabKeys.currentAdsMode).Equals(0))
        {
            toggles[0].isOn = true;
            toggles[1].isOn = true;
        }
        else
        {
            toggles[0].isOn = false;
            toggles[1].isOn = false;
        }

        string adUnitId;
#if UNITY_ANDROID
            adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
        adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd("unexpected_platform");

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        //this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

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

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        Debug.Log("HandleRewardedAdFailedToLoad event received with message: "+ args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log("HandleRewardedAdFailedToShow event received with message: "+ args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        battleController.StartCoroutine("OnDisplayMoney", reward);
        Debug.Log("広告の報酬をもらう : HandleRewardedAdRewarded event received for "+ amount.ToString() + " " + type);
    }

    /// <summary>
    /// トグルによって広告表示モードの切り替えを行うメソッド
    /// </summary>
    /// <param name="toggle"></param>
    public void OnChangeAdsMode(Toggle toggle)
    {
        int index;
        if (toggle.isOn) index = 0;
        else index = 1;
        PlayerPrefs.SetInt(PlayerPrefabKeys.currentAdsMode, index);
    }
}