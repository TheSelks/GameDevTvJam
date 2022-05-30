using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class VolumeOptions : MonoBehaviour
{
    private Audio audio;

    [SerializeField] private Slider volumeSlider;

    // Start is called before the first frame update
    void Awake()
    {
        audio = FindObjectOfType<Audio>();
        volumeSlider.onValueChanged.AddListener(delegate { VolumeChange();});
        
    }

    void Start()
    {
        audio.Play("Background");
    }

    public void VolumeChange()
    {
        audio.chosenVolume = volumeSlider.value;
        audio.VolumeUpdate();
    }
    
}
