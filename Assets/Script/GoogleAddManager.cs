using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoogleAddManager : MonoBehaviour
{
    private bool canNextScene = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!string.IsNullOrWhiteSpace(_adUnitId))
        {
            MobileAds.Initialize((InitializationStatus initStatus) =>
            {

                LoadInterstitialAd(_adUnitId);

            });
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (canNextScene)
        {
            if (Config.CurrentLevel >= 120)
            {
                Config.CurrentLevel = 119;
                SceneManager.LoadScene("MainMenu");

            }
            else
            {
                SceneManager.LoadScene("CatchMeGame");

            }

        }
    }




#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-4087428783254962/5240077799";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
  private string _adUnitId = "unused";
#endif
    public InterstitialAd interstitialAd;



    public void AdShow()
    {

        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            interstitialAd.Show();
        }
        else
        {
            canNextScene = true;

            Debug.LogError("Interstitial ad is not ready yet.");
        }

    }

    public void LoadInterstitialAd(string adModGoogleId)
    {
        // Clean up the old ad before loading a new one.
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        InterstitialAd.Load(adModGoogleId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd = ad;
                interstitialAd.OnAdFullScreenContentClosed += () =>
                {
                    Debug.Log("Interstitial Ad full screen content closed.");

                    canNextScene = true;

                };
                interstitialAd.OnAdFullScreenContentOpened += () =>
                {
                    Debug.Log("Interstitial Ad full screen content closed.");

                    canNextScene = false;

                };




            });

    }
    private void OnDestroy()
    {
        if (interstitialAd != null)
        {

            interstitialAd.Destroy();
            interstitialAd = null;
        }

    }

    private void RegisterReloadHandler(InterstitialAd interstitialAd)
    {
        // Raised when the ad closed full screen content.

    }
}
