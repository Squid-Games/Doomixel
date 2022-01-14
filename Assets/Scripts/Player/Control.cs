using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Control : MonoBehaviour
{
    private int x = 0;
    private const int NUM_OF_SLOTS = 6; //6-1

    static List<Bullet> bullets = new List<Bullet>();
    static List<Bullet> slots = new List<Bullet>();




    static List<Transform> slots_view = new List<Transform>();

    private GameObject inventory;

    public static Bullet selected_bullet;
    public static Transform selected_border;


    void Awake()
    {
        bullets = new List<Bullet>();
        for (int i = 0; i < 7; i++)
        {
            Bullet slot1 = new Bullet(i, 25.0f, 0.1f, Resources.Load<Material>("Materials/Bullets/Bullets_" + i), Resources.Load<Sprite>("Bullets/Bullets_" + i), 0);
            bullets.Add(slot1);
        }

        slots = new List<Bullet>();
        for (int i = 0; i < NUM_OF_SLOTS; i++)
        {
            if (i > 0)
                slots.Add(null);
            else
            {
                Bullet aux = bullets[i];
                aux.ammo =20;
                slots.Add(aux);
            }

        }


        if (inventory == null)
        {
            inventory = GameObject.Find("Inventory");
        }

        int j = 0;
        slots_view = new List<Transform>();
        foreach (Transform x in inventory.transform)
        {
            slots_view.Add(x);
            x.GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            if (slots[j] != null)
            {
                x.GetChild(0).GetChild(0).GetComponent<Image>().sprite = slots[j].GetImage();

                if(j==0) 
                    x.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "\u221E";
                
                else
                    x.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = slots[j].GetAmmo().ToString("");
                x.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color32(185, 20, 238, 255);
            }
            else
            {

                x.GetChild(0).GetChild(0).GetComponent<Image>().sprite = default;
                x.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 0);
                x.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                x.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color32(185, 20, 238, 255);

            }


            j++;
        }

        selected_border = slots_view[0];
        selected_bullet = slots[0];
        SelectBullet(0);
    }

    public static int AmmoNumber(int ammoID)
    {
        switch (ammoID)
        {
            case 1:
                return Random.Range(1, 4);

            case 2:
                return Random.Range(2, 5);

            case 3:
                return Random.Range(10, 16);

            case 4:
                return Random.Range(1, 4);

            case 5:
                return Random.Range(4, 7);
                
            case 6:
                return Random.Range(10, 16);
               
            default:
                return 5;
               
        } 

    }


    public static void Reward(int x)
    {

        int freespace = 7;


        Bullet aux = bullets[x];

        bool isfull = true;
        bool gasit = false;
        for (int i = 0; i <= 5; i++)
        {
            if (slots[i] != null)
            {
                if (slots[i].id == aux.id)
                {
                    slots[i].ammo += AmmoNumber(x);
                    SoundManagerScript.PlaySound("reward");
                    slots_view[i].GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = slots[i].GetAmmo().ToString("");
                    gasit = true;
                    break;
                }
            }
            else if (slots[i] == null && isfull == true)
            {
                isfull = false;
                freespace = i;

            }

        }

        if (isfull == false && gasit == false)
        {

            slots[freespace] = aux;
            slots[freespace].ammo = AmmoNumber(x);
            SoundManagerScript.PlaySound("reward");
            slots_view[freespace].GetChild(0).GetChild(0).GetComponent<Image>().sprite = slots[freespace].GetImage();
            slots_view[freespace].GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = slots[freespace].GetAmmo().ToString("");
            slots_view[freespace].GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

    }


    public void SelectBullet(int number)
    {
        // bullets[priviousnumber]= selected_bullet 
        selected_border.GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        selected_border.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color32(185, 20, 238, 255);
      

        slots_view[number].GetChild(0).GetComponent<Image>().color = new Color32(185, 20, 238, 255);
        slots_view[number].GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
        if (slots[number] == null)
            selected_bullet = null;

        else
            selected_bullet = slots[number];

        selected_border = slots_view[number];

    }

    public void SetEmptySlot(int number)
    {
        selected_border.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        slots_view[number].GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;
        slots_view[number].GetChild(0).GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        slots[number] = null;
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
        if (selected_bullet != null)
        {
            if (selected_bullet.ammo <= 0)
            {

                SetEmptySlot(x);
                SoundManagerScript.PlaySound("gunshot_empty");
                if (x == 5)
                {
                    SelectBullet(0);
                    x = 0;
                }
                else
                {
                    SelectBullet(x + 1);
                    x = x + 1;
                }
            }
        }
        if (!GameLogic.gamePaused)
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
            for (int i = 0; i < NUM_OF_SLOTS; i++)
            {
                if (Input.GetKeyDown(numericKeyCodes[i]))
                {
                    x = i;
                    SelectBullet(x);
                }
            }
        }
    }

    private int Modulo(int a, int b)
    {
        return ((a % b) + b) % b;
    }
}
