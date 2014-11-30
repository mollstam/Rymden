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
        currentColor.a += Time.deltaTime * 0.25f;
        _overlay.color = currentColor;

        if (_overlay.color.a > 1.0f - float.Epsilon)
        {
            Application.LoadLevel(1);
        }
    }
}
