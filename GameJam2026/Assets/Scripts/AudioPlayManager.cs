using UnityEngine;

public class AudioPlayManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_AudioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudioClip(AudioClip clip)
    {
        m_AudioSource.PlayOneShot(clip);
    }
}
