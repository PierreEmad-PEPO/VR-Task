using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float mouseSensitivity;

    float horizontalAngle, verticalAngle;

    // Start is called before the first frame update
    void Start()
    {
        horizontalAngle = transform.localEulerAngles.y;
        verticalAngle = transform.localEulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        float turnPlayer = InputManager.Instance.MouseQuarterHorizontalAxis * mouseSensitivity * Time.deltaTime;
        horizontalAngle += turnPlayer;

        if (horizontalAngle > 360) horizontalAngle -= 360.0f;
        if (horizontalAngle < 0) horizontalAngle += 360.0f;

        // Camera look up/down
        float turnCam = -InputManager.Instance.MouseQuarterVerticalAxis * mouseSensitivity * Time.deltaTime;
        verticalAngle = Mathf.Clamp(turnCam + verticalAngle, -89.0f, 89.0f);

        Vector3 currentAngles = transform.localEulerAngles;
        currentAngles.y = horizontalAngle;
        currentAngles.x = verticalAngle;
        transform.localEulerAngles = currentAngles;
    }
}
