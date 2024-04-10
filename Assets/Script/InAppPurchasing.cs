using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using TMPro;
using UnityEngine.Purchasing.Extension;
using System;
using GoogleMobileAds.Api;

public class InAppPurchasing : MonoBehaviour, IDetailedStoreListener
{

    private IStoreController storeController;
    private IExtensionProvider storeExtensionProvider;
    private string coinProductId;
    public ProductType goldType = ProductType.Consumable;
    public TMP_Text GoldCountText;
    public GameObject popupPanel;
    public TMP_Text popUpText;
    int m_GoldCount;
    int goldCount;
    public GameObject spinner;

    void Start()
    {
        m_GoldCount = Inventory.Coin;
        InitializePurchasing();
        UpdateUI();
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            LoadRewardedAd();

        });
    }


    void InitializePurchasing()
    {
        if (IsInitialized())
        {

            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct("com.trdsoft.labyrinthine.goldcount500", ProductType.Consumable);
        builder.AddProduct("com.trdsoft.labyrinthine.goldcount1000", ProductType.Consumable);
        builder.AddProduct("com.trdsoft.labyrinthine.goldcount1500", ProductType.Consumable);
        builder.AddProduct("com.trdsoft.labyrinthine.goldcount2000", ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);

    }

    private bool IsInitialized()
    {
        return storeController != null && storeExtensionProvider != null;
    }

    public void BuyCoin1()
    {
        coinProductId = "com.trdsoft.labyrinthine.goldcount500";
        goldCount = 500;
        BuyProductID(coinProductId);
    }

    public void BuyCoin2()
    {
        coinProductId = "com.trdsoft.labyrinthine.goldcount1000";
        goldCount = 1000;

        BuyProductID(coinProductId);
    }

    public void BuyCoin3()
    {
        coinProductId = "com.trdsoft.labyrinthine.goldcount1500";
        goldCount = 1500;

        BuyProductID(coinProductId);
    }
    public void BuyCoin4()
    {
        coinProductId = "com.trdsoft.labyrinthine.goldcount2000";
        goldCount = 2000;
        BuyProductID(coinProductId);
    }



    private void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = storeController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {


                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: Ürün mevcut deðil: " + productId);
            }
        }
        else
        {
            Debug.Log("BuyProductID: Satýn almayý baþlatamýyor, IAP henüz baþlatýlmadý.");
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        storeExtensionProvider = extensions;
    }



    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        if (string.Equals(e.purchasedProduct.definition.id, coinProductId, StringComparison.Ordinal))
        {
            Debug.Log("Coin satýn alýndý!");
            AddGold();
            // Burada coin miktarýný artýrýn ve oyuncuya verin.
        }
        else
        {
            Debug.Log("Satýn alma iþlemi baþarýsýz oldu.");
        }

        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"OnPurchaseFailed: Ürün: {product.definition.id}, Hata: {failureReason}");
    }

    void AddGold()
    {
        m_GoldCount += goldCount;
        Inventory.Coin = m_GoldCount;
        FirestoreExample.UpdateSkill();
        UpdateUI();
    }
    void UpdateUI()
    {
        GoldCountText.text = $"{m_GoldCount}";
        Debug.Log("UpdateComplate");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log($"OnPurchaseFailed: Ürün: {product.definition.id}, Hata: {failureDescription}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("OnInitializeFailed: " + error);

    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed: " + error);
    }







    #region Ad


    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-4087428783254962/5397357442";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
  private string _adUnitId = "unused";
#endif

    private RewardedAd _rewardedAd;

    /// <summary>
    /// Loads the rewarded ad.
    /// </summary>
    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;

                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());
                ad.OnAdPaid += (AdValue adValue) =>
                {

                };

                // Raised when an impression is recorded for an ad.
                ad.OnAdImpressionRecorded += () =>
                {
                    Debug.Log("Rewarded ad recorded an impression.");
                };
                // Raised when a click is recorded for an ad.
                ad.OnAdClicked += () =>
                {
                    Debug.Log("Rewarded ad was clicked.");
                };
                // Raised when an ad opened full screen content.
                ad.OnAdFullScreenContentOpened += () =>
                {
                    Debug.Log("Rewarded ad full screen content opened.");
                };
                // Raised when the ad closed full screen content.
                ad.OnAdFullScreenContentClosed += () =>
                {
                    var reward = ad.GetRewardItem();
                    if (reward != null)
                    {
                        m_GoldCount += (int)reward.Amount;
                        Inventory.Coin = m_GoldCount;
                        FirestoreExample.UpdateSkill();
                        UpdateUI();
                    }
                    LoadRewardedAd();
                    Debug.Log("Rewarded ad full screen content closed.");
                };
                // Raised when the ad failed to open full screen content.
                ad.OnAdFullScreenContentFailed += (AdError error) =>
                {
                    LoadRewardedAd();
                    Debug.LogError("Rewarded ad failed to open full screen content " +
                                   "with error : " + error);
                };
                _rewardedAd = ad;
            });
    }

    public void ShowRewardedAd()
    {
        spinner.SetActive(true);

        const string rewardMsg =
       "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
        spinner.SetActive(false);

    }
    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.

    }
    void Destroy()
    {
        _rewardedAd.Destroy();

    }
    private void RegisterReloadHandler(RewardedAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewardedAd();
        };
    }
    #endregion
}
