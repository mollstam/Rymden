using UnityEngine;
using System.Collections;

public class DeadBushesEnabler : MonoBehaviour
{
    public bool Enabled = true;

    private SpriteRenderer _renderer;

    public void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.enabled = !Enabled;
    }

    public void Update()
    {
        if (WorldState.HasHappened(WorldEvent.VentGreenHouseOutside))
            _renderer.enabled = Enabled;
    }
}
