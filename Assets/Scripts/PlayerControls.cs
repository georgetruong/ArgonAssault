using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [Header("General Setup Settings")]
    [Tooltip("How fast ship moves up and down based upon player input")]
    [SerializeField] float controlSpeed = 50f;
    [Tooltip("Horizontal movement limit of player")]
    [SerializeField] float xRange = 20f;
    [Tooltip("Vertical movement limit of player")]
    [SerializeField] float yRange = 10f;

    [Header("Screen position based tuning")]
    [Tooltip("Amount vertical position changes pitch")]
    [SerializeField] float positionPitchFactor = -1f;
    [Tooltip("Amount horizontal positon changes yaw")]
    [SerializeField] float positionYawFactor = -.25f;

    [Header("Player input based tuning")]
    [Tooltip("Amount player movement changes roll")]
    [SerializeField] float controlRollFactor = -20f;
    [Tooltip("Amount player movement changes pitch")]
    [SerializeField] float controlPitchFactor = -20f;

    [Header("Laser gun array")]
    [Tooltip("Add all lasers here")]
    [SerializeField] GameObject[] lasers;

    // Use new Input System
    [Header("Player Input System Settings")]
    [Tooltip("Controls to move player horizontally and vertically")]
    [SerializeField] InputAction movement;
    [Tooltip("Controls to fire lasers")]
    [SerializeField] InputAction fire;

    float xThrow, yThrow;

    // Start is called before the first frame update
    void Start() {
        
    }

    void OnEnable() {
        movement.Enable();
        fire.Enable();
    }

    void OnDisable() {
        movement.Disable();    
        fire.Disable();
    }

    // Update is called once per frame
    void Update() {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    void ProcessTranslation() {
        // Old Input Manager
        // xThrow = Input.GetAxis("Horizontal");
        // yThrow = Input.GetAxis("Vertical");

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

    void ProcessFiring() {
        /** Old Input Manager
        if (Input.GetButton("Fire1")) {
            Debug.Log("Firing");
        } else {
            Debug.Log("Not Firing");
        }
        **/

        // New Input System
        if (fire.ReadValue<float>() > 0.5) {
            SetLasersActive(true);
        } else {
            SetLasersActive(false);
        }
    }

    void SetLasersActive(bool isActive) {
        foreach(GameObject laser in lasers) {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }

}
