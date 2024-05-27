using System.Collections;
using System.Collections.Generic;
//using Google.Play.AppUpdate;
//using Google.Play.Common;
using UnityEngine;


public class AppUpdate : MonoBehaviour
{
    /*
#if UNITY_ANDROID
    private void Start()
    {
        StartCoroutine(CheckForUpdate());
    }

    IEnumerator CheckForUpdate()
    {
        AppUpdateManager appUpdateManager = new AppUpdateManager();
        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation =
        appUpdateManager.GetAppUpdateInfo();

        // Wait until the asynchronous operation completes.
        yield return appUpdateInfoOperation;

        if (appUpdateInfoOperation.IsSuccessful)
        {
            var appUpdateInfoResult = appUpdateInfoOperation.GetResult();
            // Check AppUpdateInfo's UpdateAvailability, UpdatePriority,
            // IsUpdateTypeAllowed(), etc. and decide whether to ask the user
            // to start an in-app update.

            // Creates an AppUpdateOptions defining an immediate in-app
            // update flow and its parameters.
            var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();
            StartCoroutine(StartImmediateUpdate(appUpdateManager, appUpdateInfoResult, appUpdateOptions));
        }
        else
        {
            // Log appUpdateInfoOperation.Error.
        }
    }

    IEnumerator StartImmediateUpdate(AppUpdateManager appUpdateManager, AppUpdateInfo appUpdateInfoResult, AppUpdateOptions appUpdateOptions)
    {
        // Creates an AppUpdateRequest that can be used to monitor the
        // requested in-app update flow.
        var startUpdateRequest = appUpdateManager.StartUpdate(
        // The result returned by PlayAsyncOperation.GetResult().
        appUpdateInfoResult,
        // The AppUpdateOptions created defining the requested in-app update
        // and its parameters.
        appUpdateOptions);
        yield return startUpdateRequest;

        // If the update completes successfully, then the app restarts and this line
        // is never reached. If this line is reached, then handle the failure (for
        // example, by logging result.Error or by displaying a message to the user).
    }

    public void RequestReview()
    {
        StartCoroutine(RequestReviewAndroid());
    }

    private IEnumerator RequestReviewAndroid()
    {
        var reviewManager = new Google.Play.Review.ReviewManager();
        var requestFlowOperation = reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != Google.Play.Review.ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        var playReviewInfo = requestFlowOperation.GetResult();
        var launchFlowOperation = reviewManager.LaunchReviewFlow(playReviewInfo);
        yield return launchFlowOperation;
        playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != Google.Play.Review.ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        Debug.Log("レビューのお願い");
    }
#endif
*/
}
