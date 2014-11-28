﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public interface ScreenBehahvior
{
    string Text { get; }
    List<Action> Options { get; }
}

public class Screen
{
    private ScreenBehahvior _behavior;

    public Screen(ScreenBehahvior behavior)
    {
        _behavior = behavior;
    }

    public string Text
    {
        get { return _behavior.Text; }
    }

    public void OptionSelected(int option)
    {
        var index = option - 1;

        if (index < 0 || index >= _behavior.Options.Count)
            return;

        _behavior.Options[index]();
    }
}

class TestScreenBehavior : ScreenBehahvior
{
    public TestScreenBehavior()
    {
        Text = "This is an awesome test,\nplease choose something:\n\n1. Do a backflip\n2. Blow up\n\n> ";
        Options = new List<Action>
        {
            () => Debug.Log("Doing a backflip"),
            () => Debug.Log("Blew up, a lot"),
        };
    }

    public string Text { get; private set; }
    public List<Action> Options { get; private set; }
}

public class Terminal : MonoBehaviour
{
    public float CharacterInterval = 0.01f;
    public Screen StartScreen;
    private string _currentBuffer;
    private TextMesh _textMesh;
    private float _addNextCharAt;
    private Stack<Screen> _screens;
    private bool _acceptingInput;
    private string _input;

    public void Start()
    {
        StartScreen = new Screen(new TestScreenBehavior());

        if (StartScreen == null)
            throw new Exception("StartScreen is null");

        _acceptingInput = false;
        _currentBuffer = "";
        _screens = new Stack<Screen>();
        AddScreen(StartScreen);
        _addNextCharAt = 0.0f;
    }

    public void Update()
    {
        if (_currentBuffer == ScreenText())
            _acceptingInput = true;

        if (_acceptingInput)
        {
            AcceptInput();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                int enteredNumber;

                if (int.TryParse(_input, out enteredNumber))
                    _screens.Peek().OptionSelected(enteredNumber);

                _input = "";
            }

            if (Input.GetKeyDown(KeyCode.Backspace) && _input.Any())
                _input = _input.Substring(0, _input.Count() - 1);
        }
        
        AppendText();
        _textMesh = gameObject.transform.FindChild("Text").GetComponent<TextMesh>();
        _textMesh.text = _currentBuffer;
    }

    private void AcceptInput()
    {
        int enteredNumber;

        if (int.TryParse(Input.inputString, out enteredNumber))
            _input += enteredNumber.ToString(CultureInfo.InvariantCulture);
    }

    private void AppendText()
    {
        if (Time.time <= _addNextCharAt)
            return;

        if (CompleteBuffer() == _currentBuffer)
            return;

        var bufferSizeDiff = CompleteBuffer().Count() - _currentBuffer.Count();

        if (bufferSizeDiff > 0)
            _currentBuffer = _currentBuffer + CompleteBuffer().Substring(_currentBuffer.Length, 1);
        else if (bufferSizeDiff < 0)
            _currentBuffer = _currentBuffer.Substring(0, _currentBuffer.Count() - 1);

        _addNextCharAt = Time.time + CharacterInterval;
    }

    private void AddScreen(Screen screen)
    {
        _screens.Push(screen);
    }

    private string ScreenText()
    {
        return _screens.Peek().Text;
    }

    private string CompleteBuffer()
    {
        return ScreenText() + _input;
    }
}