using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField, Range(0f, 1f)]
    float musicVolume = .5f;

    [SerializeField, Range(0f, 1f)]
    float sfxVolume = .5f;

    [SerializeField]
    AudioClip musicClip;

    [SerializeField]
    int poolSize = 10;

    [SerializeField]
    AudioClip arrowClicledClip;

    [SerializeField]
    AudioClip coverRemovedClip;

    [SerializeField]
    AudioClip puzzledSolvedClip;

    [SerializeField]
    AudioClip buttonClickedClip;

    List<AudioSource> sources;


    private void Start()
    {
        AudioSource src;
        if (musicClip != null)
        {
            src = SpawnAudioSource("MusicAudioSource");
            src.volume = musicVolume;
            src.loop = true;
            src.clip = musicClip;
            src.Play();
        }

        sources = new List<AudioSource>();        
        for (int i = 0; i < poolSize; i++)
        {
            src = SpawnAudioSource($"AudioClip_Source_{i}");
            sources.Add(src);
        }
    }

    private AudioSource SpawnAudioSource(string sourceName)
    {
        var go = new GameObject(sourceName);
        go.transform.SetParent(transform);
        var src = go.AddComponent<AudioSource>();
        src.volume = sfxVolume;

        return src;
    }

    AudioSource GetAvailableSource()
    {
        return sources.Where(s => !s.isPlaying).FirstOrDefault();
    }

    public void ArrowClicked() => PlayClip(arrowClicledClip);
    public void ButtonClicked() => PlayClip(buttonClickedClip);
    public void CoverRemoved() => PlayClip(coverRemovedClip);
    public void PuzzleSolved() => PlayClip(puzzledSolvedClip);

    void PlayClip(AudioClip clip)
    {
        var src = GetAvailableSource();
        if (src == null || clip == null)
            return;

        src.volume = sfxVolume;
        src.clip = clip;
        src.Play();
    }
}
