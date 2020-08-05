using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource[] allSound;

    [Header("BGM")]
    public AudioSource bgmSource;

    public AudioClip vampire80;
    public AudioClip bgm01;

    [Header("SoundEffect_Dungeon")]
    public AudioSource charcaterRunSource;

    public AudioSource bossAppearSource;
    public AudioSource goldGetSource;

    [Header("SoundEffect_UI")]
    public AudioSource effectSource_UI;

    public AudioClip rebirthClip;    //원래 pop
    public AudioClip levelUpClip;       //원래 adget
    public AudioClip boxOpenClip;
    public AudioSource menuButtonSource;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }

    public void UISoundPlay(int index)
    {
        switch (index)
        {
            case 0:     //메뉴버튼 클릭
                effectSource_UI.clip = rebirthClip;
                break;

            case 1:     //레벨업 버튼 클릭
                effectSource_UI.clip = levelUpClip;
                break;

            case 2:
                effectSource_UI.clip = boxOpenClip;
                break;

            default:
                break;
        }

        effectSource_UI.Play();
    }

    public void BossAppearPlay()
    {
        bossAppearSource.Play();
    }

    public void GoldGetPlay()
    {
        goldGetSource.Play();
    }

    public void AllSoundPlayExceptBG()
    {
        for (int i = 0; i < allSound.Length; i++)
        {
            allSound[i].mute = false;
        }
    }

    public void BGMChange(int index)
    {
        switch (index)
        {
            case 0:
                bgmSource.clip = vampire80;
                break;

            case 1:
                bgmSource.clip = bgm01;
                break;

            default:
                break;
        }
        bgmSource.Play();
    }
}