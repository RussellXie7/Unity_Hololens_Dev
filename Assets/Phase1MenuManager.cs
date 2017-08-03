using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
public class Phase1MenuManager : MonoBehaviour, IInputClickHandler
{
    private bool isMenuOn;
    private GameObject menu;
    private GameObject hideMenuButton;
    private float alphaValue = 1;
    private Vector3 originalMenuPosition;
    private Vector3 originalMenuScale;

    // Use this for initialization
    void Start()
    {
        isMenuOn = true;
        menu = transform.Search("Menu").gameObject;
        hideMenuButton = transform.Search("HideMenuButton").gameObject;
        originalMenuPosition = menu.transform.localPosition;
        originalMenuScale = menu.transform.localScale;

        GazeManager.Instance.FocusedObjectChanged += Instance_FocusedObjectChanged;
    }

    private void Instance_FocusedObjectChanged(GameObject previousObject, GameObject newObject)
    {
        
        if(previousObject!=null && previousObject.name == "HideMenuButton")
        {
            hideMenuButton.GetComponent<ButtonInteractive>().OnHoverExit();
        }

        if(newObject!=null && newObject.name == "HideMenuButton")
        {
            hideMenuButton.GetComponent<ButtonInteractive>().OnHover();
        }
    }

    public void OnInputClicked(InputClickedEventData data)
    {
        GameObject focusedGameobject = GazeManager.Instance.HitInfo.transform.gameObject;
        OnClicked(focusedGameobject.name);
    }

    public void OnClicked(string name)
    {
        Debug.Log("The clicked object's name is " + name);

        if (name == "HideMenuButton" || name == "HideMenuButtonText")
        {
            ToggleMenu();
            return;
        }
        else if (name == "StartOption" || name == "AddRestOption" || name == "AddGymOption")
        {
            ToggleMenu();
            
        }

        switch (name)
        {
            
            case "StartOption":
                GameObject.Find("HologramCollection").transform.Search("BuildingGroup").gameObject.GetComponent<HoloToolkit.Sharing.Tests.BuildingGroupManager>().AddMainBuilding();
                break;
            case "AddRestOption":
                GameObject.Find("HologramCollection").transform.Search("BuildingGroup").gameObject.GetComponent<HoloToolkit.Sharing.Tests.BuildingGroupManager>().AddRestaurant();
                break;
            case "AddGymOption":
                GameObject.Find("HologramCollection").transform.Search("BuildingGroup").gameObject.GetComponent<HoloToolkit.Sharing.Tests.BuildingGroupManager>().AddGym();
                break;
        }

    }

    public void ToggleMenu()
    {
        if (isMenuOn)
        {
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

        while (alphaValue > 0)
        {
            foreach (MeshRenderer renderer in menu.gameObject.GetComponentsInChildren<MeshRenderer>())
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

    // Update is called once per frame
    void Update()
    {

    }


}
