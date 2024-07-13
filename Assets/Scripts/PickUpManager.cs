using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PickUpManager : MonoBehaviour
{
    [SerializeField] Transform inHandTransform;
    [SerializeField] float movingSpeed;
    [SerializeField] float rotatingSpeed;
    [SerializeField] float scrollingSpeed;
    [SerializeField] GameObject dropZonePrefab;

    GameObject pickedItem;
    Vector3 initialPos;
    Transform initialParent;
    Quaternion initialRotation;
    GameObject dropZone;
    Vector3 moveTarget;
    UnityAction actionAfterMoving;
    Vector3 mouseLastPos;

    public GameObject PickedItem { get { return pickedItem; } }

    // Start is called before the first frame update
    void Start()
    {
        actionAfterMoving = null;
        moveTarget = Vector3.down;  // the down value indicates no target
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
            mouseLastPos = Input.mousePosition;

        if (pickedItem != null && moveTarget != Vector3.down)
        {
            MoveToTarget();
        }
        else if (pickedItem != null && Input.GetMouseButton(1))
        {
            InputManager.Instance.StopCameraRotation = true;
            HandleRotating();
        }
        else if (pickedItem != null && Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            HandleScrolling();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            InputManager.Instance.StopCameraRotation = false;
        }
    }

    void MoveToTarget()
    {
        // Reach the target
        if (Vector3.Distance(pickedItem.transform.position, moveTarget) < 0.01f)
        {
            actionAfterMoving?.Invoke();
            moveTarget = Vector3.down;
            
            return;
        }

        pickedItem.transform.position = Vector3.MoveTowards(pickedItem.transform.position, moveTarget, movingSpeed * Time.deltaTime);
    }

    void HandleRotating()
    {
        // Calculate the rotation based on mouse movement
        Vector3 rot = mouseLastPos - Input.mousePosition;
        rot = Time.deltaTime * rotatingSpeed * rot.normalized;

        if (Mathf.Abs(rot.x) >= Mathf.Abs(rot.y))
        {
            Vector3 rotationAxis = transform.TransformDirection(Vector3.up);
            pickedItem.transform.Rotate(Vector3.up, rot.x, Space.World);
        }
        else
        {
            Vector3 rotationAxis = transform.TransformDirection(Vector3.right);
            pickedItem.transform.Rotate(rotationAxis, -rot.y, Space.World);
        }

        mouseLastPos = Input.mousePosition;
    }

    void HandleScrolling()
    {
        inHandTransform.Translate(Input.GetAxis("Mouse ScrollWheel")
            * scrollingSpeed * Time.deltaTime * transform.forward);
    }

    public void PickUp(GameObject toPick)
    {
        moveTarget = inHandTransform.position;
        pickedItem = toPick;
        pickedItem.layer = LayerMask.NameToLayer("Ignore Raycast");
        if (dropZone == null || dropZone.IsDestroyed())
        {
            BoxCollider col = pickedItem.GetComponent<BoxCollider>();
            initialPos = pickedItem.transform.position;
            initialParent = pickedItem.transform.parent;
            initialRotation = pickedItem.transform.rotation;
            Vector3 dropPos = pickedItem.transform.TransformPoint(col.center) + Vector3.down * col.size.y/2 + Vector3.up * dropZonePrefab.transform.localScale.y;
            dropZone = Instantiate(dropZonePrefab, dropPos, pickedItem.transform.rotation);
            Vector3 newScale = new Vector3(col.size.x, dropZone.transform.localScale.y, col.size.z);
            dropZone.transform.localScale = newScale;
        }
        pickedItem.transform.parent = inHandTransform;
        actionAfterMoving = ResetTarget;
    }

    public void Drop()
    {
        moveTarget = initialPos;
        Destroy(dropZone);
        actionAfterMoving = ResetPickedItem;
    }

    public void Drop(Vector3 hitPos)
    {
        moveTarget = hitPos;
        moveTarget.y = hitPos.y + pickedItem.GetComponent<BoxCollider>().size.y;
        actionAfterMoving = ResetPickedItem;
    }

    void ResetTarget()
    {
        moveTarget = Vector3.down;
    }

    void ResetPickedItem()
    {
        pickedItem.transform.parent = initialParent;
        pickedItem.transform.rotation = initialRotation;
        pickedItem.layer = LayerMask.NameToLayer("Default");
        pickedItem = null;
        ResetTarget();
    }
}
