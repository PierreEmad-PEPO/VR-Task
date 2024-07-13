using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    static InputManager instance;

    [SerializeField] float rayLength;
    Camera mainCamera;

    // UI
    [SerializeField] Image laserPointer;
    Vector2 mouseUIPosition;
    Vector2 topLeftLimit, bottomRightLimit;



    public static InputManager Instance
    {
        get 
        {
            if (instance == null)
                instance = FindObjectOfType<InputManager>();
            return instance;
        }
    }

    public bool StopCameraRotation {  get; set; }

    public float MouseQuarterVerticalAxis
    {
        get
        {
            if (StopCameraRotation) return 0;

            if (Input.mousePosition.y >= topLeftLimit.y)
                return (Input.mousePosition.y - topLeftLimit.y) / (Screen.height / 4); // [0,1]
            
            if (Input.mousePosition.y <= bottomRightLimit.y)
                return (Input.mousePosition.y - bottomRightLimit.y) / (Screen.height / 4); // [-1,0]

            return 0;
        }
    }

    public float MouseQuarterHorizontalAxis
    {
        get
        {
            if (StopCameraRotation) return 0;

            if (Input.mousePosition.x >= bottomRightLimit.x)
                return (Input.mousePosition.x - bottomRightLimit.x) / (Screen.width / 4); // [0,1]
            
            if (Input.mousePosition.x <= topLeftLimit.x)
                return (Input.mousePosition.x - topLeftLimit.x) / (Screen.width / 4); // [-1,0]

            return 0;
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        laserPointer.rectTransform.position = Vector2.zero;
        topLeftLimit.x = Screen.width / 4; topLeftLimit.y = Screen.height / 4 * 3;
        bottomRightLimit.x = Screen.width / 4 * 3; bottomRightLimit.y = Screen.height / 4;
    }

    public RaycastHit GetMouseRaycast()
    {
        RaycastHit hit;
        Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, rayLength);

        Button uiButton;
        if (hit.transform != null && hit.transform.TryGetComponent<Button>(out  uiButton))
        {
            uiButton.onClick.Invoke();
        }

        return hit;
    }

    private void LateUpdate()
    {
        HandleLaserPointer();
    }

    void HandleLaserPointer()
    {
        mouseUIPosition = Input.mousePosition;
        laserPointer.rectTransform.sizeDelta = new Vector2(laserPointer.rectTransform.sizeDelta.x, mouseUIPosition.magnitude);
        laserPointer.rectTransform.eulerAngles = new Vector3(0, 0, -Mathf.Atan2(mouseUIPosition.x, mouseUIPosition.y) * Mathf.Rad2Deg);
    }
}
