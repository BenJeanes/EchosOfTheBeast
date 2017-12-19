using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingObject : MonoBehaviour {

    public Color GlowColor;
    public float LerpFactor = 10;

    public Color CurrentColor
    {
        get { return _currentColor; }
    }

    public Renderer[] Renderers
    {
        get;
        private set;
    }

    private List<Material> _materials = new List<Material>();
    private Color _currentColor;
    private Color _targetColor;

    // Use this for initialization
    void Start () {
        Renderers = GetComponentsInChildren<Renderer>();

        foreach (var renderer in Renderers)
        {
            _materials.AddRange(renderer.materials);
        }
    }

    private void OnMouseEnter()
    {
        _targetColor = GlowColor;
        enabled = true;
    }

    private void OnMouseExit()
    {
        _targetColor = Color.black;
        enabled = true;
    }

    // Update is called once per frame
    void Update () {
        _currentColor = Color.Lerp(_currentColor, _targetColor, Time.deltaTime * LerpFactor);

        for (int i = 0; i < _materials.Count; i++)
        {
            _materials[i].SetColor("_GlowColor", _currentColor);
        }

        if (_currentColor.Equals(_targetColor))
        {
            enabled = false;
        }
    }

    public void DoGlow()
    {
        _targetColor = GlowColor;
    }
    public void EndGlow()
    {
        _targetColor = Color.black;
    }
}
