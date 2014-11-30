using System;
using UnityEngine;
using System.Collections.Generic;

public struct TerminalRenderData {
    public int width;
    public int height;
    public RenderTexture renderTexture;
}

public class TerminalRenderer : MonoBehaviour {

    private RenderTexture _prevActiveTexture = null;
    private Terminal _terminal;
    private TerminalRenderData _renderData;

    public GUIStyle TerminalStyle;

    public void Start()
    {
        _terminal = GetComponent<Terminal>();

        _renderData.width = 1600;
        _renderData.height = 900;
        _renderData.renderTexture = new RenderTexture(_renderData.width, _renderData.height, 24);
        _renderData.renderTexture.filterMode = FilterMode.Point;

        ClearTexture();

        renderer.material.SetTexture("_MainTex", _renderData.renderTexture);
    }

    public void Update()
    {
        //float fontSize = 20.0f * (UnityEngine.Screen.height / 400.0f);
        //TerminalStyle.fontSize = (int)fontSize;
    }

    public void OnGUI()
    {
        if (_terminal != null)
        {
            BeginRenderTextureGUI(_renderData.renderTexture);

            GUI.skin = RymdenGUI.TerminalSkin;
            int padding = 10;
            GUILayout.BeginArea(new Rect(padding, padding, _renderData.width - padding * 2, _renderData.height - padding * 2));
            GUILayout.Label(_terminal.Buffer, TerminalStyle);
            GUILayout.EndArea();

            EndRenderTextureGUI();
        }
    }

    public void ClearTexture()
    {
        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = _renderData.renderTexture;
        GL.Clear(false, true, new Color (0.0f, 0.0f, 0.0f, 0.0f));
        RenderTexture.active = prev;
    }

    protected void BeginRenderTextureGUI(RenderTexture targetTexture)
    {
        if (Event.current.type == EventType.Repaint)
        {
            if (targetTexture != null)
            {
                RenderTexture.active = targetTexture;
                ClearTexture();
            }
        }
    }
     
     
    protected void EndRenderTextureGUI()
    {
        if (Event.current.type == EventType.Repaint)
        {
            //_renderData.texture2D.ReadPixels(new Rect(0, 0, _renderData.width, _renderData.height), 0, 0);
            //_renderData.texture2D.Apply();

            RenderTexture.active = _prevActiveTexture;
        }
    }

}
