using UnityEngine;

public class AlphaToggler : MonoBehaviour
{
    public float LowAlpha = 0.7f;
    public float HighAlpha = 1;
    public float BaseInterval = 0.5f;
    public float IntervalRandomMin = 0.1f;
    public float IntervalRandomMax = 0.5f;
    public float BaseLowTime = 0.1f;
    public float LowTimeRandomMin = 0.05f;
    public float LowTimeRandomMax = 0.1f;
    private float _nextToggleAt;
    private float _lowTime;
    private SpriteRenderer _spriteRenderer;

    public void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetNextToggle();
    }

    public void Update()
    {
        if (Time.time > _nextToggleAt)
        {
            SetAlpha(LowAlpha);

            if (Time.time > _nextToggleAt + _lowTime)
            {
                SetAlpha(HighAlpha);
                SetNextToggle();
            }
        }
    }

    private void SetAlpha(float alpha)
    {
        var currentColor = _spriteRenderer.color;
        currentColor.a = alpha;
        _spriteRenderer.color = currentColor;
    }

    private void SetNextToggle()
    {
        _nextToggleAt = Time.time + BaseInterval + Random.Range(IntervalRandomMin, IntervalRandomMax);
        _lowTime = BaseLowTime + Random.Range(LowTimeRandomMin, LowTimeRandomMax);
    }
}
