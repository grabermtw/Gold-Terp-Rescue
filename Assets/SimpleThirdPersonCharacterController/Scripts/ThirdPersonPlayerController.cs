using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonPlayerController : MonoBehaviour
{
    public AudioClip[] audioClips;
    public Transform hips;
    private CameraController cameraController;
    public float gamepadLookSensititvity = 20;
    public float mouseLookSensitivity = 15;
    public GameObject getUpText;
    public Transform camParent; 
    
    private AudioSource audioSource;
    private Rigidbody rb;
    private Animator animator; 
    private Ragdoll ragdoll;
    private Movement movement;
    private GoldAbilities abilities;
    private Vector2 look = Vector2.zero;
    private Vector2 move = Vector2.zero;
    private bool canRevive = false;
    private bool dead = false;
    private bool following = false;

    // --------------- INPUT EVENTS -------------
    // these are all called by the PlayerInput component based on the InputActions asset.
    // Read about the PlayerInput component here: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.PlayerInput.html
    
    public void OnMovement(InputAction.CallbackContext ctx)
    {   
        if (ctx.performed && !dead)
            move = ctx.ReadValue<Vector2>();
        else if (ctx.canceled)
            move = Vector2.zero;
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            look = ctx.ReadValue<Vector2>() * (ctx.control.device is Gamepad ? Time.deltaTime * gamepadLookSensititvity : mouseLookSensitivity);
        else if (ctx.canceled)
            look = Vector2.zero;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (dead && canRevive)
                Revive();
            else {
                movement.Jump(true);
            }
        }
        else if (ctx.canceled)
            movement.Jump(false);
    }

    public void OnUtility(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !dead) // when the button is pressed down
        {
            // Do whatever you want this button to do when it is pressed
            if (abilities != null)
            {
                abilities.Action();
            }
        }
        else if (ctx.canceled)
        {
            // Do whatever you want this button to do when it is released
        }
        
    }
    
    // ------------ END INPUT EVENTS -----------

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ragdoll = GetComponentInChildren<Ragdoll>();
        cameraController = GetComponentInChildren<CameraController>();
        movement = GetComponent<Movement>();
        abilities = GetComponent<GoldAbilities>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        movement.AddMovement(move.x, move.y);
        movement.RotateCamera(look.x, look.y);
    }

    public Transform GetHips()
    {
        return hips;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("wine"))
        {
            Die(other.gameObject.GetComponent<Rigidbody>().velocity);
        }
    }

    public void Die(Vector3 newMomentum) // handle if AddMomentum should be called
    {
        abilities.Action();
        animator.enabled = false;
        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        ragdoll.SetRagdoll(true);
        Vector3 momentum;
        momentum = newMomentum;

        ragdoll.AddMomentum(momentum, false);
        dead = true;
        canRevive = false;

        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.Play();
        
        StartCoroutine(RevivalEnabler());
        StartCoroutine(FollowRagdoll());
    }

    private IEnumerator RevivalEnabler()
    {
        // Don't allow a revival until we stop moving on the ground
        yield return new WaitUntil(() => !ragdoll.IsMoving());
        canRevive = true;
        getUpText.SetActive(true);
    }

    public virtual void Revive()
    {
        if (dead && canRevive)
        {
            ragdoll.SetRagdoll(false);
            animator.enabled = true;
            rb.isKinematic = false;
            GetComponent<Collider>().enabled = true;
            transform.position = hips.position;
            hips.localPosition = Vector3.zero;
            dead = false;
            getUpText.SetActive(false);
        }
    }

    // Start this to have the camera follow the character when they ragdoll
    // Start this again to stop following
    public IEnumerator FollowRagdoll()
    {
        Debug.Log("FollowRagdoll");
        if (following)
        {
            Debug.Log("Stop Following!");
            following = false;
            yield break;
        }
        following = true;
        while (following)
        {
            
            camParent.position = hips.position;
            Debug.Log("our pos: " + camParent.position);
            Debug.Log("character pos: " + hips.position);
            yield return null;
        }
        Debug.Log("End!");
        camParent.localPosition = new Vector3(0, 1.5f, 0);
    }
}
