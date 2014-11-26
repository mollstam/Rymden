using UnityEngine;

public class ComputerScreen : MonoBehaviour
{
    public RenderTexture ComputerTexture;
    private Texture2D _computerTexture2D;
    public Material ComputerMaterial;
    private RenderTexture _prevActive;

    public void Start()
    {
        _computerTexture2D = new Texture2D(800, 600);
     
        if (ComputerMaterial != null)
        {
            ComputerMaterial.mainTexture = _computerTexture2D;
        }
    }

    public void OnGUI()
    {
        if (Event.current.type == EventType.Repaint)
        {
            _prevActive = RenderTexture.active;
            
            if (ComputerTexture != null)
            {
                RenderTexture.active = ComputerTexture;
                GL.Clear(false, true, Color.cyan);
            }
        }

        GUI.TextField(new Rect(0, 0, 800, 600), "Yes hello this is dog");

        if (Event.current.type == EventType.Repaint)
        {
            if (ComputerTexture != null)
            {
                _computerTexture2D.ReadPixels(new Rect(0, 0, ComputerTexture.width, ComputerTexture.height), 0, 0);
                _computerTexture2D.Apply();
            }
    
            RenderTexture.active = _prevActive;
        }
    }
}
