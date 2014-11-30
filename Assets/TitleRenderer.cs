using UnityEngine;
using System.Collections;

public class TitleRenderer : MonoBehaviour {

    public GUIStyle TitleStyle;
    public GUIStyle MenuStyle;

    public void Update()
    {
        TitleStyle.fontSize = (int)(50.0f * (UnityEngine.Screen.width / 800.0f));
        MenuStyle.fontSize = (int)(20.0f * (UnityEngine.Screen.width / 800.0f));
    }

	public void OnGUI()
    {
        GUILayout.BeginArea(new Rect(UnityEngine.Screen.width * 0.03f, UnityEngine.Screen.height * 0.3f, UnityEngine.Screen.width, UnityEngine.Screen.height * 0.7f));
        GUILayout.Label("Shipment-23", TitleStyle);
        GUILayout.Label("\n\n\n1) Start Game\n\n2) Quit", MenuStyle);
        int prevSize = MenuStyle.fontSize;
        MenuStyle.fontSize = (int)(MenuStyle.fontSize * 0.4f);
        GUILayout.Label("\n\n\n\n\n\n\n\n\n\nMade during Games Against Ebola game jam\n\nby Karl (@KarlZylinski), Pontus (@pint38), and Tobias (@mollstam).\n\nMusic and sound by another Karl (@sumprunk). Eat your greens.", MenuStyle);
        MenuStyle.fontSize = prevSize;
        GUILayout.EndArea();
    }

}
