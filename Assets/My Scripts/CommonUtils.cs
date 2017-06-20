using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MyCommonUtils : MonoBehaviour {

    public static void DestroyImmediateAllChildrenOf(Transform t)
    {
        var children = new List<GameObject>();
        foreach (Transform child in t) children.Add(child.gameObject);
        children.ForEach(child => DestroyImmediate(child));
    }

    public static void DestroImmediateyAllChildrenOf(GameObject g)
    {
        DestroyImmediateAllChildrenOf(g.transform);
    }

    public static void DestroyAllChildrenOf(Transform t)
    {
        var children = new List<GameObject>();
        foreach (Transform child in t) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
    }

    public static void DestroyAllChildrenOf(GameObject g)
    {
        DestroyAllChildrenOf(g.transform);
    }

    public static bool EditEmissionColor(GameObject gameObject, Color color)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material mat = renderer.material;
            mat.SetColor("_EmissionColor", color);
            return true;
        }
        else
        {
            Debug.LogWarning("Gameobject " + gameObject + "\nCannont find renderer to emit color.");
            return false;
        }
    }

    public static bool EditAlbedoColor(GameObject gameObject, Color color)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            var tempMaterial = new Material(renderer.sharedMaterial);
            tempMaterial.color = Color.red;
            tempMaterial.SetColor("_Color", color);
            renderer.sharedMaterial = tempMaterial;
            return true;
        }
        else
        {
            Debug.LogWarning("Gameobject " + gameObject + "\nCannont find renderer to edit color.");
            return false;
        }
    }




}
