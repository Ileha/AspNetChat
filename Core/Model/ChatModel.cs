using AspNetChat.Core.Interfaces;
using System;

namespace AspNetChat.Core.Model
{
    public class ChatModel : IChat
    {
        public Guid Id { get; }

        private readonly object _eventsLocker = new object();
        private readonly SortedSet<IEvent> _events = new SortedSet<IEvent>(new EventsComparer());

        public ChatModel(Guid guid) 
        {
            Id = guid;
        }

        public void DisconnectedParticipant(IChatPartisipant partisipant)
        {
            lock (_eventsLocker)
            {
                _events.Add(new UserDisconnected(partisipant.Id, GetTime()));
            }

            PostEvents();
        }

        public string GetChatMessageList()
        {
            lock (_eventsLocker) 
            {
                return "Hello world!!!";
            }
        }

        public void JoinParticipant(IChatPartisipant partisipant)
        {
            lock (_eventsLocker)
            {
                _events.Add(new UserConnected(partisipant.Name, partisipant.Id, GetTime()));
            }

            PostEvents();
        }

        public void SendMessage(IChatPartisipant partisipant, string message)
        {
            lock (_eventsLocker)
            {
                _events.Add(new UserSendMessage(partisipant.Id, message, GetTime()));
            }

            PostEvents();
        }

        private DateTime GetTime() 
        {
            return DateTime.UtcNow;
        }

        private Task PostEvents() 
        {
            return Task.Run(() => 
            {

                var message = GetChatMessageList();


            });
        }

        private interface IEventVisitor 
        {
            void Visit(UserConnected userConnected);
            void Visit(UserSendMessage userSendMessage);
            void Visit(UserDisconnected userDisconnected);
        }

        private interface IEvent 
        {
            DateTime DateTime { get; }
            void Accept(IEventVisitor eventVisitor);
        }

        private class EventsComparer : IComparer<IEvent>
        {
            private readonly IComparer<DateTime> _timeComparer = Comparer<DateTime>.Default;

            public int Compare(IEvent? x, IEvent? y)
            {
                if (x == null)
                    throw new ArgumentNullException(nameof(x));

                if (y == null)
                    throw new ArgumentNullException(nameof(y));

                return _timeComparer.Compare(x.DateTime, y.DateTime);
            }
        }

        private class UserConnected : IEvent
        {
            private readonly string _name;
            private readonly Guid _guid;

            public DateTime DateTime { get; }

            public UserConnected(string name, Guid guid, DateTime dateTime) 
            {
                _name = name ?? throw new ArgumentNullException(nameof(name));
                _guid = guid;
                DateTime = dateTime;
            }

            public void Accept(IEventVisitor eventVisitor)
            {
                if (eventVisitor == null)
                    throw new ArgumentNullException(nameof(eventVisitor));

                eventVisitor.Visit(this);
            }
        }

        public class UserSendMessage : IEvent
        {
            private readonly Guid _guid;
            private readonly string _message;

            public DateTime DateTime { get; }

            public UserSendMessage(Guid guid, string message, DateTime dateTime)
            {
                _guid = guid;
                _message = message ?? throw new ArgumentNullException(nameof(message));
                DateTime = dateTime;
            }

            void IEvent.Accept(IEventVisitor eventVisitor)
            {
                if (eventVisitor == null)
                    throw new ArgumentNullException(nameof(eventVisitor));

                eventVisitor.Visit(this);
            }
        }

        public class UserDisconnected : IEvent
        {
            private readonly Guid _guid;

            public DateTime DateTime { get; }

            public UserDisconnected(Guid guid, DateTime dateTime) 
            {
                _guid = guid;
                DateTime = dateTime;
            }

            void IEvent.Accept(IEventVisitor eventVisitor)
            {
                if (eventVisitor == null)
                    throw new ArgumentNullException(nameof(eventVisitor));

                eventVisitor.Visit(this);
            }
        }
    }
}
