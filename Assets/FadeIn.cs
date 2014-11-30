using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour 
{
    private SpriteRenderer _renderer;

    public void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        var currentColor = _renderer.color;
        currentColor.a -= Time.deltaTime*0.05f;
        _renderer.color = currentColor;
    }
}
