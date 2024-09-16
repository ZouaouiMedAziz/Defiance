using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Slider healthBar;

    public void GiveFullHealth (float Health)
    {
        healthBar.maxValue = Health;
        healthBar.value = Health;
    }

    public void SetHealth(float Health)
    {
        healthBar.value = Health;
    }

}
