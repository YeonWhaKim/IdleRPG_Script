using UnityEngine;
using UnityEngine.UI;

public class TermsManager : MonoBehaviour
{
    public GPGSLogin gpgsLogin;
    public Toggle[] toggles;
    public Button gameStartButton;

    private void Awake()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            toggles[i].isOn = false;
            toggles[i].onValueChanged.AddListener((bool bOn) =>
            {
                OnClickToggleAndGameStartButtonSet();
            });
        }
    }

    public void OnClickTermsButton()
    {
        Application.OpenURL("https://www.thebackend.io/30c1f0ba69b34f7fdad4a94b7ff02b53034620c55e0d72d667d2a4ee247ddd13/terms.html");
    }

    public void OnClickPersonalInformationProcessingPolicyButton()
    {
        Application.OpenURL("https://www.thebackend.io/30c1f0ba69b34f7fdad4a94b7ff02b53034620c55e0d72d667d2a4ee247ddd13/privacy.html");
    }

    public void OnClickToggleAndGameStartButtonSet()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
                continue;
            else
                return;
        }
        gameStartButton.interactable = true;
    }

    public void OnClickGameStart()
    {
        gameObject.SetActive(false);
        gpgsLogin.CreateNickname();
    }
}