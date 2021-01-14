using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldAbilities : MonoBehaviour
{
    public Transform carryParent;
    private Movement movement;
    public TextMeshProUGUI inGameText;
    private Rigidbody rb;
    private bool carrying = false;
    private Animator anim;
    private Rescuee rescuee;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<Movement>();
        inGameText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Action()
    {
        if (rescuee != null)
        {
            if (!carrying) // pickup
            {
                rescuee.Pickup();
                anim.SetLayerWeight(1,1);
                rescuee.transform.SetParent(carryParent);
                rescuee.transform.localPosition = new Vector3(0.09f, -1.19f, 0.38f);
                rescuee.transform.localRotation = Quaternion.Euler(-2.52f, -14.82f, 3.52f);
                carrying = true;
                inGameText.enabled = false;
            }
            else // drop kick
            {
                if (movement.Grounded)
                    anim.SetTrigger("Kick");
                rescuee.transform.SetParent(null);
                rescuee.Toss(rb.velocity + carryParent.forward * 10);
                anim.SetLayerWeight(1,0);
                carrying = false;
                rescuee = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Rescuee newRescuee;
        try {
            newRescuee = other.transform.parent.parent.GetComponent<Rescuee>();
        } catch 
        {
            newRescuee = other.GetComponent<Rescuee>();
        }
        if (newRescuee != null && !carrying && !newRescuee.safe)
        {
            inGameText.enabled = true;
            inGameText.text = "Press E to Pick Up!";
            rescuee = newRescuee;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rescuee newRescuee;
        try {
            newRescuee = other.transform.parent.parent.GetComponent<Rescuee>();
        } catch 
        {
            newRescuee = other.GetComponent<Rescuee>();
        }
        if (newRescuee != null)
        {
            inGameText.enabled = false;
            rescuee = null;
        }
    }
}
