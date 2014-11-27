using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LightStrober : MonoBehaviour
{
    public float Frequency;
    public float BaseStrobeTime;
    public float RandomPartMin;
    public float RandomPartMax;
    public float MinIntensity;
    private float _strobeStartTime;
    private float _strobeTime;
    private float _currentIntensity;
    private SpriteRenderer _spriteRenderer;

    public void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currentIntensity = 1;
        SetNextStartTime();
    }

    public void Update()
    {
        if (Time.time > _strobeStartTime + _strobeTime)
            SetNextStartTime();

        if (Time.time > _strobeStartTime)
        {
            var halfTime = _strobeTime/2;

            if (Time.time > _strobeStartTime + halfTime)
            {
                _currentIntensity = Math.Abs((Time.time + halfTime) - (_strobeStartTime + _strobeTime)) / halfTime;
            }
            else
            {
                _currentIntensity = Math.Abs(Time.time - (_strobeStartTime + halfTime)) / halfTime;
            }
        }

        _currentIntensity = Math.Max(_currentIntensity, MinIntensity);
        Debug.Log(_currentIntensity);
        var currentColor = _spriteRenderer.color;
        currentColor.a = _currentIntensity;
        _spriteRenderer.color = currentColor;
    }

    private void SetNextStartTime()
    {
        _strobeStartTime = Time.time + Frequency + Random.Range(RandomPartMin, RandomPartMax);
        _strobeTime = BaseStrobeTime + Random.Range(RandomPartMin, RandomPartMax);
    }
}
