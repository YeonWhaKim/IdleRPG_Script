using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUpButton_InfoMenu : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isBtnDown = false;
    public InfoMenu infomenu;

    public void OnPointerDown(PointerEventData eventData)
    {
        isBtnDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isBtnDown = false;
    }

    //private void Start()
    //{
    //    InfoMenu.EquipLevelUp_CO = StartCoroutine(ButtonPressedCO());
    //}

    //private IEnumerator ButtonPressedCO()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSecondsRealtime(0.3f);
    //        if (isBtnDown)
    //            infomenu.OnClickEquipLevelUp();
    //    }
    //}
}