using UnityEngine;

public class DoorWay : MonoBehaviour
{
    private Tooltip _tooltip;
    public GameObject Target;
    public string TooltipText;

    public void Start()
    {
        _tooltip = new Tooltip();
        _tooltip.Text = TooltipText != "" ? TooltipText : "Door";
    }

    public void Update()
    {
        if (_tooltip != null)
        {
            Vector2 mouseWorldSpace = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (collider2D.OverlapPoint(mouseWorldSpace))
            {
                _tooltip.Show();
            }
            else
            {
                _tooltip.Hide();
            }
        }
    }
}
