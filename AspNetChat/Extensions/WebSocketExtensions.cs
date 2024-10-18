using System.Net.WebSockets;
using System.Text;

namespace AspNetChat.Extensions
{
    public static class WebSocketExtensions
    {
		private const int BufferSize = 4096;

		public static async Task<string> WaitMessageAsync(this WebSocket webSocket, CancellationToken token)
		{
			using var stream = new MemoryStream();

			var receiveBuffer = new byte[BufferSize];
			var segment = new ArraySegment<byte>(receiveBuffer);
			var result = default(WebSocketReceiveResult);

			do
			{
				result = await webSocket.ReceiveAsync(segment, token);

				if (result.MessageType == WebSocketMessageType.Close)
					throw new OperationCanceledException();

				stream.Write(segment.Array!, segment.Offset, result.Count);
			}
			while (!result.EndOfMessage);

			return Encoding.UTF8.GetString(stream.ToArray());
		}

		public static async Task SendMessageAsync(this WebSocket webSocket, string message, CancellationToken token)
		{
			var sendBuffer = new byte[BufferSize];
			var messageLength = message.Length;
			var messageCount = (int)Math.Ceiling((double)messageLength / BufferSize);

			for (var i = 0; i < messageCount; i++)
			{
				var offset = BufferSize * i;
				var count = BufferSize;
				var lastMessage = i + 1 == messageCount;

				if (count * (i + 1) > messageLength)
					count = messageLength - offset;

				var segmentLength = Encoding.UTF8.GetBytes(message, offset, count, sendBuffer, 0);
				var segment = new ArraySegment<byte>(sendBuffer, 0, segmentLength);
				await webSocket.SendAsync(segment, WebSocketMessageType.Text, lastMessage, token);
			}
		}
	}
}
