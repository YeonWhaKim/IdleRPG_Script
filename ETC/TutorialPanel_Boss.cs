using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialPanel_Boss : MonoBehaviour, IPointerClickHandler
{
    public TutorialManager tutorialManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        tutorialManager.OnClickTutorialPanel_Boss();
    }
}