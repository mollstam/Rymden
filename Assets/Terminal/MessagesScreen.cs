
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Assets.Terminal
{
    public class MessageScreen : ScreenBehahvior
    {
        private readonly Message _message;

        public MessageScreen(Message message)
        {
            _message = message;
        }

        public ScreenInfo CurrentInfo
        {
            get
            {
                var subjectUnderline = new string('-', _message.Subject.Count());

                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var time = epoch.AddSeconds(_message.Time);

                return new ScreenInfo(
                    _message.Subject + "\n" + subjectUnderline + "\n[" + string.Format("{0}-{1}-{2} {3:00}:{4:00}", time.Year, time.Month, time.Day, time.Hour, time.Minute) + "]\n\n" + _message.Text,
                    new List<ScreenAction> { new ScreenAction("Back", () =>
                    {
                        _message.MarkRead();
                        return null;
                    }) }
                );
            }
        }

        public bool ShowMessages
        {
            get { return false; }
        }

        public bool ShowMap
        {
            get { return false; }
        }
    }

    public class MessagesScreen : ScreenBehahvior
    {
        public ScreenInfo CurrentInfo
        {
            get
            {
                var allMessages = new List<Message>(WorldState.AllMessages());

                var allMessageActions = allMessages.Select(message =>
                    new ScreenAction(GetMessageSubjectDisplayName(message), () =>
                        new MessageScreen(message))).ToList();

                allMessageActions.Add(new ScreenAction("Refresh", () => this));
                allMessageActions.Add(new ScreenAction("Back", () => null));

                return new ScreenInfo(
                    "All messages\n" +
                    "------------",
                    allMessageActions
                );
            }
        }

        private static string GetMessageSubjectDisplayName(Message message)
        {
            var subject = message.Subject;

            if (!message.Read)
                subject += " [Unread]";

            return subject;
        }

        public bool ShowMessages
        {
            get { return false; }
        }

        public bool ShowMap
        {
            get { return false; }
        }
    }
}
