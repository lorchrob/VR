/* Tal Rastopchin
 * February 20, 2020
 * 
 * A script to get input from a single SteamVR CameraRigController
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Assign this script as a script component of a GameObject. Then,
 * drag either of the left or right controller child objects from
 * the CameraRig prefab into the public controller variable.
 */
public class MyMonoBehaviour : MonoBehaviour
{
    // CameraRig controller
    public GameObject controller;
    private SteamVR_TrackedObject trackedController;
    private SteamVR_Controller.Device device;

    // buttonmask for ButtonB and ButtonA (work for buttons X and Y too)
    private const ulong ButtonB = SteamVR_Controller.ButtonMask.ApplicationMenu;
    private const ulong ButtonA = 128ul;

    Rigidbody rigidBody;
    [SerializeField] float rcsThrust = 250f;
    [SerializeField] float mainThrust = 50f;

    private void Start()
    {
        // get a reference to the SteamVR_TrackedObject component
        //  of the controller GameObject
        trackedController = controller.GetComponent<SteamVR_TrackedObject>();

        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Assigns the SteamVR_Controller.Device objects to the
        // corresponding controller device
        device = SteamVR_Controller.Input((int)trackedController.index);

        // getting all sorts of button presses looks like this
        // you can get most buttons via the SteamVR_Controller.ButtonMask struct
        bool pressDownTrigger = device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger);
        bool pressTrigger = device.GetPress(SteamVR_Controller.ButtonMask.Trigger);
        bool pressUpTrigger = device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger);

        // if you need the A or B buttons, use the const ulong fields
        bool pressButtonB = device.GetPress(ButtonB);
        bool pressButtonA = device.GetPress(ButtonA);

        // getting smoothed controller joystick looks like this
        float joystickX = Smooth(device.GetAxis().x);
        float joystickY = Smooth(device.GetAxis().y);

        // your code here --------------------------------------------------------------------------------
        // can thrust while rotating
        if (pressTrigger)
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        }

        // take manual control of rotation
        rigidBody.freezeRotation = true;
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (device.GetAxis().x < 0)
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (device.GetAxis().x > 0)
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        // resume physics control of rotation
        rigidBody.freezeRotation = false;
    }

    // a simple smoothing model to smooth joystick axis input in
    // a float domain of [-1, 1]
    private float Smooth(float input)
    {
        float sign = (input > 0) ? 1 : -1;
        return sign * Mathf.Pow(input, 2);
    }
}