using UnityEngine;
using System.Collections;

public class Tooltip {
    public string Text;

    public void Show()
    {
        TooltipHandler.Instance.Set(this);
    }

    public void Hide()
    {
        if (TooltipHandler.Instance.IsCurrent(this))
            TooltipHandler.Instance.Set(null);
    }
}

public class TooltipHandler : MonoBehaviour {

    private Tooltip _currentTooltip;
    private Rect _rect;

    private static TooltipHandler _instance;
    
    public static TooltipHandler Instance {
        get {
            if (_instance == null)
                _instance = GameObject.Find("TooltipHandler").GetComponent<TooltipHandler>();
            return _instance;
        }
    }

    public void Start()
    {
        _rect = new Rect(10, UnityEngine.Screen.height - 30, UnityEngine.Screen.width - 20, 30);
    }

    public void OnGUI()
    {
        if (_currentTooltip != null)
        {
            GUI.skin = RymdenGUI.Skin;
            GUI.Label(_rect, _currentTooltip.Text);
        }
    }

    public bool IsCurrent(Tooltip tooltip)
    {
        return _currentTooltip == tooltip;
    }

    public void Set(Tooltip tooltip)
    {
        _currentTooltip = tooltip;
    }

}
