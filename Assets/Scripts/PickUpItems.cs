using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItems : MonoBehaviour
{
    [Header("Item info")]
    public int itemRadius;
    public string itemTag;
    private GameObject itemToPick;

    [Header("player")]
    public GameObject player;

    void Start()
    {
        itemToPick = GameObject.FindWithTag(itemTag);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= itemRadius)
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (itemTag == "Pistol")
                {
                    WeaponSwitching.instance.allWeapons += 1;
                    WeaponSwitching.instance.selectedWeapon += 1;
                    Debug.Log(itemTag);
                    Debug.Log(WeaponSwitching.instance.selectedWeapon);
                    Debug.Log(WeaponSwitching.instance.allWeapons);
                }
                else if (itemTag == "UMP")
                {
                    WeaponSwitching.instance.allWeapons += 1;
                    WeaponSwitching.instance.selectedWeapon += 1;

                    Debug.Log(itemTag);
                    Debug.Log(WeaponSwitching.instance.selectedWeapon);
                    Debug.Log(WeaponSwitching.instance.allWeapons);
                }
                else if (itemTag == "AK")
                {
                    WeaponSwitching.instance.allWeapons += 1;
                    WeaponSwitching.instance.selectedWeapon += 1;

                    Debug.Log(itemTag);
                    Debug.Log(WeaponSwitching.instance.selectedWeapon);
                    Debug.Log(WeaponSwitching.instance.allWeapons);
                }
                itemToPick.SetActive(false);
            }
        }

    }
}
