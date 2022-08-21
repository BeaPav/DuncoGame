using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteAudioScript : MonoBehaviour
{
    [SerializeField] Image iconImg;
    [SerializeField] Sprite iconPlay;
    [SerializeField] Sprite iconMute;
    [SerializeField] GameObject musicHolder;
    AudioSource audio;
    bool playingMusic;

    // Start is called before the first frame update
    void Start()
    {
        audio = musicHolder.GetComponent<AudioSource>();
        playingMusic = true;
        iconImg = transform.Find("escMenu/menu/ButtonMute").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AudioFunc()
    {
        if(playingMusic)
        {
            audio.mute = true;
            iconImg.sprite = iconMute;
            playingMusic = !playingMusic;
        }
        else
        {
            audio.mute = false;
            iconImg.sprite = iconPlay;
            playingMusic = !playingMusic;
        }
    }

}
