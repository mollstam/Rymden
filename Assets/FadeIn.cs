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

        if (WorldState.HasHappened(WorldEvent.EngineOverheating))
        {
            currentColor.r = 1.0f;
            currentColor.g = 1.0f;
            currentColor.b = 1.0f;
        }

        if (Time.timeSinceLevelLoad > 3.0f)
            currentColor.a -= Time.deltaTime * 0.1f;

        _renderer.color = currentColor;
    }
}
