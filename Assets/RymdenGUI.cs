using UnityEngine;
using System.Collections;

public class RymdenGUI : MonoBehaviour {

    private static RymdenGUI Instance;
    public static GUISkin Skin
    {
        get {
            if (Instance == null)
                Instance = GameObject.Find("Main Camera").GetComponent<RymdenGUI>();
            return Instance.SkinInstance;
        }
    }
    public static GUISkin TerminalSkin
    {
        get {
            if (Instance == null)
                Instance = GameObject.Find("Main Camera").GetComponent<RymdenGUI>();
            return Instance.TerminalSkinInstance;
        }
    }

    public GUISkin SkinInstance;
    public GUISkin TerminalSkinInstance;

    public void OnGUI()
    {
        GUI.skin = SkinInstance;
    }

}
