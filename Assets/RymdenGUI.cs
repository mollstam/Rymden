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

    public GUISkin SkinInstance;

    public void OnGUI()
    {
        GUI.skin = SkinInstance;
    }

}
