using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Control : MonoBehaviour
{
    List<Bullet> bullets = new List<Bullet>();

    public static Bullet selected_bullet;

    private int x = 0;
    private const int NUM_OF_SLOTS = 6;

    private GameObject inventory;
    private List<Transform> slots = new List<Transform>();
    private Transform selected_border;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i <= NUM_OF_SLOTS; i++)
        {
            Bullet slot1 = new Bullet(25.0f, 0.1f, Resources.Load<Material>("Materials/Bullets"+i), Resources.Load<Sprite>("Bullets/Bullets" + i));
            bullets.Add(slot1);
        }

        if (inventory == null)
        {
            inventory = GameObject.Find("Inventory");
        }

        int j = 0;
        foreach (Transform x in inventory.transform)
        {
            slots.Add(x);
           
            x.GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            x.GetChild(0).GetChild(0).GetComponent<Image>().sprite = bullets[j].GetImage();
            j++;
        }
       
        selected_border = slots[0];
        selected_bullet = bullets[0];
        SelectBullet(0);
    }

    public void SelectBullet(int number)
    {
        selected_border.GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 255);

        slots[number].GetChild(0).GetComponent<Image>().color = new Color32(185, 20, 238, 255);

        selected_border = slots[number];
        selected_bullet = bullets[number];
    }

    public void SetEmptySlot(int number)
    {
        slots[number].GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;
        slots[number].GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 0);
    }

    private KeyCode[] numericKeyCodes = {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
    };

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            x = Modulo((x + 1), NUM_OF_SLOTS);
            SelectBullet(x);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) // forward
        {
            x = Modulo((x - 1), NUM_OF_SLOTS);
            SelectBullet(x);
        }
        for(int i = 0; i < NUM_OF_SLOTS; i++)
        {
            if(Input.GetKeyDown(numericKeyCodes[i]))
            {
                x = i;
                SelectBullet(x);
            }
        }
    }

    private int Modulo(int a, int b)
    {
        return ((a % b) + b) % b;
    }
}
