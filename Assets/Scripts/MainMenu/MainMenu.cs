using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static DataManager.SavedData savedData;
    public void Start()
    {
        Debug.Log("Menu started");
        savedData = DataManager.Load();
    }
}