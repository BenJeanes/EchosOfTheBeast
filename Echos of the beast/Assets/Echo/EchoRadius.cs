using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoRadius : MonoBehaviour {

    public Transform EchoOrigin;
    public Material EchoMaterial;

    public float EchoDistance;

    //Shader properties

    public float travelRate;
    public float range;
    public float fadeRate;
    public float width;

    private Camera _cam;

    bool _echoActive;
        
    void Update()
    {
        if(_echoActive)
        {
            EchoDistance += Time.deltaTime * travelRate;            
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _echoActive = true;
            EchoDistance = 0;
        }

        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit))
            {
                _echoActive = true;
                EchoDistance = 0;
                EchoOrigin.position = hit.point;
            }
        }
    }
    
    void OnEnable()
    {
        _cam = GetComponent<Camera>();
        _cam.depthTextureMode = DepthTextureMode.Depth;
    }

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        EchoMaterial.SetVector("_WorldSpaceScannerPos", EchoOrigin.position);
        EchoMaterial.SetFloat("_EchoDistance", EchoDistance);
        EchoMaterial.SetFloat("_EchoRange", range);
        EchoMaterial.SetFloat("_EchoWidth", width);
        EchoMaterial.SetFloat("_EchoFadeRate", fadeRate);
        RaycastCornerBlit(src, dst, EchoMaterial);
    }

    void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat)
    {
        // Compute Frustum Corners
        float camFar = _cam.farClipPlane;
        float camFov = _cam.fieldOfView;
        float camAspect = _cam.aspect;

        float fovWHalf = camFov * 0.5f;

        Vector3 toRight = _cam.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
        Vector3 toTop = _cam.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

        Vector3 topLeft = (_cam.transform.forward - toRight + toTop);
        float camScale = topLeft.magnitude * camFar;

        topLeft.Normalize();
        topLeft *= camScale;

        Vector3 topRight = (_cam.transform.forward + toRight + toTop);
        topRight.Normalize();
        topRight *= camScale;

        Vector3 bottomRight = (_cam.transform.forward + toRight - toTop);
        bottomRight.Normalize();
        bottomRight *= camScale;

        Vector3 bottomLeft = (_cam.transform.forward - toRight - toTop);
        bottomLeft.Normalize();
        bottomLeft *= camScale;

        // Custom Blit, encoding Frustum Corners as additional Texture Coordinates
        RenderTexture.active = dest;

        mat.SetTexture("_MainTex", source);

        GL.PushMatrix();
        GL.LoadOrtho();

        mat.SetPass(0);

        GL.Begin(GL.QUADS);

        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.MultiTexCoord(1, bottomLeft);
        GL.Vertex3(0.0f, 0.0f, 0.0f);

        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.MultiTexCoord(1, bottomRight);
        GL.Vertex3(1.0f, 0.0f, 0.0f);

        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.MultiTexCoord(1, topRight);
        GL.Vertex3(1.0f, 1.0f, 0.0f);

        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.MultiTexCoord(1, topLeft);
        GL.Vertex3(0.0f, 1.0f, 0.0f);

        GL.End();
        GL.PopMatrix();
    }
}
