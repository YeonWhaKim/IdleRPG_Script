using UnityEngine;

public class Fade : MonoBehaviour
{
    public GameObject titleImage;
    public MeshRenderer meshrenderer;

    // Start is called before the first frame update
    public void FadeDisActice()
    {
        gameObject.SetActive(false);
        //meshrenderer.enabled = true;
    }

    public void TitleImageDisActice()
    {
        titleImage.SetActive(false);
    }
}