using AspNetChat.Core.Interfaces.ChatEvents;

namespace AspNetChat.Extensions.Comparers
{

    public class EventsComparer : IComparer<IEvent>
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
}
