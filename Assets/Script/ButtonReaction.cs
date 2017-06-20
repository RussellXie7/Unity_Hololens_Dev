using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonReaction : MonoBehaviour {
    public Material mat;
    private Material savedMat;
    private Color initColor;

    public Renderer rendererToChange;
    public Text textToChangeColor;
    public TextMesh TextMeshToChangeColor;


	// Use this for initialization
	void Start () {
        savedMat = rendererToChange.material;
        if (textToChangeColor != null) initColor = textToChangeColor.color;
        if (TextMeshToChangeColor != null) initColor = TextMeshToChangeColor.color;

    }
	
    public void CursorHover()
    {
        rendererToChange.material = mat;
        if(textToChangeColor != null) textToChangeColor.color = Color.red;
        if (TextMeshToChangeColor != null) TextMeshToChangeColor.color = Color.red;
    }

    public void CursorExit()
    {
        rendererToChange.material = savedMat;
        if (textToChangeColor != null) textToChangeColor.color = initColor;
        if (TextMeshToChangeColor != null) TextMeshToChangeColor.color = initColor;
    }
}
