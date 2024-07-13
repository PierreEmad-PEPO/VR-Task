using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    static InteractionManager instance;
    
    PickUpManager pickUpManager;

    bool stopInteraction;

    float elapsedTime, totalTime;
    

    public static InteractionManager Instance
    {
        get
        {
            if (instance == null) 
                instance = FindObjectOfType<InteractionManager>();
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pickUpManager = GetComponent<PickUpManager>();
        stopInteraction = true;
        elapsedTime = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (elapsedTime < totalTime) elapsedTime += Time.deltaTime;
        if (elapsedTime >= totalTime) 
        {
            stopInteraction = false;
        }

        if (!stopInteraction && Input.GetMouseButtonDown(0)) 
        {
            RaycastHit hit = InputManager.Instance.GetMouseRaycast();         

            if (hit.transform != null)
            {
                if (hit.transform.CompareTag("TeleportZone"))
                {
                    transform.position = hit.transform.position;
                    TaskManager.Instance.CheckIfCurTaskDone(TaskEnum.Teleport, hit.transform.gameObject);
                }
                else if (pickUpManager.PickedItem == null && hit.transform.CompareTag("Pickable"))
                {
                    pickUpManager.PickUp(hit.transform.gameObject);
                    TaskManager.Instance.CheckIfCurTaskDone(TaskEnum.PickUp, hit.transform.gameObject);
                }
                else if (pickUpManager.PickedItem != null && hit.transform.gameObject != pickUpManager.PickedItem)
                {
                    if (hit.transform.CompareTag("DropZone"))
                    {
                        TaskManager.Instance.CheckIfCurTaskDone(TaskEnum.Drop, pickUpManager.PickedItem);
                        pickUpManager.Drop();
                    }
                    else
                    {
                        pickUpManager.Drop(hit.point);
                    }
                }
            }
            
        }
    }

    public void StopInteractionForSeconds(float seconds)
    {
        if (stopInteraction) return;
        stopInteraction = true;
        elapsedTime = 0;
        totalTime = seconds;
    }
}
