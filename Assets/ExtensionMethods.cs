﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    /// <summary>
    /// Depth-First Search through all children and return an Array of children that have specified tag.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gameObject"></param>
    /// <param name="tag"></param>
    /// <returns></returns>
    public static T[] GetComponentsInChildrenWithTag<T>(this GameObject gameObject, string tag)where T: Component
    {
        List<T> results = new List<T>();

        if (gameObject.CompareTag(tag))
            results.Add(gameObject.GetComponent<T>());

        foreach (Transform t in gameObject.transform)
            results.AddRange(t.gameObject.GetComponentsInChildrenWithTag<T>(tag));

        return results.ToArray();
    }

    /// <summary>
    /// Depth-First Search through all the children to find the first children that matches the name
    /// </summary>
    /// <param name="target"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Transform Search(this Transform target, string name)
    {
        if (target.name == name) return target;

        for(int i = 0; i < target.childCount; ++i)
        {
            var result = Search(target.GetChild(i), name);

            if (result != null) return result;

        }

        return null;
    }


    /// <summary>
    /// Find deeper child by name, using breadth-first search
    /// </summary>
    /// <param name="aParent"></param>
    /// <param name="aName"></param>
    /// <returns></returns>
    public static Transform FindDeepChild(this Transform aParent, string aName)
    {
        var result = aParent.Find(aName);
        if (result != null)
            return result;
        foreach (Transform child in aParent)
        {
            result = child.FindDeepChild(aName);
            if (result != null)
                return result;
        }
        return null;
    }

}
