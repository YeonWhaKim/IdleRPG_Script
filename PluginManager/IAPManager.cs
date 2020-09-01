using BackEnd;
using Firebase.Analytics;
using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager instance;
    public static IStoreController storeController;

    public const string jewel_500 = "jewel_500";
    public const string jewel_1000 = "jewel_1000";
    public const string jewel_2500 = "jewel_2500";
    public const string jewel_5000 = "jewel_5000";
    public const string dailyjewel25 = "dailyjewel25";
    public const string adremove_doublegold = "adremove_doublegold";
    public const string adremove_doubleatk = "adremove_doubleatk";
    public const string speedBuff = "speedbuff";

    private string productId = "";

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
        InInitialized();
    }

    private void InInitialized()
    {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(jewel_500, ProductType.Consumable, new IDs { { jewel_500, GooglePlay.Name } });
        builder.AddProduct(jewel_1000, ProductType.Consumable, new IDs { { jewel_1000, GooglePlay.Name } });
        builder.AddProduct(jewel_2500, ProductType.Consumable, new IDs { { jewel_2500, GooglePlay.Name } });
        builder.AddProduct(jewel_5000, ProductType.Consumable, new IDs { { jewel_5000, GooglePlay.Name } });
        builder.AddProduct(dailyjewel25, ProductType.Consumable, new IDs { { dailyjewel25, GooglePlay.Name } });
        builder.AddProduct(adremove_doublegold, ProductType.Consumable, new IDs { { adremove_doublegold, GooglePlay.Name } });
        builder.AddProduct(adremove_doubleatk, ProductType.Consumable, new IDs { { adremove_doubleatk, GooglePlay.Name } });
        builder.AddProduct(speedBuff, ProductType.Consumable, new IDs { { speedBuff, GooglePlay.Name } });

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        Debug.Log("결제 기능 초기화 완료");
        extensions.GetExtension<IAppleExtensions>().RestoreTransactions(result =>
        {
            if (result)
            {
                // This does not mean anything was restored,
                // merely that the restoration process succeeded.
                Debug.Log("거래 복원");
            }
            else
            {
                // Restoration failed.
            }
        });
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log(string.Format("error : {0}", error));
    }

    public void BuyProductID(string productID)
    {
#if UNITY_EDITOR
        BuyJewel(productId);
#elif UNITY_ANDROID

        productId = productID;
        try
        {
            if (storeController != null)
            {
                Product p = storeController.products.WithID(productID);
                if (p != null && p.availableToPurchase)
                    storeController.InitiatePurchase(p);
                else if (p != null && p.hasReceipt)
                    Debug.Log("BuyProductID : Fail. The Non Consumable product has already been bought.");
                else
                    Debug.Log("BuyProductID : Fail. Not purchasing product, either is not found or is not available for purchase");
            }
            else
                Debug.Log("BuyProductID : Fail. Not initialized.");
        }
        catch (System.Exception e)
        {
            Debug.Log(string.Format("BuyProductID : Fail. Exception during purchase. - error : {0}", e));
        }
#endif
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        BackendAsyncClass.BackendAsync(Backend.Receipt.IsValidateGooglePurchase, args.purchasedProduct.receipt, "receiptDescription", false, callback =>
        {
            // 영수증 검증에 성공한 경우
            if (callback.IsSuccess())
            {
                if (String.Equals(args.purchasedProduct.definition.id, productId, StringComparison.Ordinal))
                {
                    Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                    // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
                    BuyJewel(productId);
                }
                // Or ... a non-consumable product has been purchased by this user.
                else if (String.Equals(args.purchasedProduct.definition.id, productId, StringComparison.Ordinal))
                {
                    Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                    // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
                }
                // Or ... a subscription product has been purchased by this user.
                else if (String.Equals(args.purchasedProduct.definition.id, productId, StringComparison.Ordinal))
                {
                    Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                    // TODO: The subscription item has been successfully purchased, grant this to the player.
                }
            }
            else
            {
                // 영수증 검증에 실패한 경우
                Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
            }
        });
        // Return a flag indicating whether this product has completely been received, or if the application needs
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still
        // saving purchased products to the cloud, and when that save is delayed.
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        throw new System.NotImplementedException();
    }

    private void BuyJewel(string productId)
    {
        FirebaseAnalytics.LogEvent(string.Format("InAppPurchase_{0}", productId));

        switch (productId)
        {
            case "dailyjewel25":
                GameManager.SaveLogToServer("보석 구매", string.Format("상품이름 : {0}", productId), "유료 재화 구매");
                ShopMenu.instance.OnClickBuyJewel(0);
                break;

            case "jewel_500":
                GameManager.SaveLogToServer("보석 구매", string.Format("상품이름 : {0}", productId), "유료 재화 구매");

                ShopMenu.instance.OnClickBuyJewel(1);
                break;

            case "jewel_1000":
                GameManager.SaveLogToServer("보석 구매", string.Format("상품이름 : {0}", productId), "유료 재화 구매");

                ShopMenu.instance.OnClickBuyJewel(2);

                break;

            case "jewel_2500":
                GameManager.SaveLogToServer("보석 구매", string.Format("상품이름 : {0}", productId), "유료 재화 구매");

                ShopMenu.instance.OnClickBuyJewel(3);

                break;

            case "jewel_5000":
                GameManager.SaveLogToServer("보석 구매", string.Format("상품이름 : {0}", productId), "유료 재화 구매");

                ShopMenu.instance.OnClickBuyJewel(4);

                break;

            case "adremove_doublegold":
                GameManager.SaveLogToServer("광고제거 구매", string.Format("상품이름 : {0}", productId), "유료 재화 구매");

                ShopMenu.instance.OnClickBuyJewel(5);

                break;

            case "adremove_doubleatk":
                GameManager.SaveLogToServer("광고제거 구매", string.Format("상품이름 : {0}", productId), "유료 재화 구매");

                ShopMenu.instance.OnClickBuyJewel(6);

                break;

            case "speedbuff":
                GameManager.SaveLogToServer("속도 버프 구매", string.Format("상품이름 : {0}", productId), "유료 재화 구매");

                SpeedManager.instance.OnClickPayJewel();
                break;

            default:
                break;
        }
    }
}