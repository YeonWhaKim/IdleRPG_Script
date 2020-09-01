using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialPanel : MonoBehaviour, IPointerClickHandler
{
    public TutorialManager tutorialManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        tutorialManager.OnClickTutorialPanel_Rebirth();
    }
}