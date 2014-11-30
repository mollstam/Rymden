using UnityEngine;

public class EndSpriteSelector : MonoBehaviour
{
    public Sprite PlayerDiesColonyDies;
    public Sprite PlayerDiesColonyLives;
    private SpriteRenderer _renderer;

    public void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();

        if (WorldState.HasHappened(WorldEvent.VentGreenHouseInside)
            || WorldState.HasHappened(WorldEvent.EngineBlownUp)
            || WorldState.HasHappened(WorldEvent.PlottedForEarth))
        {
            _renderer.sprite = PlayerDiesColonyDies;
            return;
        }

        _renderer.sprite = PlayerDiesColonyLives;
    }
}
