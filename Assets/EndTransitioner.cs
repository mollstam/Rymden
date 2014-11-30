using UnityEngine;
using System.Collections;

public class EndTransitioner : MonoBehaviour
{
    private SpriteRenderer _overlay;

    public void Start()
    {
        _overlay = transform.FindChild("Overlay").GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        if (!WorldState.HasEndState())
            return;

        var currentColor = _overlay.color;

        var speed = 0.25f;

        if (WorldState.HasHappened(WorldEvent.EngineOverheating))
        {
            currentColor.r = 1.0f;
            currentColor.g = 1.0f;
            currentColor.b = 1.0f;
            speed = 0.7f;
        }

        currentColor.a += Time.deltaTime * speed;
        _overlay.color = currentColor;

        if (_overlay.color.a > 1.0f - float.Epsilon)
        {
            Application.LoadLevel(2);
        }
    }
}
