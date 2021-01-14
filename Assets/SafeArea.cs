using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    public GameObject gameFinishText;
    private List<Rescuee> rescuees;

    // Start is called before the first frame update
    void Start()
    {
        rescuees = new List<Rescuee>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }


    void OnTriggerEnter(Collider other)
    {
        Rescuee newRescuee;
        try {
            newRescuee = other.transform.parent.parent.GetComponent<Rescuee>();
        } catch 
        {
            newRescuee = other.GetComponent<Rescuee>();
        }
        if (newRescuee != null && !rescuees.Contains(newRescuee))
        {
            rescuees.Add(newRescuee);
            StartCoroutine(newRescuee.Safe());
            Debug.Log("Got one!");
            if (rescuees.Count == 3)
            {
                gameFinishText.SetActive(true);
            }
        }
    }
}
