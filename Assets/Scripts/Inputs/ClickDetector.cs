using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDetector : MonoBehaviour
{
    [SerializeField]
    EventSystem system;

    Camera cam;
    IInteractable currentInteractable;
    GameObject currentObject;
    bool click;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        currentInteractable = null;
        currentObject = null;
        click = false;
    }


    // Update is called once per frame
    private void Update()
    {
        if (UnityEngine.Input.GetMouseButtonDown(0))
        {
            click = true;
        }
    }

    void FixedUpdate()
    {
        Ray ray = cam.ScreenPointToRay(UnityEngine.Input.mousePosition);
        RaycastHit2D hit;

        hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);




        IInteractable interactable = null;

        //TODO ADD CHECK TO SEE IF CURSOR IS OCCUPIED


        if (hit)
        {
            interactable = hit.transform.GetComponent<IInteractable>();
            currentObject = hit.transform.gameObject;
        }
            
        if(currentInteractable != interactable && currentInteractable != null)
        {
            currentInteractable.MouseExit();
        }
        if(interactable != null)
        {
            if (click)
            {
                interactable.MouseClick();
                click = false;
            }

            interactable.MouseEnter();
        }
   

        currentInteractable = interactable;
        click = false;


   
    }
}
