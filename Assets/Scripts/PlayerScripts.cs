using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScripts : MonoBehaviour
{
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        shoot();
    }

    private void shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo; //RaycastHit hit to store hit collider
            Ray ray = cam.ScreenPointToRay(Input.mousePosition); //Ray ray to store ray where we point the ray out from camera
            
            if(Physics.Raycast(ray, out hitInfo,50f))
            {
                //hitInfo have collider information
                if (hitInfo.collider != null)
                {
                    //find whether collider hit have IHitable script
                    IHitable[] hitables = hitInfo.collider.GetComponents<IHitable>();

                    if (hitables != null && hitables.Length > 0)
                    {
                        foreach (var hitable in hitables)
                        {
                            //if so, trigger function hit inside hitables object
                            hitable.Hit(hitInfo);
                        }
                    }
                    
                    Debug.Log(hitInfo.collider.gameObject.name);
                }
            }
        }
    }
}
