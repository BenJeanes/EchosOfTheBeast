using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindImageEffect : MonoBehaviour {

    public Material blindMaterial;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, blindMaterial);        
    }
}
