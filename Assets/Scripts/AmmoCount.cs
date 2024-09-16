using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCount : MonoBehaviour
{
    public Text ammunitionText;
    public Text magText;
   
    public static AmmoCount occurrence;

    private void Awake()
    {
        occurrence = this;
    }

    public void UpdateAmmoText(int presentAmmunition)
    {
        ammunitionText.text = presentAmmunition + "/" ;
    }

    public void UpdateMagText(int Mag)
    {
        magText.text = Mag.ToString();
    }
}
