using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public GameObject Inventory;
    private void Start()
    {
        Inventory.SetActive(false);
        SelectWeapon();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("tab down");
            Inventory.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            Inventory.SetActive(false);
        }

        SelectWeapon();
        
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == WeaponSwitching.instance.selectedWeapon)
                weapon.gameObject.SetActive(true);

            else

                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
