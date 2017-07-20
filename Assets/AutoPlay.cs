using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlay : MonoBehaviour {

    public GameObject player;
    public AudioSource clip;
    public GameObject collectionManager;

    private bool triggered = false;


	// Use this for initialization
	void Start () {
        triggered = false;
        clip = transform.GetComponent<AudioSource>();
        player = GameObject.Find("HoloLensCamera");
	}
	
    void CheckDistance()
    {
        Debug.Log("Distance is " + Vector3.Distance(player.transform.position, transform.position));
        if (Vector3.Distance(player.transform.position, transform.position) < 0.5f)
        {
            
            triggered = true;
            Transform child = transform.GetChild(0);
            child.gameObject.SetActive(false);
            StartCoroutine(TriggerMusic());
        }
    }

    IEnumerator TriggerMusic()
    {
        yield return new WaitForSeconds(3);
        clip.Play();
        collectionManager = GameObject.Find("Scene");
        collectionManager.SendMessageUpwards("AudioIsPlaying");
    }

    public void StopMusic()
    {
        if (clip.isPlaying)
        {
            clip.Stop();
        }
    }

	// Update is called once per frame
	void Update () {
        if (!triggered)
        {
            CheckDistance();
        }
        else
        {
            Debug.Log("Triggered is " + triggered);
        }
	}
}
