using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Audio : MonoBehaviour
{
    public static Audio AUDIO = null;

    [SerializeField]
    private AudioSource m_musicSource;
    [SerializeField]
    private List<AudioSource> m_sfxSources;

    public AudioClip m_lowPowerMusic;
    public AudioClip m_midPowerMusic;
    public AudioClip m_highPowerMusic;
    public AudioClip m_endGameMusic;


    public AudioClip m_hoverSound;
    public AudioClip m_death;
    public AudioClip m_checkpoint;
    public AudioClip m_equip;
    public AudioClip m_unequip;
    public AudioClip m_jump;
    public AudioClip m_dash;
    public AudioClip m_door;

    private int m_currentIndex = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        if(AUDIO)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        AUDIO = this;
    }

    // Update is called once per frame
    void Update()
    {
        AUDIO = this;
        gameObject.transform.position = Camera.main.transform.position;
    }

    
    public void SetMusic(AudioClip clip)
    {
        if(clip == null)
        {
            Debug.Log("NULL");
            m_musicSource.Stop();
        }
        else
        {
            Debug.Log("Playing");
            m_musicSource.clip = clip;
            m_musicSource.Play();
            m_musicSource.loop = clip != m_endGameMusic;
        }
    }

    public void PlayClip(AudioClip clip)
    {
        m_currentIndex++;
        if (m_currentIndex >= m_sfxSources.Count)
        {
            m_currentIndex = 0;
        }

        m_sfxSources[m_currentIndex].PlayOneShot(clip);
    }
}
