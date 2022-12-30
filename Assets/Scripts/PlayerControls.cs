using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    // Use new Input System
    [SerializeField] InputAction movement;
    [SerializeField] float controlSpeed = 50f;
    [SerializeField] float xRange = 20f;
    [SerializeField] float yRange = 10f;

    [SerializeField] float positionPitchFactor = -1f;
    [SerializeField] float controlPitchFactor = -20f;
    [SerializeField] float positionYawFactor = -.25f;
    [SerializeField] float controlRollFactor = -20f;
    
    float xThrow, yThrow;

    // Start is called before the first frame update
    void Start() {
        
    }

    void OnEnable() {
        movement.Enable();
    }

    void OnDisable() {
        movement.Disable();    
    }

    // Update is called once per frame
    void Update() {
        ProcessTranslation();
        ProcessRotation();
    }

    void ProcessTranslation() {
        // Old Input Manager
        // float horizontalThrow = Input.GetAxis("Horizontal");
        // float verticalThrow = Input.GetAxis("Vertical");

        // New Input System
        xThrow = movement.ReadValue<Vector2>().x;
        yThrow = movement.ReadValue<Vector2>().y;

        float xOffset = xThrow * Time.deltaTime * controlSpeed;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        float yOffset = yThrow * Time.deltaTime * controlSpeed;
        float rawYPos = transform.localPosition.y + yOffset;

        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    void ProcessRotation() {
        float pitchDueToPosition = (transform.localPosition.y / 2) * positionPitchFactor;
        float pitchDuetoControlThrow = yThrow * controlPitchFactor;

        float pitch = pitchDueToPosition + pitchDuetoControlThrow; 
        float yaw = transform.localPosition.x * positionYawFactor; 
        float roll = xThrow * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }
}