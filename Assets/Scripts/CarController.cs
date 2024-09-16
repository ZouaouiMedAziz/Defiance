using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Wheels colliders")]
    public WheelCollider frontRightWheelCollider;
    public WheelCollider frontLeftWheelCollider;
    public WheelCollider backRightWheelCollider;
    public WheelCollider backLeftWheelCollider;

    [Header("Wheels Transforms")]
    public Transform frontRightWheelTransform;
    public Transform frontLeftWheelTransform;
    public Transform backRightWheelTransform;
    public Transform backLeftWheelTransform;
    public Transform carDoor;
    public Transform playerDog;

    [Header("Car Engine")]
    public float accelerationForce = 100f;
    private float presentAcceleration = 0f;
    public float breakingForce = 200f;
    private float presentBreakForce = 0f;
    public GameObject carCamera;

    [Header("Car Steering")]
    public float WheelsTorque = 20f;
    private float presentTurnAngle = 0f;

    [Header("Car Security")]
    public PlayerScript player;
    public DogScript Dog;
    private float radius = 3f;
    private bool isOpened = false;

    [Header("Disable Thnigs")]
    public GameObject Crosshair;
    public GameObject playerCamera;
    public GameObject playerCharacter;
    public GameObject thirdPersonCam;
    public GameObject AimCam;
    public GameObject AimCrosshair;
    public GameObject DogCharacter;



    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                isOpened = true;
                radius = 5000f;
                playerCharacter.SetActive(false);
                DogCharacter.SetActive(false);
                


            }
            else if (Input.GetKeyDown(KeyCode.G))
            {
                player.transform.position = carDoor.transform.position;
                Dog.transform.position = playerDog.transform.position;
                isOpened = false;
                radius = 5f;
                playerCharacter.SetActive(true);
                DogCharacter.SetActive(true);

            }
        }
        if(isOpened == true)
        {
            Crosshair.SetActive(false);
            playerCamera.SetActive(false);
            carCamera.SetActive(true);

            MoveCar();
            CarSteering();
            ApplyBreaks();
        }
        else if (isOpened == false)
        {
            playerCamera.SetActive(true);
            carCamera.SetActive(false);
        }

      
    }

    void MoveCar()
    {


        frontRightWheelCollider.motorTorque = presentAcceleration;
        frontLeftWheelCollider.motorTorque = presentAcceleration;
        backRightWheelCollider.motorTorque = presentAcceleration;
        backLeftWheelCollider.motorTorque = presentAcceleration;
        presentAcceleration = accelerationForce * Input.GetAxis("Vertical");

    }

    void CarSteering()
    {
        presentTurnAngle = WheelsTorque * Input.GetAxis("Horizontal");
        frontRightWheelCollider.steerAngle = presentTurnAngle;
        frontLeftWheelCollider.steerAngle = presentTurnAngle;

        SteeringWheels(frontRightWheelCollider, frontRightWheelTransform);
        SteeringWheels(frontLeftWheelCollider, frontLeftWheelTransform);
        SteeringWheels(backRightWheelCollider, backRightWheelTransform);
        SteeringWheels(backLeftWheelCollider, backLeftWheelTransform);
    }

    void SteeringWheels(WheelCollider WC, Transform WT)
    {
        Vector3 position;
        Quaternion rotation;

        WC.GetWorldPose(out position, out rotation);

        WT.position = position; 
        WT.rotation = rotation;
    }

    void ApplyBreaks()
    {
        if (Input.GetKey(KeyCode.Space))
            presentBreakForce = breakingForce;

        else
            presentBreakForce = 0f;

        frontRightWheelCollider.brakeTorque = presentBreakForce;
        frontLeftWheelCollider.brakeTorque = presentBreakForce;
        backRightWheelCollider.brakeTorque = presentBreakForce;
        backLeftWheelCollider.brakeTorque = presentBreakForce;

    }
}
