using System.Collections;
using UnityEngine;

public class HitSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] hitSound;

    // Start is called before the first frame update
    private void Awake()
    {
        audioSource.clip = hitSound[Random.Range(0, hitSound.Length)];
    }

    private void OnEnable()
    {
        StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        ObjectPoolManager.instance.ReturnToPool_HitSound(gameObject);
    }
}