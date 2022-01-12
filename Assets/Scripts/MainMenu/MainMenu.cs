using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static DataManager.SavedData savedData;
    public void Start()
    {
        savedData = DataManager.Load();
    }
}