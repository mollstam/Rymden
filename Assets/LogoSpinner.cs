using UnityEngine;
using System.Collections;
using System;

public class LogoSpinner : MonoBehaviour {

    private float _changeLevelAt = 0;
    private SpriteRenderer _spriteRenderer;

	// Use this for initialization
	void Start () {
	   _spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 rot = transform.eulerAngles;
        float delta = Math.Max(0, 5000 - (Time.timeSinceLevelLoad * 1000));
        rot.z += delta * Time.deltaTime;
        transform.eulerAngles = rot;

        if (delta == 0 && _changeLevelAt == 0)
            _changeLevelAt = Time.time + 4;

        if (_changeLevelAt != 0 && Time.time > _changeLevelAt)
        {
            Application.LoadLevel("Menu");
        }
        if (_changeLevelAt != 0 && Time.time > (_changeLevelAt - 2))
        {
            float factor = Time.time - (_changeLevelAt - 2);
            var color = _spriteRenderer.color;
            color.a = 1 - factor * 2.0f;
            _spriteRenderer.color = color;
        }
	}
}
