using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [Header("Camera to Assign")]
    public GameObject AimCam;
    public GameObject AimCanvas;
    public GameObject ThirdPersonCam;
    public GameObject ThirdPersonCanvas;

    [Header("Camera Animator")]
    public Animator animator;

    void Update()
    {
        if(Input.GetButton("Fire2") && Input.GetKey(KeyCode.Z) )
        {

            animator.SetBool("Aim", true);


            ThirdPersonCam.SetActive(false);
            ThirdPersonCanvas.SetActive(false);
            AimCam.SetActive(true);
            AimCanvas.SetActive(true);

        }
       else if (Input.GetButton("Fire2"))
        {
            animator.SetBool("Aim", true);


            ThirdPersonCam.SetActive(false);
            ThirdPersonCanvas.SetActive(false);
            AimCam.SetActive(true);
            AimCanvas.SetActive(true);


        }
        else
        {
            animator.SetBool("Aim", false);


            ThirdPersonCam.SetActive(true);
            ThirdPersonCanvas.SetActive(true);
            AimCam.SetActive(false);
            AimCanvas.SetActive(false);
        }
    }
}
