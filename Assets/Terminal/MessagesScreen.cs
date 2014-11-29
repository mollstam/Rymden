
using System.Collections.Generic;
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

                return new ScreenInfo(
                    _message.Subject + "\n" + subjectUnderline + "\n\n" + _message.Text,
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
                var allMessageActions = WorldState.AllMessages().Select(message =>
                    new ScreenAction(GetMessageSubjectDisplayName(message), () =>
                        new MessageScreen(message))).ToList();

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
