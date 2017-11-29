using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffectScript : MonoBehaviour
{
   
    public Vector4 origin;
    
    public Material effectMat;
    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private float distance;
    [SerializeField]
    private float width = 2;

    bool _active;

    private Camera _cam;

    void OnEnable()
    {
        _cam = GetComponent<Camera>();
        _cam.depthTextureMode = DepthTextureMode.Depth;
    }

    private void Update()
    {
        distance += Time.deltaTime * speed;
    }

    [ImageEffectOpaque]
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        effectMat.SetVector("_WorldSpaceScannerPos", origin);
        effectMat.SetFloat("_EchoDistance", distance);
        effectMat.SetFloat("_EchoWidth", 2);
        RaycastCornerBlit(source, destination, effectMat);
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
