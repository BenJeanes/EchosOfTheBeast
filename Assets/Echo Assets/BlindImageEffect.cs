using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindImageEffect : MonoBehaviour {

    public Material blindMaterial;
    [Range(0,1)]
    public float visibility = 1;
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //blindMaterial.SetFloat("_Visibility", visibility);
        Graphics.Blit(source, destination, blindMaterial);        
    }
}
