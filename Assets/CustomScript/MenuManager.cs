using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    public GameObject sceneObject;

    private bool isMenuOn;
    private GameObject menu;
    private GameObject hideMenuButton;
    private float alphaValue = 1;
    private Vector3 originalMenuPosition;
    private Vector3 originalMenuScale;


    public void OnClicked(string name)
    {
        Debug.Log("The clicked object's name is " + name);
        sceneObject = GameObject.Find("Scene");
        Debug.Log("**** The Gameobject found is " + sceneObject.name);

        if(name == "HideMenuButton" || name == "HideMenuButtonText")
        {
            ToggleMenu();
            return;
        }
        else if(name == "BedRoomOption" || name == "GymOption" || name == "KitchenOption" || name == "OfficeOption")
        {
            ToggleMenu();
        }
        sceneObject.SendMessageUpwards("MenuClicked",name);
    }

    public void ToggleMenu()
    {
        if (isMenuOn) {
            isMenuOn = false;
            StartCoroutine(FadeOutMenu());
        }
        else
        {
            isMenuOn = true;
            StartCoroutine(FadeInMenu());
        }
    }

    IEnumerator FadeOutMenu()
    {
        menu.GetComponent<BoxCollider>().enabled = false;
        
        while(alphaValue > 0)
        {
            foreach(MeshRenderer renderer in menu.gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                // do not fade the overlay
                if (renderer.gameObject.name == "Overlay") continue;

                renderer.material.color -= new Color(0, 0, 0, 0.1f);
                alphaValue = renderer.material.color.a;
            }

            menu.gameObject.transform.localPosition = Vector3.Lerp(menu.transform.localPosition, hideMenuButton.transform.localPosition, 0.3f);
            menu.gameObject.transform.localScale = Vector3.Lerp(menu.transform.localScale, new Vector3(0f, 0f, 0f), 0.3f);
            yield return new WaitForSeconds(Time.deltaTime * 0.5f);
        }

        menu.SetActive(false);

        yield return null;
    }

    IEnumerator FadeInMenu()
    {
        menu.SetActive(true);

        while (alphaValue < 1)
        {
            foreach (MeshRenderer renderer in menu.gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                // do not fade the overlay
                if (renderer.gameObject.name == "Overlay") continue;

                renderer.material.color += new Color(0, 0, 0, 0.1f);
                alphaValue = renderer.material.color.a;
            }

            menu.gameObject.transform.localPosition = Vector3.Lerp(menu.transform.localPosition, originalMenuPosition, 0.3f);
            menu.gameObject.transform.localScale = Vector3.Lerp(menu.transform.localScale, originalMenuScale, 0.3f);

            yield return new WaitForSeconds(Time.deltaTime * 0.5f);
        }

        menu.gameObject.transform.localPosition = originalMenuPosition;
        menu.gameObject.transform.localScale = originalMenuScale;


        menu.GetComponent<BoxCollider>().enabled = true;
        yield return null;
    }

	// Use this for initialization
	void Start () {
        isMenuOn = true;
        menu = transform.Search("Menu").gameObject;
        hideMenuButton = transform.Search("HideMenuButton").gameObject;
        originalMenuPosition = menu.transform.localPosition;
        originalMenuScale = menu.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {

    }


}
