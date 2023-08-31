using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] private int currentWeapon = 0;
    List<GameObject> weapons = new List<GameObject>();

    void Start()
    {
        int weaponIndex = 0;
        foreach (Transform weapon in transform)
        {
            if (weaponIndex == currentWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }

            weapons.Add(weapon.gameObject);
            weaponIndex++;
        }
    }

    void Update()
    {
        ProcessKeyInput();
        ProcessScrollWheel();
    }

    private void ProcessKeyInput()
    {
        // Switch weapon
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(2);
        }
    }

    private void ProcessScrollWheel()
    {
        // Forward
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f) 
        {
            if (currentWeapon == 0)
            {
                SwitchWeapon(weapons.Count - 1);
            }
            else
            {
                SwitchWeapon(currentWeapon - 1);
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f) 
        {
            if (currentWeapon == weapons.Count - 1)
            {
                SwitchWeapon(0);
            }
            else
            {
                SwitchWeapon(currentWeapon + 1);
            }
        }
    }

    private void SwitchWeapon(int weaponIndex)
    {
        if (weaponIndex != currentWeapon)
        {
            weapons[currentWeapon].SetActive(false);
            weapons[weaponIndex].SetActive(true);
            currentWeapon = weaponIndex;
        }
    }
}
