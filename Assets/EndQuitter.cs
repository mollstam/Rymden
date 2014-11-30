using UnityEngine;
using System.Collections;

public class EndQuitter : MonoBehaviour {

    private float _fadeStartAt = 0;
    private float _fadeEndAt = 0;
    private float _fadeTime = 4;
    private SpriteRenderer _renderer;

	// Use this for initialization
	void Start () {
        _fadeStartAt = Time.time + 25;
        _renderer = GameObject.Find("TestBackground").GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > _fadeStartAt)
        {
            if (_fadeEndAt == 0)
                _fadeEndAt = Time.time + _fadeTime;
        
            var currentColor = _renderer.color;
            currentColor.r = 0;
            currentColor.g = 0;
            currentColor.b = 0;
            
            float f = (_fadeEndAt - Time.time) / _fadeTime;
            currentColor.a = 1 - f;
            _renderer.color = currentColor;
        }

        if (_fadeEndAt != 0 && Time.time > _fadeEndAt + 1.0f)
        {
            Application.Quit();
        }
	}
}
