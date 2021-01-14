using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rescuee : MonoBehaviour
{
    public AudioClip[] audioClips;
    public Transform hips;
    private Animator anim;
    private Ragdoll ragdoll;
    private CapsuleCollider collider;
    private Rigidbody rb;
    private AudioSource audioSource;
    public bool safe = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        ragdoll = GetComponentInChildren<Ragdoll>();
        collider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pickup()
    {
        ragdoll.SetRagdoll(false);
        anim.enabled = true;
        transform.position = hips.position;
        hips.localPosition = Vector3.zero;
        anim.SetBool("carried", true);
        rb.isKinematic = true;
        collider.enabled = false;
        audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.Play();
    }
    
    public void Toss(Vector3 momentum)
    {
        anim.SetBool("carried", false);
        anim.enabled = false;
        StartCoroutine(ragdoll.DontHitJordan());
        ragdoll.SetRagdoll(true);
        ragdoll.AddMomentum(momentum, true);
    }

    public IEnumerator Safe()
    {
        yield return new WaitUntil(() => !ragdoll.IsMoving());
        ragdoll.SetRagdoll(false);
        anim.enabled = true;
        transform.position = hips.position;
        hips.localPosition = Vector3.zero;
        anim.SetTrigger("celebrate");
        rb.isKinematic = false;
        collider.enabled = true;
        safe = true;
    }


}
