using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuAudioManager : MonoBehaviour
{

    [SerializeField] Slider volumeSlider;

    void Start()
    {
        if (!PlayerPrefs.HasKey("audioVolume"))
        {
            PlayerPrefs.SetFloat("audioVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }
	
    public void ChangeVolume()
    {
		// Volume of the game is equal to float. for eg. 0.5 = 50%
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("audioVolume");
    }
	
    private void Save()
    {
        PlayerPrefs.SetFloat("audioVolume", volumeSlider.value);
    }

}
