using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour {
    public GameObject effect1;
    public GameObject effect2;
    public GameObject effect3;
    public GameObject effect4;
    public GameObject effect5;
    public GameObject effect6;
    public GameObject effect7;
    public GameObject lid;

    // Use this for initialization
    void Start () {
        effect1.SetActive(false);
        effect2.SetActive(false);
        effect3.SetActive(false);
        effect4.SetActive(false);
        effect5.SetActive(false);
        effect6.SetActive(false);
        effect7.SetActive(false);
    }
	

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            effect1.SetActive(true);
            effect2.SetActive(true);
            effect3.SetActive(true);
            effect4.SetActive(true);
            effect5.SetActive(true);
            effect6.SetActive(true);
            effect7.SetActive(true);
            lid.GetComponent<Animator>().SetBool("opening", true);
        }
    }
}
