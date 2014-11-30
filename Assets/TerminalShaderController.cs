using UnityEngine;
using System.Collections;

public class TerminalShaderController : MonoBehaviour {

    public float HSyncAmount = 0;
    public float HSyncTime = 0.5f;

    private float _hsyncStartAt = 0;
    private float _hsyncCurrentValue;
    private AudioClip _sound;

    public void Start()
    {
        _sound = GetComponent<Terminal>().Hsync;
    }

    public void Update()
    {
        if (HSyncAmount > 0 && Random.value < HSyncAmount)
        {
            TriggerHsync();
        }

        // Hsync
        if (_hsyncStartAt != 0)
        {
            if (Time.time > _hsyncStartAt + HSyncTime)
            {
                _hsyncStartAt = 0;
                _hsyncCurrentValue = 0;
            }
            else
            {
                _hsyncCurrentValue = HSyncAmount - (Time.time - _hsyncStartAt)/HSyncTime;
            }

            renderer.material.SetFloat("_HsyncFactor", _hsyncCurrentValue);
        }

    }

    public void TriggerHsync()
    {
        _hsyncStartAt = Time.time;
        if (GetComponent<Terminal>().InUse)
            audio.PlayOneShot(_sound, 0.065f);
    }

}
