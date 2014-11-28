using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class ScreenAction
{
    public ScreenAction(string label, Action action)
    {
        if (label == null)
            throw new NullReferenceException("label");

        Label = label;
        Action = action;
    }

    public string Label { get; private set; }
    public Action Action { get; private set; }
}

public interface ScreenBehahvior
{
    string Text { get; }
    List<ScreenAction> Options { get; }
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
        get
        {
            var text =  _behavior.Text;

            if (_behavior.Options.Any())
            {
                text += "\n";

                for (var i = 0; i < _behavior.Options.Count(); ++i)
                    text += "\n" + (i + 1).ToString(CultureInfo.InvariantCulture) + " " + _behavior.Options[i].Label;
            }

            text += "\n\n> ";
            return text;
        }
    }

    public void OptionSelected(int option)
    {
        var index = option - 1;

        if (index < 0 || index >= _behavior.Options.Count)
            return;

        _behavior.Options[index].Action();
    }
}

class TestScreenBehavior : ScreenBehahvior
{
    public TestScreenBehavior()
    {
        Text = "This is an awesome test, please\nchoose something:";
        Options = new List<ScreenAction>
        {
            new ScreenAction("Do a backflip", () => Debug.Log("Doing a backflip")),
            new ScreenAction("Blow up", () => Debug.Log("Blew up, a lot"))
        };
    }

    public string Text { get; private set; }
    public List<ScreenAction> Options { get; private set; }
}

public class Terminal : MonoBehaviour
{

    public float CharacterInterval = 0.01f;
    public string StartScreenName;
    private string _currentBuffer;
    private TextMesh _textMesh;
    private float _addNextCharAt;
    private Stack<Screen> _screens;
    private bool _acceptingInput;
    private string _input;

    public void Start()
    {
        if (StartScreenName == null)
            throw new Exception("StartScreenName is null");

        var startScreens = new Dictionary<string, Screen>
        {
            {"TestScreen", new Screen(new TestScreenBehavior())}
        };

        Screen startScreen;

        if (!startScreens.TryGetValue(StartScreenName, out startScreen))
            throw new Exception("Couldn't find startscreen with name " + StartScreenName);

        _acceptingInput = false;
        _currentBuffer = "";
        _screens = new Stack<Screen>();
        AddScreen(startScreen);
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
