using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("tab down");
            gameObject.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
                gameObject.SetActive(false);
        }
    }
}
