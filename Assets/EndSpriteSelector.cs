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
            || WorldState.HasHappened(WorldEvent.EngineBlownUp))
            _renderer.sprite = PlayerDiesColonyDies;

        _renderer.sprite = PlayerDiesColonyLives;
    }
}
