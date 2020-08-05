using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance;

    public string itemName_Offense = "Offense";
    public Transform parent_Offense;
    public GameObject prefab_Offense;

    public string itemName_HitEffect = "HitEffect";
    public Transform parent_HitEffect;
    public GameObject prefab_HitEffect;

    public string itemName_HitEffect_Cri = "HitEffect_Cri";
    public Transform parent_HitEffect_Cri;
    public GameObject prefab_HitEffect_Cri;

    public string itemName_HitSound = "HitSound";
    public Transform parent_HitSound;
    public GameObject prefab_HitSound;

    public string itemName_SwingSound = "SwingSound";
    public Transform parent_SwingSound;
    public GameObject prefab_SwingSound;

    public Transform parent_RunSound;
    public int poolCount = 10;

    public List<GameObject> poolList_Offense = new List<GameObject>();
    public List<GameObject> poolList_HitEffect = new List<GameObject>();
    public List<GameObject> poolList_HitEffect_Cri = new List<GameObject>();
    public List<GameObject> poolList_HitSound = new List<GameObject>();
    public List<GameObject> poolList_SwingSound = new List<GameObject>();
    public List<GameObject> poolList_RunSound = new List<GameObject>();
    public SettingManager settingManager;

    private void Start()
    {
        instance = this;
        Initialized();
    }

    public void Initialized()
    {
        for (int i = 0; i < poolCount; i++)
        {
            poolList_Offense.Add(CreateItem(parent_Offense, prefab_Offense, itemName_Offense));
            poolList_HitEffect.Add(CreateItem(parent_HitEffect, prefab_HitEffect, itemName_HitEffect));
            poolList_HitEffect_Cri.Add(CreateItem(parent_HitEffect_Cri, prefab_HitEffect_Cri, itemName_HitEffect_Cri));
            poolList_HitSound.Add(CreateItem_hitSound(parent_HitSound, prefab_HitSound, itemName_HitSound));
            poolList_SwingSound.Add(CreateItem_hitSound(parent_SwingSound, prefab_SwingSound, itemName_SwingSound));
            poolList_RunSound.Add(parent_RunSound.transform.GetChild(i).gameObject);
            settingManager.hitSoundList.Add(parent_RunSound.transform.GetChild(i).gameObject.GetComponent<AudioSource>());
        }
    }

    public GameObject CreateItem(Transform parent, GameObject prefab, string name)
    {
        var item = Instantiate(prefab) as GameObject;
        item.transform.name = name;
        item.transform.SetParent(parent);
        item.gameObject.SetActive(false);

        return item;
    }

    public GameObject CreateItem_hitSound(Transform parent, GameObject prefab, string name)
    {
        var item = Instantiate(prefab) as GameObject;
        item.transform.name = name;
        item.transform.SetParent(parent);
        item.gameObject.SetActive(false);

        settingManager.hitSoundList.Add(item.GetComponent<AudioSource>());

        return item;
    }

    public GameObject PopFromPool_Offense()
    {
        if (poolList_Offense.Count == 0)
            poolList_Offense.Add(CreateItem(parent_Offense, prefab_Offense, itemName_Offense));
        var item = poolList_Offense[0];
        item.SetActive(true);
        poolList_Offense.RemoveAt(0);

        return item;
    }

    public void ReturnToPool_Offense(GameObject item)
    {
        item.transform.SetParent(parent_Offense);
        item.SetActive(false);
        poolList_Offense.Add(item);
    }

    public GameObject PopFromPool_HitEffect()
    {
        if (poolList_HitEffect.Count == 0)
            poolList_HitEffect.Add(CreateItem(parent_HitEffect, prefab_HitEffect, itemName_HitEffect));
        var item = poolList_HitEffect[0];
        item.SetActive(true);
        poolList_HitEffect.RemoveAt(0);

        return item;
    }

    public void ReturnToPool_HitEffect(GameObject item)
    {
        item.transform.SetParent(parent_HitEffect);
        item.SetActive(false);
        poolList_HitEffect.Add(item);
    }

    public GameObject PopFromPool_HitEffect_Cri()
    {
        if (poolList_HitEffect_Cri.Count == 0)
            poolList_HitEffect_Cri.Add(CreateItem(parent_HitEffect_Cri, prefab_HitEffect_Cri, itemName_HitEffect_Cri));
        var item = poolList_HitEffect_Cri[0];
        item.SetActive(true);
        poolList_HitEffect_Cri.RemoveAt(0);

        return item;
    }

    public void ReturnToPool_HitEffectCri(GameObject item)
    {
        item.transform.SetParent(parent_HitEffect_Cri);
        item.SetActive(false);
        poolList_HitEffect_Cri.Add(item);
    }

    public GameObject PopFromPool_HitSound()
    {
        if (poolList_HitSound.Count == 0)
            poolList_HitSound.Add(CreateItem(parent_HitSound, prefab_HitSound, itemName_HitSound));
        var item = poolList_HitSound[0];
        item.SetActive(true);
        poolList_HitSound.RemoveAt(0);

        return item;
    }

    public void ReturnToPool_HitSound(GameObject item)
    {
        item.transform.SetParent(parent_HitSound);
        item.SetActive(false);
        poolList_HitSound.Add(item);
    }

    public GameObject PopFromPool_SwingSound()
    {
        if (poolList_SwingSound.Count == 0)
            poolList_SwingSound.Add(CreateItem(parent_SwingSound, prefab_SwingSound, itemName_SwingSound));
        var item = poolList_SwingSound[0];
        item.SetActive(true);
        poolList_SwingSound.RemoveAt(0);

        return item;
    }

    public void ReturnToPool_SwingSound(GameObject item)
    {
        item.transform.SetParent(parent_SwingSound);
        item.SetActive(false);
        poolList_SwingSound.Add(item);
    }

    public GameObject PopFromPool_RunSound()
    {
        var item = poolList_RunSound[0];
        item.SetActive(true);
        poolList_RunSound.RemoveAt(0);

        return item;
    }

    public void ReturnToPool_RunSound(GameObject item)
    {
        item.transform.SetParent(parent_RunSound);
        item.SetActive(false);
        poolList_RunSound.Add(item);
    }
}