using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoManager : MonoBehaviour
{
    public Material effectMat;
    [Range(0.5f,5.0f)]
    public float width;
    public float speed;
    public float sharpness;
    Camera _cam;

    private void Start()
    {
        _cam = GetComponent<Camera>();
        width = 0.5f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector4 mousePosAtClick;
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                mousePosAtClick = hit.point;
                CreateEchoEffect(mousePosAtClick, width);
            }
        }
    }

    public void CreateEchoEffect(Vector4 originPos, float width)
    {
        EchoImageEffect newEchoImageEffect = this.gameObject.AddComponent<EchoImageEffect>();
        newEchoImageEffect.EffectMaterial = effectMat;
        newEchoImageEffect.Origin = originPos;
        newEchoImageEffect.Width = width;
        newEchoImageEffect.LeadingEdgeSharpness = sharpness;
    }
}
