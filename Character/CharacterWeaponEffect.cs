using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponEffect : MonoBehaviour
{
    public SkeletonRenderer skeletonRenderer;
    private string slotName = "weapon_point";
    private int slotIndex;
    private int effectIndex;
    public PointAttachment point;
    public PointFollower pointFollower;
    public List<GameObject> weaponeEffects = new List<GameObject>();

    private string[] weaponePoint = {"06", "06", "06", "07", "09", "08", "09", "10", "10", "12",
        "11", "05", "10", "09", "03", "08", "11", "09", "09", "10",
        "12", "13", "09", "08", "12", "11","05","07","06","06" };

    // Start is called before the first frame update
    private void Start()
    {
        effectIndex = 0;
        slotIndex = skeletonRenderer.Skeleton.FindSlotIndex(slotName);
    }

    public void ChangeWeaponEffect(int index)
    {
        weaponeEffects[effectIndex].SetActive(false);
        ChangePointAttachment(index);
        weaponeEffects[index].SetActive(true);
        effectIndex = index;
    }

    private void ChangePointAttachment(int index)
    {
        //point = skeletonRenderer.Skeleton.GetAttachment(slotIndex, string.Format("weapon_point/weapon_point{0}", weaponePoint[index])) as PointAttachment;
        pointFollower.pointAttachmentName = string.Format("weapon_point{0}", weaponePoint[index]);
        pointFollower.weaponEffect = weaponeEffects[index].gameObject.transform;
        pointFollower.UpdateReferences();
    }
}