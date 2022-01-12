using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public Slider soundSlider;

    public static float GetSoundVolume()
    {
        if(PlayerPrefs.HasKey("Volume"))
            return PlayerPrefs.GetFloat("Volume");
        return 1;
    }
    void Start()
    {
        soundSlider.value = GetSoundVolume();
    }

    public void UpdateSoundVolume(float val)
    {
        if (val == 1)
            return;
        PlayerPrefs.SetFloat("Volume", val);
        PlayerPrefs.Save();
    }
}