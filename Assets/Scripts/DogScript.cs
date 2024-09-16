using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class DogScript : MonoBehaviour
{
    public Transform TargetLookMoving, TargetLookStop, TargetFollow, TargetFollowM, TargetFollowS;
    public float Dspeed = 0f, distance = 1;
    public Animator animatorD;
    public Animator animatorP;

    private bool isFollowingPlayer = true; // New flag to track if the dog should follow the player or only respond to voice


    //VOICE
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    private bool isTurning = false;
    private Quaternion targetRotation;
    private float turnSpeed = 2.0f; // Adjust the turning speed as necessary

    public Text scoreText, commentaires, finalScore;
    public CharacterController controller;
    public float ActSpeed, Speed = 0f;
    public float horizontal;
    public float vertical;
    public Animator animator;
    public AudioSource Breack;
    private float moveDuration = 2f; // Duration the dog should move forward (in seconds)
    private bool isMoving = false; // A flag to check if the dog is currently moving
        public ZoneFolder zoneFolder; // Reference to ZoneFolder script to interact with document pickup

    Vector3 vector3 = Vector3.forward;
    private Vector3[] orientations = new Vector3[] { Vector3.forward, Vector3.right, -Vector3.forward, Vector3.left };
    public string[] Demande = new string[] { "devant", "droite", "tourne", "gauche" };

    public Vector3[][] matrix = new Vector3[][] {
        new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward },
        new Vector3[] { Vector3.right, -Vector3.forward, Vector3.left, Vector3.forward },
        new Vector3[] { -Vector3.forward, Vector3.left, Vector3.forward, Vector3.right },
        new Vector3[] { Vector3.left, Vector3.forward, Vector3.right, -Vector3.forward }
    };

    public float timeValue = 180;
    
  
   

    void Start()
    {

        //voice
        actions.Add("devant", Front);
        actions.Add("bouge", Move);
        actions.Add("avance", Move);
        actions.Add("tourne", Turn);
        actions.Add("droite", Right);
        actions.Add("gauche", Left);
        actions.Add("stop", Stop);
        actions.Add("plus vite", Fast);
        actions.Add("vite", Fast);
        actions.Add("moin vite", Slow);
        actions.Add("prends le document", PickDocument);


        if (actions.Count > 0)
        {
            keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
            keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
            keywordRecognizer.Start();

            // Add log to confirm KeywordRecognizer started
            Debug.Log("KeywordRecognizer started successfully.");
        }
        else
        {
            Debug.LogWarning("No voice commands found to initialize KeywordRecognizer.");
        }
    
}

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log("Recognized voice command: " + speech.text);  // Check if this is logging

        if (actions.ContainsKey(speech.text))
        {
            actions[speech.text].Invoke();  // This should invoke the action (e.g., Front, Turn)
            Debug.Log("Action invoked for: " + speech.text);  // Add an additional log to see if the action is invoked
        }
        else
        {
            Debug.LogWarning("Unrecognized command: " + speech.text);
        }
    }
        private void PickDocument()
    {

      

      zoneFolder.PickDocVoice(); // Call the PickDoc method in ZoneFolder script
        Debug.Log("hez!");
        
     
    }
    public void StartKeywordRecognizer()
    {
        Debug.Log("keywordRecognizer."+ keywordRecognizer);
        Debug.Log("keywordRecognizer.IsRunning." + keywordRecognizer.IsRunning);

        if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        {
            keywordRecognizer.Start();
            Debug.Log("KeywordRecognizer started.");
        }
    }



    public bool IsKeywordRecognizerRunning()
    {
        return keywordRecognizer != null && keywordRecognizer.IsRunning;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowingPlayer)
        {
            if (animatorD.GetFloat("Dspeed") == 0 || animatorP.GetFloat("vInput") < 0 || (animatorP.GetFloat("vInput") < 0 && animatorP.GetFloat("hzInput") == 1))
        {
            transform.LookAt(TargetLookStop);
            TargetFollow = TargetFollowS;
            distance = 3;
        }
        else
        {
            transform.LookAt(TargetLookMoving);
            TargetFollow = TargetFollowM;
            distance = 1;
        }
        if (animatorP.GetFloat("vInput") < 0 && animatorP.GetFloat("hzInput") == 1)
        {
            transform.LookAt(TargetLookStop);
            TargetFollow = TargetFollowS;
            distance = 4;
        }
        if (animatorP.GetFloat("hzInput") != 0)
        {
            transform.LookAt(TargetLookMoving);
            TargetFollow = TargetFollowM;
            distance = 3;
        }
        if (Vector3.Distance(transform.position, TargetFollow.position) > distance)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetFollow.position, Time.deltaTime * Dspeed);
            transform.position = new Vector3 (transform.position.x, 0, transform.position.z);
        }
        if (animatorP.GetFloat("vInput") == 0 && animatorP.GetFloat("hzInput") == 0 && Vector3.Distance(transform.position, TargetFollow.position) <= 1.0f)
        {
            Dspeed = 0;
            animatorD.SetFloat("Dspeed", Dspeed);
        }
        if (animatorP.GetBool("Walking") == true)
        {
            Dspeed = 3;
            animatorD.SetFloat("Dspeed", Dspeed);
            animatorD.SetBool("Turn", true);
        }
        if (animatorP.GetBool("Running") == true)
        {
            Dspeed = 5;
            animatorD.SetFloat("Dspeed", Dspeed);
        }

        //VVVVVVVVVOOOOOOOOOOOOOOOOOIIIIIIIIIIIIIIIIICCCCCCCCCCCCCCCCEEEEEEEEE

        
        if (Speed > 1f)
        {
            ActSpeed = Speed;
        }
        horizontal = vector3.z;
        vertical = vector3.x;
        }
        if (!isFollowingPlayer)
        {
            PlayerMove();
        }

        if (isTurning)
        {
            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);

            // Check if the rotation is nearly complete
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                transform.rotation = targetRotation; // Snap to the final rotation
                isTurning = false;
            }
        }
    }
    void PlayerMove()
    {
        // Use the dog's forward direction for movement
        Vector3 direction = transform.forward.normalized; // Use the dog's actual forward direction

        // Create a LayerMask to exclude the CameraZone layer
        int layerMask = ~(1 << LayerMask.NameToLayer("CameraZone")); // Inverts the mask to include all layers except CameraZone

        // Cast a ray forward to check if there is an obstacle in front of the dog, excluding the CameraZone
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Check if there's an obstacle within a short range (e.g., 1 unit in front), excluding the CameraZone
        if (Physics.Raycast(ray, out hit, 1f, layerMask))
        {

            // Stop the dog from moving if there's an obstacle
            animator.SetFloat("Dspeed", 0);  // Stop animation when hitting an obstacle
            return;
        }

        // Apply movement to the CharacterController if there's no obstacle
        if (direction.magnitude > 0.1f)
        {
            // Move in the direction the dog is facing (world space)
            controller.Move(direction * Speed * Time.deltaTime);
        }

        // Update the dog's walking animation based on the speed
        animator.SetFloat("Dspeed", Speed);
    }

    public Vector3 GetVectorFromMatrix(Vector3 currentOrientation, string action)
    {
        int x = System.Array.IndexOf(orientations, currentOrientation);
        int y = System.Array.IndexOf(Demande, action);
        return matrix[y][x];
    }
    // MOVE
    private void Front()
    {
        vector3 = Vector3.forward;  // Forward movement
        Debug.Log("Dog moving forward.");
    }

    // Turn around (180 degrees from the current forward direction)
    private void Turn()
    {
        transform.Rotate(0, 180, 0);  // Rotate the dog by 180 degrees
        vector3 = transform.forward;  // Update vector3 to reflect the new forward direction
        Debug.Log("Dog turned around. New forward direction: " + transform.forward);
    }

    // Rotate to the right (90 degrees)
    private void Right()
    {
        if (!isTurning)
        {
            // Set the target rotation for a 90-degree right turn
            targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + 90, transform.eulerAngles.z);
            isTurning = true;
            Debug.Log("Dog is turning right.");
        }
    }

    // Rotate to the left (90 degrees)
    private void Left()
    {
        if (!isTurning)
        {
            // Set the target rotation for a 90-degree left turn
            targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y - 90, transform.eulerAngles.z);
            isTurning = true;
            Debug.Log("Dog is turning left.");
        }
    }
    private void Stop()
    {
        Speed = 0;
    }
    private void Fast()
    {
        if (Speed > 0)
        {
            Speed += 2f;
        }
    }
    private void Slow()
    {
        if (Speed > 0)
        {
            Speed -= 1f;
        }
    }

    private void Move()
    {
        if (!isMoving) // Only start moving if not already moving
        {
            Speed = 2; // Start moving with speed
            StartCoroutine(AdvanceAndStop()); // Start the Coroutine to move forward for a while and stop
        }
    }

    private IEnumerator AdvanceAndStop()
    {
        isMoving = true; // Set the moving flag to true

        // Wait for the moveDuration (e.g., 2 seconds)
        yield return new WaitForSeconds(moveDuration);

        Speed = 0; // Stop the movement
        isMoving = false; // Reset the moving flag to false
    }
    private void Setspeed()
    {
        Speed = ActSpeed;
    }


    public void EnableFollowing()
    {
        isFollowingPlayer = true;
    }

    public void DisableFollowing()
    {
        isFollowingPlayer = false;
        Dspeed = 0; // Stop any automatic movement
        animatorD.SetFloat("Dspeed", Dspeed);
    }

   /* private void OnTriggerEnter(Collider other)
    {
        if 
    }
   */

}
