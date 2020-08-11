using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickDetector : MonoBehaviour
{
    [SerializeField]
    EventSystem system;

    public static ClickDetector clickDetector;

    Camera cam;
    IInteractable currentInteractable;
    GameObject currentObject;
    public bool clickDown;
    public bool overGUI;

    private void Awake()
    {
        clickDetector = this;
    }

    // Start is called before the first frame update
    public void Start()
    {
        cam = Camera.main;
        currentInteractable = null;
        currentObject = null;
        clickDown = false;
    }


    // Update is called once per frame
    private void Update()
    {
        if (UnityEngine.Input.GetMouseButtonDown(0))
        {
            clickDown = true;
        }
        overGUI = system.IsPointerOverGameObject();
    }

    void FixedUpdate()
    {
        Ray ray = cam.ScreenPointToRay(UnityEngine.Input.mousePosition);
        RaycastHit2D hit;

        hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        IInteractable interactable = null;

        if(overGUI)
        {
            clickDown = false;
            return;
        }
            


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
            if (clickDown)
            {
                interactable.MouseButtonDown();
                clickDown = false;
            }

            interactable.MouseEnter();
        }
   

        currentInteractable = interactable;

        if (clickDown)
        {
            SimulationController.singleton.ClearSelection();
        }

        clickDown = false;



   
    }
}
