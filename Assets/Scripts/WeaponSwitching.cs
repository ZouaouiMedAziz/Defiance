using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{

    public int selectedWeapon = 0;
    public int allWeapons = 0;
    public Animator anim;
    public static WeaponSwitching instance;
    public GameObject[] WeaponsInventory;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        SelectWeapon();
    }

    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= allWeapons) selectedWeapon = 0;
            else
                selectedWeapon++;


            if (selectedWeapon > 0)
                anim.SetTrigger("SwapWeapon");
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = allWeapons;
            else
                selectedWeapon--;

            if (selectedWeapon > 0)
                anim.SetTrigger("SwapWeapon");
        }


        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
        if (selectedWeapon == 0)
        {
            anim.SetBool("Idle", true);
            anim.SetBool("Rifle", false);
            anim.SetBool("Pistol", false);
        }
        else if (selectedWeapon == 1)
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Rifle", false);
            anim.SetBool("Pistol", true);
        }
        else if (selectedWeapon > 1)
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Rifle", true);
            anim.SetBool("Pistol", false);
        }
    }


    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);

            else

                weapon.gameObject.SetActive(false);
            i++;
        }
    }

}