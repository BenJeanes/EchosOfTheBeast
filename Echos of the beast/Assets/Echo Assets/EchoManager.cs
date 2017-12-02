using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoManager : MonoBehaviour
{
    public Material effectMat;
    [Range(0.5f, 5.0f)]
    public float width = 1;
    public float speed = 10;
    public float sharpness = 10;
    public float range = 10;
    Camera _cam;

    public float inputFromMicScript;

    private void Start()
    {
        _cam = GetComponent<Camera>();
    }

    float cd = 0.2f;

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
                CreateEchoEffect(mousePosAtClick);
            }
        }
        if(inputFromMicScript > 0.1f && cd <= 0.0f)
        {
            CreateEchoEffect(this.transform.position, inputFromMicScript);
            cd = 0.2f;
        }

        cd -= Time.deltaTime;
    }

    public void CreateEchoEffect(Vector4 originPos)
    {
        EchoImageEffect newEchoImageEffect = this.gameObject.AddComponent<EchoImageEffect>();
        newEchoImageEffect.EffectMaterial = effectMat;
        newEchoImageEffect.Origin = originPos;
        newEchoImageEffect.Width = width;
        newEchoImageEffect.LeadingEdgeSharpness = sharpness;
        newEchoImageEffect.MaxRange = range;
        newEchoImageEffect.Speed = speed;
    }

    public void CreateEchoEffect(Vector4 originPos, float inputLevel)
    {        
        originPos = new Vector4(originPos.x, originPos.y - 0.5f, originPos.z, originPos.w);
        EchoImageEffect newEchoImageEffect = this.gameObject.AddComponent<EchoImageEffect>();
        newEchoImageEffect.EffectMaterial = effectMat;
        newEchoImageEffect.Origin = originPos;
        newEchoImageEffect.Width = width;
        newEchoImageEffect.LeadingEdgeSharpness = sharpness;
        newEchoImageEffect.MaxRange = range * inputLevel;
        //Debug.Log(string.Format("{0} X {1} = {2}", range, inputLevel, range * inputLevel));
        newEchoImageEffect.Speed = speed;
    }
}
