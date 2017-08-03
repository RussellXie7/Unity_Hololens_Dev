using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSubobjects : MonoBehaviour {

    public GameObject spawnPlace;
    public GameObject toSpawn;

	// Use this for initialization
	void Start () {
        StartCoroutine(WaitAndSpawn());
	}
	
    IEnumerator WaitAndSpawn()
    {
        yield return new WaitForSeconds(2f);
        GameObject spawned = Instantiate(toSpawn, Vector3.zero, Quaternion.identity);
        spawned.transform.parent = this.gameObject.transform;
        spawned.transform.localPosition = spawnPlace.transform.localPosition;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
