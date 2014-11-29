using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Assets.Terminal;
using UnityEngine;

public class ScreenAction
{
    public ScreenAction(string label, Func<ScreenBehahvior> action)
    {
        if (label == null)
            throw new NullReferenceException("label");

        Label = label;
        Action = action;
    }

    public string Label { get; private set; }
    public Func<ScreenBehahvior> Action { get; private set; }
}

public class ScreenInfo
{
    public ScreenInfo(string text, List<ScreenAction> options)
    {
        if (text == null)
            throw new ArgumentNullException("text");

        if (options == null)
            throw new ArgumentNullException("options");

        Text = text;
        Options = options;
    }

    public string Text { get; private set; }
    public List<ScreenAction> Options { get; private set; }
}

public interface ScreenBehahvior
{
    ScreenInfo CurrentInfo { get; }
    bool ShowMessages { get; }
    bool ShowMap { get; }
}

public class Screen
{
    private ScreenBehahvior _behavior;
    private RoomType _roomType;

    public Screen(RoomType roomType, ScreenBehahvior behavior)
    {
        _roomType = roomType;
        _behavior = behavior;
    }
    
    public string Text
    {
        get
        {
            var text =  _behavior.CurrentInfo.Text;
            var options = AllOptions();

            if (options.Any())
            {
                text += "\n";

                for (var i = 0; i < options.Count(); ++i)
                    text += "\n" + (i + 1).ToString(CultureInfo.InvariantCulture) + ". " + options[i].Label;
            }

            text += "\n\n> ";
            return text;
        }
    }

    public Screen OptionSelected(int option)
    {
        var index = option - 1;

        var options = AllOptions();

        if (index < 0 || index >= options.Count)
            return this;

        var newBehavior = options[index].Action();

		if (newBehavior == null)
			return null;

        return newBehavior == _behavior
            ? this
            : new Screen(_roomType, newBehavior);
    }

    public bool Equals(Screen screen)
    {
        return screen._behavior == _behavior;
    }

    private List<ScreenAction> AllOptions()
    {
        var options = new List<ScreenAction>(_behavior.CurrentInfo.Options);

        if (_behavior.ShowMessages)
            options.Add(new ScreenAction("Messages" + (WorldState.AnyNewMessages() ? " [Unread]" : ""), () => new MessagesScreen()));

        if (_behavior.ShowMap)
            options.Add(new ScreenAction("Map", () => new MapScreen(_roomType)));

        return options;
    }
}

public class Terminal : MonoBehaviour
{
    private static readonly Dictionary<string, Screen> StartScreens = new Dictionary<string, Screen>
    {
        {"LivingQuartersTerminal", new Screen(RoomType.LivingQuarters, new LivingQuartersTerminal())},
        {"CommandBridgeTerminal", new Screen(RoomType.Bridge,new CommandBridgeTerminal())},
        {"GreenhouseTerminal", new Screen(RoomType.Greenhouse,new GreenhouseTerminal())},
        {"ScienceLabTerminal", new Screen(RoomType.ScienceLab,new ScienceLabTerminal())},
        {"EngineeringScreen", new Screen(RoomType.Engineering,new EngineeringScreen())}
    };

    public float CharacterInterval = 0.01f;
    public bool HasQuit { get; set; }
    private string _currentBuffer;
    private TextMesh _textMesh;
    private float _addNextCharAt;
    private Stack<Screen> _screens = new Stack<Screen>();
    private bool _acceptingInput;
    private string _input;
    
    public void Reset(string startScreeName)
    {
        Screen startScreen;

        if (!StartScreens.TryGetValue(startScreeName, out startScreen))
            throw new Exception("Couldn't find startscreen with name " + startScreeName);

        _acceptingInput = false;
        _currentBuffer = "";
        _input = "";
        _textMesh = gameObject.transform.FindChild("Text").GetComponent<TextMesh>();
        _textMesh.text = "";
        _screens = new Stack<Screen>();
        AddScreen(startScreen);
        _addNextCharAt = 0.0f;
        HasQuit = false;
    }

    public void Update()
    {
        if (!_screens.Any())
            return;

        if (_currentBuffer == ScreenText())
            _acceptingInput = true;

        if (_acceptingInput)
        {
            AcceptInput();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                int enteredNumber;

                if (int.TryParse(_input, out enteredNumber))
                {
                    var currentScreen = _screens.Peek();
                    var newScreen = currentScreen.OptionSelected(enteredNumber);

                    if (newScreen == null)
                    {
                        _screens.Pop();
                        _currentBuffer = "";
                        _acceptingInput = false;

                        if (!_screens.Any())
                        {
                            HasQuit = true;
                            return;
                        }
                    }
                    else if (!newScreen.Equals(currentScreen))
                        _screens.Push(newScreen);

                    _currentBuffer = "";
                    _acceptingInput = false;
                }

                _input = "";
            }

            if (Input.GetKeyDown(KeyCode.Backspace) && _input.Any())
                _input = _input.Substring(0, _input.Count() - 1);
        }
        
        AppendText();
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
