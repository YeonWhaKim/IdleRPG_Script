using System.Collections;
using UnityEngine;

public class SwingSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] swingSound;

    // Start is called before the first frame update
    private void Awake()
    {
        audioSource.clip = swingSound[Random.Range(0, swingSound.Length)];
    }

    private void OnEnable()
    {
        StartCoroutine(Disable());
    }

    private IEnumerator Disable()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        ObjectPoolManager.instance.ReturnToPool_SwingSound(gameObject);
    }
}