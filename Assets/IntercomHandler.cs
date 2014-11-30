using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public struct QueueItem
{
    public string message;
    public Action callback;
}

public class IntercomHandler : MonoBehaviour {

    public static bool IntroMode = true;

    private Queue<QueueItem> _queue = new Queue<QueueItem>();
    private string _currentMessage;
    private float _showMessageUntil;
    private string _labelContents; // Partial of message while printing
    private float _printingThrottle = 0.2f; // Pause between each segment
    private float _printNextSegmentAt;
    private float _minShowTime = 5.0f; // min no secs to show message
    private float _showTimePerCharacter = 0.3f; // seconds to add to show time for each character in message
    private int _maxWidth; // Calc on update
    private Action _currentCallback;

    public GUIStyle LabelStyle;
    public AudioClip Voice1;
    public AudioClip Voice2;
    public AudioClip Voice3;
    public AudioClip Voice4;

    private static IntercomHandler instance;
    public static IntercomHandler Instance
    {
        get {
            if (instance == null)
                instance = GameObject.Find("IntercomHandler").GetComponent<IntercomHandler>();
            return instance;
        }
    }

    public static void Broadcast(string message, Action callback = null)
    {
        Instance.Add(message, callback);
    }

    public static void Clear()
    {
        Instance.ClearAllMessages();
    }

    public void Update()
    {
        _maxWidth = UnityEngine.Screen.width - 240;

        if ((_currentMessage != "" && Time.time > _showMessageUntil) || (_currentMessage == "" && _queue.Count > 0))
        {
            ClearMessage();
            if (_currentCallback != null)
            {
                _currentCallback();
                _currentCallback = null;
            }
            
            if (_queue.Count > 0)
            {
                QueueItem next = _queue.Dequeue();
                _currentCallback = next.callback;
                ShowMessage(next.message);
            }
            else
            {
                ShowMessage("");
            }
        }

        if (_labelContents != _currentMessage && Time.time > _printNextSegmentAt)
        {
            ShowNextPart();
        }
    }

    public bool IsEmpty()
    {
        return _labelContents == "";
    }

    public void Add(string message, Action callback)
    {
        QueueItem qi;
        qi.message = message;
        qi.callback = callback;
        _queue.Enqueue(qi);
    }

    public void ClearAllMessages()
    {
        _queue.Clear();
        ClearMessage();
    }

    private void ClearMessage()
    {
        _currentMessage = "";
        _labelContents = "";
    }

    private void ShowMessage(string message)
    {
        _currentMessage = message;
        if (message != "")
        {
            _showMessageUntil = Time.time + Math.Max(_minShowTime, message.Length * _showTimePerCharacter);
        }
    }

    private void ShowNextPart()
    {
        if (_currentMessage == "")
        {
            _labelContents = "";
            return;
        }

        // Doing some sanity checks
        if (_labelContents.Length > _currentMessage.Length)
        {
            // label is longer than message, what?
            _labelContents = "";
        }
        if (_labelContents.Length > 0 && _labelContents.Length < _currentMessage.Length && _labelContents != _currentMessage.Substring(0, _labelContents.Length))
        {
            // label isn't the beginning of current message, strange!
            _labelContents = "";
        }

        string nextPart = _currentMessage.Substring(_labelContents.Length);
        CutToFirstSegment(ref nextPart);

        _labelContents += nextPart;
        OnPartAdded(nextPart);

        _printNextSegmentAt = Time.time + _printingThrottle;
    }

    private void OnPartAdded(string part)
    {
        // First vowel decides sample, that or random
        Regex r = new Regex(@"[aeiuo]");
        MatchCollection matches = r.Matches(part);
        AudioClip clip = new AudioClip[]{Voice1, Voice2, Voice3, Voice4}[(int)UnityEngine.Random.value * 4];
        if (matches.Count > 0)
        {
            string vowel = matches[0].Value;
            if (vowel == "e")
                clip = Voice1;
            else if (vowel == "a" || vowel == "i")
                clip = Voice2;
            else if (vowel == "u")
                clip = Voice3;
            else if (vowel == "o")
                clip = Voice4;
            else
                clip = null;
        }

        if (part != " " && clip != null)
            audio.PlayOneShot(clip, 1.0f);
    }

    private void CutToFirstSegment(ref string s)
    {
        // Whitespace
        Regex r = new Regex(@"^\s+");
        MatchCollection matches = r.Matches(s);
        if (matches.Count > 0)
        {
            s = s.Substring(0, matches[0].Length);
            return;
        }

        // Any non-alphanumerics
        r = new Regex(@"^\W+");
        matches = r.Matches(s);
        if (matches.Count > 0)
        {
            s = s.Substring(0, matches[0].Length);
            return;
        }

        // Any digit
        r = new Regex(@"^\d");
        matches = r.Matches(s);
        if (matches.Count > 0)
        {
            s = s.Substring(0, matches[0].Length);
            return;
        }

        // Up to and including next vowel
        r = new Regex(@"^\w+?[aeiou]");
        matches = r.Matches(s);
        if (matches.Count > 0)
        {
            s = s.Substring(0, matches[0].Length);
            return;
        }

        // Fail safe, just take a bunch
        s = s.Substring(0, Math.Min(3, s.Length));
    }

    public void OnGUI()
    {
        String s = "";
        if (_labelContents.Length > 0)
        {
            String temp = (!IntroMode ? "[Intercom] " : "") + _labelContents;
            while (temp.Length > 0)
            {
                if (s != "")
                    s += (!IntroMode ? "\n           " : "");

                String chunk = "";
                int i = 0;
                while (i <= temp.Length && LabelStyle.CalcSize(new GUIContent(chunk)).x < _maxWidth)
                {
                    chunk = temp.Substring(0, i++);
                }
                // We've found where to cut, but mayber there's a nice space nearby
                if (temp.Length > chunk.Length && chunk.Length - chunk.LastIndexOf(" ") < 20)
                    chunk = temp.Substring(0, chunk.LastIndexOf(" ") + 1);

                s += chunk;
                temp = temp.Substring(chunk.Length);
            }
        }

        GUI.skin = RymdenGUI.Skin;

        GUI.Label(new Rect(60, 40, UnityEngine.Screen.width - 120, 100), s, LabelStyle);
    }

}
