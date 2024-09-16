using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Add this for UI components
using TMPro; // Make sure to include this namespace

public class ZoneFolder : MonoBehaviour
{
    public GameObject Zone;
    public GameObject ZoneActive;
    public GameObject document; // The document GameObject that will appear
   // public TMP_Text interactText; // UI Text that will display the pickup message (drag and drop in the inspector)
                              // Start is called before the first frame update
    public bool isInZone = false; // Track if the dog is in the zone
     public GameObject objectToHighlight; // The object you want to highlight
     private Renderer objectRenderer; // Renderer component of the object
    private Color originalColor; // To store the original color of the object
    private Material objectMaterial; // Reference to the object's material
        private Vector3 originalScale; // Store the original scale of the object

     public Color highlightColor = Color.yellow; // The highlight color (yellow by default)
    public float emissionIntensity = 0.5f; // Emission intensity for glowing effect
    public float scaleMultiplier = 1.2f; // Scale increase when highlighted
    private void Start()
    {
        // Get the Renderer component from the object
         if (objectToHighlight != null)
        {
            objectRenderer = objectToHighlight.GetComponent<Renderer>();

            // Store the original color and material
            if (objectRenderer != null)
            {
                objectMaterial = objectRenderer.material;
                originalColor = objectMaterial.color;
                originalScale = objectToHighlight.transform.localScale;

                // Highlight the object initially by setting emission and scale
                HighlightObject(true);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("dog"))
        {
            isInZone = true; // The dog is now in the zone
            //interactText.text = "Press E to pick up the document"; // Set the interaction text
            //interactText.gameObject.SetActive(true);
            Zone.SetActive(false);
            ZoneActive.SetActive(true);


        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("dog"))
        {
            isInZone = false; // The dog has left the zone
            //interactText.gameObject.SetActive(false); // Hide the interaction text
        }
    }

    private void Update()
    {
       PickDoc();
    }

    public void PickDoc()
    {
         if (isInZone && Input.GetKeyDown(KeyCode.E)) // Check if in zone and 'E' is pressed
        {
            objectToHighlight.SetActive(false); 
            document.SetActive(true); // Show the document
            //interactText.gameObject.SetActive(false); // Hide the interaction text after picking up
        }
    }
      public void PickDocVoice()
    {
         if (isInZone ) // Check if in zone and 'E' is pressed
        {
            Debug.Log("ey true!");
            objectToHighlight.SetActive(false);
            document.SetActive(true); // Show the document
           // interactText.gameObject.SetActive(false); // Hide the interaction text after picking up
        }
    }
      public void HighlightObject(bool highlight)
    {
        if (objectRenderer != null)
        {
            if (highlight)
            {
                // Set object to highlighted color with emission glow and scale up
                objectMaterial.color = highlightColor;
                objectMaterial.EnableKeyword("_EMISSION");
                objectMaterial.SetColor("_EmissionColor", highlightColor * emissionIntensity);
                objectToHighlight.transform.localScale = originalScale * scaleMultiplier; // Enlarge object
            }
            else
            {
                // Revert object color, emission, and scale
                objectMaterial.color = originalColor;
                objectMaterial.DisableKeyword("_EMISSION");
                objectMaterial.SetColor("_EmissionColor", Color.black);
                objectToHighlight.transform.localScale = originalScale; // Reset to original scale
            }
        }
    }
}
