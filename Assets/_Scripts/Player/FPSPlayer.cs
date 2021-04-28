using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FPSPlayer : MonoBehaviour
{
    [Header("Online Settings")]
    public PhotonView pv;
    public GameObject[] fpAssets;
    public GameObject[] tpAssets;

    [Header("Player Settings")]
    [SerializeField] float moveSpeed = 5f;  // THE PLAYERS MOVEMENTSPEED
    public float mouseSensitivityX = 100f;
    public float mouseSensitivityY = 100f;
    public float sensitivityMultiplyer = 1f;
    public float gravity = -5f;

    [Header("First Person Settins")]
    public ShootingSystem ss;
    public Animator fpsAnim;
    public Animator aimAnim;
    public Vector3 normalPos;
    public Vector3 crouchPos;

    [Header("Third person Settings")]
    public Animator thirdPersonAnim;
    public FPSCamera fpsCam;
    [SerializeField] Transform lookObjHead;
    [SerializeField] Transform lookObjBody;
    [SerializeField] Vector3 offset;

    [Header("Player States")]
    public bool sprinting;
    public bool crouching;

    [Header("RayCast Settings")]
    [SerializeField] bool useRayCasting;    // ENABLED THE USE OF RAYCAST IN THE FPSCAMERA SCRIPT
    public float rangeCameraRay = 4;

    [HideInInspector] public PlayerControls pc;
    [HideInInspector] public Pause pausescript;
    private int footStepInterval;
    CharacterController cc;
    Transform chest;

    void Start()
    {
        if (ss.currentGun == null)
        {
            Debug.LogError("There should ALWAYS be a currentgun in ShootingSystem.cs");
        }
        cc = GetComponent<CharacterController>();
        pc = GetComponent<PlayerControls>();
        pv = GetComponent<PhotonView>();
        pausescript = GetComponent<Pause>();

        ss.SwapWeapon(ss.currentGun);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        chest = thirdPersonAnim.GetBoneTransform(HumanBodyBones.Chest);

        if (pv.IsMine)
        {
            foreach (var t in tpAssets)
            {
                t.SetActive(false);
            }
            foreach (var f in fpAssets)
            {
                f.SetActive(true);
            }
            fpsCam.GetComponent<Camera>().enabled = true;
            fpsCam.GetComponent<AudioListener>().enabled = true;
        }
        else
        {
            foreach (var f in fpAssets)
            {
                f.SetActive(false);
            }
        }
    }
    
    void Update()
    {
        if (!pv.IsMine)
        {
            return;
        }
        if (useRayCasting)
        {
            fpsCam.Raycast();
        }
        Gun();
        Crouching();
        Move();
    }

    private void LateUpdate()
    {
        chest.LookAt(lookObjBody.position);
        chest.rotation = chest.rotation * Quaternion.Euler(offset);
    }
    
    private void Gun()
    {
        ss.ShootMechanic();
    }
    
    void ExitCrouch()
    {
        crouching = false;
        thirdPersonAnim.SetBool("crouch", crouching);
        StartCoroutine(LerpObject(fpsCam.gameObject, normalPos));
    }

    private void Crouching()
    {
        if (Input.GetKeyDown(pc.crouch))
        {
            crouching = !crouching;
            thirdPersonAnim.SetBool("crouch", crouching);
            if (crouching == true)
            {
                Debug.Log("Called");
                StartCoroutine(LerpObject(fpsCam.gameObject, crouchPos));
            }
            else
            {
                ExitCrouch();
            }
        }
    }
    
    private void Move() // THE MOVEMENT OF THE CHARACTER WITH WASD AND ARROW KEYS
    {
        Vector3 ySpeed = Vector3.zero;
        Vector3 xSpeed = Vector3.zero;

        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        
        if (Input.GetKey(pc.sprint) && inputY == 1 && ss.currentGun.reloading == false)
        {
            sprinting = true;
            if (crouching)
            {
                ExitCrouch();
            }
            ySpeed = (transform.forward * Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
            xSpeed = (transform.right * Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime);
        }
        else
        {
            sprinting = false;
            ySpeed = (transform.forward * Input.GetAxis("Vertical") * (moveSpeed / 3) * Time.deltaTime);
            xSpeed = (transform.right * Input.GetAxis("Horizontal") * (moveSpeed / 3) * Time.deltaTime);
        }
        thirdPersonAnim.SetBool("sprinting", sprinting);
        thirdPersonAnim.SetFloat("y", inputX);
        thirdPersonAnim.SetFloat("x", inputY);

        fpsAnim.SetBool("sprinting", sprinting);

        Vector3 gravityCalculation = new Vector3(0, gravity, 0);

        Vector3 motion = ySpeed + xSpeed + gravityCalculation;

        cc.Move(motion);

        if (pausescript.paused == false)
        {
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime);
        }
    }
    
    IEnumerator LerpObject(GameObject lerpedObject, Vector3 to)
    {
        float elapsedTime = 0;
        float waitTime = 0.5f;
        Vector3 from = lerpedObject.transform.localPosition;

        while (elapsedTime < waitTime)
        {
            Debug.Log("Lerping");
            lerpedObject.transform.localPosition = Vector3.Lerp(from, to, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
        // Make sure we got there
        lerpedObject.transform.localPosition = to;
        yield return null;
    }
}