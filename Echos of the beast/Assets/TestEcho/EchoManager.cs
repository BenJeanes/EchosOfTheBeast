using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoManager : MonoBehaviour
{
    public Material effectMat;
    Camera _cam;

    private void Start()
    {
        _cam = GetComponent<Camera>();
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
                EchoEffectScript eff = this.gameObject.AddComponent<EchoEffectScript>();                
                eff.origin = mousePosAtClick;
                eff.effectMat = effectMat;
            }             
            
        }
    }
}
