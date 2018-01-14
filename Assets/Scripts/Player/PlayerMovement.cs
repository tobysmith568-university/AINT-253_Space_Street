using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Transform player; 
    Rigidbody mainRigidBody;
    [SerializeField]
    Animator topAnimator;
    [SerializeField]
    Transform topTransform;
    [SerializeField]
    Transform bottomTransform;
    [SerializeField]
    Collider bottomCollider;

    [SerializeField]
    float lookSensitivityX = 3f;
    [SerializeField]
    float lookSensitivityY = 3f;

    [SerializeField]
    float movementSpeedForward = 5f;
    [SerializeField]
    float movementSpeedSideways = 5f;
    [SerializeField]
    float movementSpeedSprint = 7.5f;

    [SerializeField]
    float jumpPower = 5;

    [SerializeField]
    Text openText;
    GameObject currentDoor;
    [SerializeField]
    LayerMask doors;
    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    AudioSource doorHiss;
    
    float rotationX = 0F;
    float rotationY = 0F;
    Quaternion originalRotation;
    float distToGround;
    bool isCrouching;
    bool isFloating;
    bool invertX;
    bool invertY;

    void Start()
    {
        //NEEDS MOVING TO A MAIN GAME CONTROLLER
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        player = GetComponent<Transform>();
        mainRigidBody = GetComponent<Rigidbody>();
        originalRotation = transform.localRotation;
        distToGround = bottomCollider.bounds.extents.y;

        //Find PlayPrefs
        invertX = MyPrefs.XAxisInverted;
        invertY = MyPrefs.YAxisInverted;
        lookSensitivityX = MyPrefs.XSensitivity;
        lookSensitivityY = MyPrefs.YSensitivity;
    }

    void Update()
    {
        //Game quitting ---------------- PROBABLY NEEDS MOVING TO A UI SCRIPT
        if (MyInput.GetButtonDown(Control.Pause))
            SceneManager.LoadScene(0, LoadSceneMode.Single);

        //Walking and sprinting
        if (MyInput.GetButton(Control.Forward))
            transform.Translate(Vector3.forward * ((Input.GetKey(KeyCode.LeftShift) && !isCrouching && IsGrounded() && !isFloating) ? movementSpeedSprint : movementSpeedForward) * Time.deltaTime);
        if (MyInput.GetButton(Control.Backward))
            transform.Translate(Vector3.back * movementSpeedForward * Time.deltaTime);
        if (MyInput.GetButton(Control.Left))
            transform.Translate(Vector3.left * movementSpeedSideways * Time.deltaTime);
        if (MyInput.GetButton(Control.Right))
            transform.Translate(Vector3.right * movementSpeedSideways * Time.deltaTime);
        
        //Jumping
        if (MyInput.GetButtonDown(Control.Jump) && IsGrounded() && !isCrouching && !isFloating)
            mainRigidBody.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);

        ////Crouching
        //if (MyInput.GetButtonDown(Control.Crouch) && IsGrounded() && !isFloating)
        //{
        //    isCrouching = true;
        //    topAnimator.SetTrigger("Crouch");
        //}
        //else if (MyInput.GetButtonUp(Control.Crouch) && IsGrounded())
        //{
        //    isCrouching = false;
        //    topAnimator.SetTrigger("Stand");
        //}

        //Looking X axis
        rotationX += Input.GetAxis("Mouse X") * lookSensitivityX;
        rotationX = ClampAngle(rotationX, -360F, 360F);
        Quaternion xQuaternion = Quaternion.AngleAxis((invertX) ? -rotationX : rotationX, Vector3.up);
        player.localRotation = originalRotation * xQuaternion;

        //Looking Y axis
        rotationY += Input.GetAxis("Mouse Y") * lookSensitivityY;
        rotationY = ClampAngle(rotationY, -40F, 80F);
        Quaternion yQuaternion = Quaternion.AngleAxis((invertY) ? -rotationY : rotationY, -Vector3.right);
        topTransform.localRotation = originalRotation * yQuaternion;

        RaycastHit raycastHit;
        Animator a;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out raycastHit, 1.5f, doors))
        {
            GameObject g = raycastHit.transform.gameObject;
            if (LayerMask.LayerToName(g.layer) == "Bunker Door")
            {
                openText.text = "You cannot open the bunker door!";
            }
            else
            {
                if (LayerMask.LayerToName(g.layer) == "Airlock Door")
                    g = g.transform.parent.parent.gameObject;

                a = g.GetComponent<Animator>();

                if (a.GetBool("isOpen"))
                    openText.text = "Click to close: " + g.name;
                else
                    openText.text = "Click to open: " + g.name;

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    a.SetBool("isOpen", !a.GetBool("isOpen"));
                }
            }
        }
        else
            openText.text = "";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Water")
            isFloating = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water")
            isFloating = false;
    }

    /// <summary>
    /// Ensures a given angle is clamped between two bounds
    /// </summary>
    /// <param name="angle">The angle to clam</param>
    /// <param name="min">The lowest the angle can be</param>
    /// <param name="max">The highest the angle can be</param>
    /// <returns>The new clamped angle</returns>
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    /// <summary>
    /// Returns if the player is on the floor
    /// </summary>
    /// <returns>True if the player is not in mid-air</returns>
    bool IsGrounded()
    {
        return Physics.Raycast(bottomTransform.position, -Vector3.up, distToGround + 0.0001f);
    }

    /// <summary>
    /// Used by other objects, this slows or re-speeds up the look sensitivity while scoped
    /// </summary>
    /// <param name="isScoped"></param>
    public void SetScoped(bool isScoped)
    {
        if (isScoped)
        {
            lookSensitivityX = lookSensitivityY *= 8;
        }
        else
        {
            lookSensitivityX = lookSensitivityY /= 8;
        }
    }

    public void DoorHiss()
    {
        doorHiss.Play();
    }
}
