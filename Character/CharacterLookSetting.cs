using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using UnityEngine;

public class CharacterLookSetting : MonoBehaviour
{
    public static CharacterLookSetting instance;
    public CharacterWeaponEffect characterWEaponEffect;
    public SkeletonAnimation skeletonAnimation;
    private Skeleton skeleton;
    private Skin baseSkin;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        skeleton = skeletonAnimation.Skeleton; // 스켈레톤 클래스
        baseSkin = skeleton.Data.FindSkin("base"); // 기존에 있는 스킨 가져오기
        baseSkin.AddAttachments(skeleton.Data.FindSkin("weapon/weapon001")); // 기존에 있는 스킨에 정의된 값을 새로운 스킨에 추가
        baseSkin.AddAttachments(skeleton.Data.FindSkin("body/body001")); // 기존에 있는 스킨에 정의된 값을 새로운 스킨에 추가
        baseSkin.AddAttachments(skeleton.Data.FindSkin("face/face001")); // 기존에 있는 스킨에 정의된 값을 새로운 스킨에 추가
        baseSkin.AddAttachments(skeleton.Data.FindSkin("cape/cape001")); // 기존에 있는 스킨에 정의된 값을 새로운 스킨에 추가
        baseSkin.AddAttachments(skeleton.Data.FindSkin("hand/hand001")); // 기존에 있는 스킨에 정의된 값을 새로운 스킨에 추가
        baseSkin.AddAttachments(skeleton.Data.FindSkin("hat/hat001")); // 기존에 있는 스킨에 정의된 값을 새로운 스킨에 추가
        baseSkin.AddAttachments(skeleton.Data.FindSkin("hip/hip001")); // 기존에 있는 스킨에 정의된 값을 새로운 스킨에 추가
        baseSkin.AddAttachments(skeleton.Data.FindSkin("shoes/shoes001")); // 기존에 있는 스킨에 정의된 값을 새로운 스킨에 추가

        skeleton.SetSkin(baseSkin);
        skeleton.SetSlotsToSetupPose();
    }

    private void OnEnable()
    {
        CentralInfoManager.instance.startSetting += StartSetting;
    }

    private void StartSetting()
    {
        for (int i = 0; i < CentralInfoManager.EQUIPKIND; i++)
        {
            if (CentralInfoManager.instance.equipLook[i] >= 0)
                ChracterEquipSetting(true, i, int.Parse(TableData.Item_Look_Number[CentralInfoManager.instance.equipLook[i]]));
        }
    }

    public void ChracterEquipSetting(bool isStart, int equipIndex, int equipNumber)
    {
        switch (equipIndex)
        {
            case 0:
                baseSkin.AddAttachments(skeleton.Data.FindSkin(string.Format("weapon/weapon{0}", string.Format("{0:D3}", equipNumber))));
                characterWEaponEffect.ChangeWeaponEffect(equipNumber - 1);
                break;

            case 1:
                baseSkin.AddAttachments(skeleton.Data.FindSkin(string.Format("hat/hat{0}", string.Format("{0:D3}", equipNumber))));
                if (equipNumber != 24
                    && equipNumber != 19
                    && equipNumber != 25)
                    baseSkin.RemoveAttachment(skeletonAnimation.Skeleton.FindSlotIndex("hatback"), "hatback");
                break;

            case 2:
                baseSkin.AddAttachments(skeleton.Data.FindSkin(string.Format("body/body{0}", string.Format("{0:D3}", equipNumber))));
                break;

            case 3:
                baseSkin.AddAttachments(skeleton.Data.FindSkin(string.Format("hand/hand{0}", string.Format("{0:D3}", equipNumber))));
                break;

            case 4:
                //Slot slot = skeletonAnimation.Skeleton.FindSlot("hip");
                baseSkin.RemoveAttachment(skeletonAnimation.Skeleton.FindSlotIndex("hip"), "hip");
                baseSkin.RemoveAttachment(skeletonAnimation.Skeleton.FindSlotIndex("hip_front1"), "hip_front1");
                baseSkin.RemoveAttachment(skeletonAnimation.Skeleton.FindSlotIndex("hip_front2"), "hip_front2");
                baseSkin.RemoveAttachment(skeletonAnimation.Skeleton.FindSlotIndex("hip_front3"), "hip_front3");
                baseSkin.AddAttachments(skeleton.Data.FindSkin(string.Format("hip/hip{0}", string.Format("{0:D3}", equipNumber))));
                //skeletonAnimation.Skeleton.SetAttachment(slot.ToString(), string.Format("skin/skin{0}/hip_front3", string.Format("{0:D3}", equipNumber + 1)));
                //baseSkin.SetAttachment(0, string.Format("hip/hip{0}", string.Format("{0:D3}", equipNumber + 1)), baseSkin.GetAttachment(0, "hip"));
                break;

            case 5:
                baseSkin.AddAttachments(skeleton.Data.FindSkin(string.Format("shoes/shoes{0}", string.Format("{0:D3}", equipNumber))));
                break;

            case 6:
                baseSkin.AddAttachments(skeleton.Data.FindSkin(string.Format("cape/cape{0}", string.Format("{0:D3}", equipNumber))));
                if (equipNumber != 21)
                {
                    baseSkin.RemoveAttachment(skeletonAnimation.Skeleton.FindSlotIndex("wing1"), "wing1");
                    baseSkin.RemoveAttachment(skeletonAnimation.Skeleton.FindSlotIndex("wing2"), "wing2");
                }
                else
                    baseSkin.RemoveAttachment(skeletonAnimation.Skeleton.FindSlotIndex("cape_back"), "cape_back");

                break;

            case 7:
                baseSkin.AddAttachments(skeleton.Data.FindSkin(string.Format("face/face{0}", string.Format("{0:D3}", equipNumber))));

                break;

            default:
                break;
        }
        skeleton.SetSkin(baseSkin);
        skeleton.SetSlotsToSetupPose();
        skeletonAnimation.AnimationState.Apply(skeleton);
        //나중에 여기서 저장.
    }
}