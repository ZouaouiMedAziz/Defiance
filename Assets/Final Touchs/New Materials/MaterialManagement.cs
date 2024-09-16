using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManagement : MonoBehaviour
{
    public Material mat;
    public Renderer lightrend;
    // Start is called before the first frame update
    void Start()
    {
        lightrend = GetComponent<Renderer>();
        lightrend.material = mat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
