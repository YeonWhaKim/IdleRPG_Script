using UnityEngine;

public class CartoonManager : MonoBehaviour
{
    public static CartoonManager instance;
    public GameObject cartoonObject;
    public GameObject[] cartoonContents;
    public RectTransform[] cartoonRT;
    private int cartoonIndex;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    public void OnClickExit()
    {
        LoadManager.instance.isDataLoaded = true;
        cartoonObject.SetActive(false);
    }

    public void OnClickNext()
    {
        cartoonIndex++;
        if (cartoonIndex >= cartoonContents.Length)
        {
            cartoonIndex = cartoonContents.Length - 1;
            return;
        }
        NextPage(cartoonIndex);
    }

    public void OnClickBack()
    {
        cartoonIndex--;
        if (cartoonIndex < 0)
        {
            cartoonIndex = 0;
            return;
        }

        NextPage(cartoonIndex);
    }

    public void CartoonOn()
    {
        cartoonObject.SetActive(true);
        cartoonIndex = 0;
        NextPage(cartoonIndex);
    }

    private void NextPage(int index)
    {
        cartoonRT[index].position = Vector2.zero;
        for (int i = 0; i < cartoonContents.Length; i++)
        {
            if (i.Equals(index))
                cartoonContents[i].SetActive(true);
            else
                cartoonContents[i].SetActive(false);
        }
    }
}