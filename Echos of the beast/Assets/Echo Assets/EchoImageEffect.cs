using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoImageEffect : MonoBehaviour
{
    [SerializeField]
    private Vector4 origin;
    [SerializeField]
    private Material effectMat;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float width;
    [SerializeField]
    private float maxRange;
    [SerializeField]
    private float leadingEdgeSharpness;

    EchoManager em;
    bool scanning;

    private void Awake()
    {
        em = GetComponent<EchoManager>();
        scanning = true;
    }


    private float distance;

    public Vector4 Origin
    {
        set
        {
            origin = value;
        }
    }

    public Material EffectMaterial
    {
        set
        {
            effectMat = value;
        }
    }

    public float Speed
    {
        set
        {
            speed = value;
        }
    }

    public float Width
    {
        set
        {
            width = value;
        }
    }    

    public float MaxRange
    {
        set
        {
            maxRange = value;
        }
    }

    public float LeadingEdgeSharpness
    {
        set
        {
            leadingEdgeSharpness = value;
        }
    }

    private Camera _cam;

    void OnEnable()
    {
        _cam = GetComponent<Camera>();
        _cam.depthTextureMode = DepthTextureMode.Depth;
        Invoke("EndEffect", 2);
    }

    private void Update()
    {
        distance += Time.deltaTime * speed;
        if(scanning)
        {
            foreach (GlowingObject g in em.glowers)
            {
                if (Vector3.Distance(origin, g.transform.position) <= distance)
                {
                    g.DoGlow();
                }
            }
        }
    }

    private void EndEffect()
    {
        scanning = false;
        foreach (GlowingObject g in em.glowers)
        {
            g.EndGlow();
        }
        Destroy(this);
    }

    [ImageEffectOpaque]
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {        
        effectMat.SetVector("_WorldSpaceScannerPos", origin);
        effectMat.SetFloat("_EchoDistance", distance);
        effectMat.SetFloat("_EchoWidth", width);
        effectMat.SetFloat("_LeadSharpness", leadingEdgeSharpness);
        effectMat.SetFloat("_EchoRange", maxRange);
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
