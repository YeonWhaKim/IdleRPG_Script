using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PetMenu : MonoBehaviour
{
    public static PetMenu instance;

    [Header("Pet Info UI")]
    public Image petImage;

    public Text petName;
    public Text petStat;
    public Text mountText;
    public Text buyOrUpgradeText;
    public Text priceText;
    public Button mountButton;
    public Button buyOrUpgradeButton;

    [Header("Pet Scroll")]
    public Image[] mountGO;

    public Image[] selectedGO;

    public int selectedPetIndex = 0;
    public int mountingPetIndex = -1;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => LoadManager.instance.isDataLoaded);

        if (mountingPetIndex > -1)
        {
            selectedPetIndex = mountingPetIndex;
            OnClickMount();
        }
    }

    // Update is called once per frame
    public void OnClickPetScroll(int index)
    {
        selectedPetIndex = index;
        PetInfoSet(index);
        //여기서도 보석 세팅하기
        BuyButtonSetting();
    }

    private void PetInfoSet(int index)
    {
        petImage.sprite = PetManager.instance.petSprite[index];
        petName.text = PetManager.instance.ReturnPetName(index);
        petStat.text = PetManager.instance.ReturnPetStat(index);

        if (mountingPetIndex.Equals(index))
        {
            mountText.text = "모험중";
            mountButton.interactable = false;
        }
        else
        {
            mountText.text = "같이 모험하기";
            mountButton.interactable = PetManager.instance.isPetBuy[index];
        }

        if (PetManager.instance.isPetBuy[index].Equals(false))
            buyOrUpgradeText.text = "구매하기";
        else
            buyOrUpgradeText.text = "업그레이드 하기";

        if (PetManager.instance.petLevel[index].Equals(TableData.Pet_Max_Level[index]))
            buyOrUpgradeText.text = "애완동물 만렙";
        priceText.text = PetManager.instance.ReturnUpgradePrice(index);
        buyOrUpgradeButton.interactable = !PetManager.instance.petLevel[index].Equals(TableData.Pet_Max_Level[index]);

        for (int i = 0; i < selectedGO.Length; i++)
        {
            if (i.Equals(index))
                selectedGO[i].gameObject.SetActive(true);
            else
                selectedGO[i].gameObject.SetActive(false);
        }
    }

    public void OnClickBuyOrUpgrade()
    {
        CurrencyManager.instance.Jewel -= int.Parse(priceText.text);
        if (PetManager.instance.isPetBuy[selectedPetIndex].Equals(false))
            PetManager.instance.PetBuy(selectedPetIndex);
        else
            PetManager.instance.PetUpgrade(selectedPetIndex);

        PetInfoSet(selectedPetIndex);
        //여기서도 구매 버튼 세팅하기
        BuyButtonSetting();
    }

    public void OnClickMount()
    {
        mountingPetIndex = selectedPetIndex;
        mountButton.interactable = false;
        mountText.text = "모험중";
        PetManager.instance.PetApply(selectedPetIndex);
        for (int i = 0; i < mountGO.Length; i++)
        {
            if (i.Equals(mountingPetIndex))
                mountGO[i].gameObject.SetActive(true);
            else
                mountGO[i].gameObject.SetActive(false);
        }
        //StartCoroutine(SaveManager.instance.SaveData());
    }

    private void BuyButtonSetting()
    {
        if (priceText.text.Equals(PetManager.PETMAXLEVEL_SIGN))
            return;
        if (int.Parse(priceText.text) <= CurrencyManager.instance.Jewel)
            buyOrUpgradeButton.interactable = true;
        else
            buyOrUpgradeButton.interactable = false;
    }
}