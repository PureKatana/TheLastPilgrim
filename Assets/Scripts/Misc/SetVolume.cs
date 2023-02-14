using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class SetVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string nameParam;
    [SerializeField] private bool isMusic;
    private Menu_Manager manager;
    private float volume;
    private Slider slider;

    public string NameParam { get => nameParam; set => nameParam = value; }

    // Start is called before the first frame update
    void Start()
    {
        manager = Menu_Manager.instance;
        slider = GetComponent<Slider>();
        if (isMusic)
            volume = manager.musicVolume;
        else
            volume = manager.sfxVolume;
        slider.value = volume;
    }

    public void SetVol(float volume)
    {
        audioMixer.SetFloat(nameParam, Mathf.Log10(volume) * 30);
        PlayerPrefs.SetFloat(nameParam, volume);
    }
}
