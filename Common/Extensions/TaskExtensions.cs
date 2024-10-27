
namespace Common.Extensions
{
	public static class TaskExtensions
	{
		public static async Task<(T0 ,T1)> WhenAll<T0 ,T1>(Task<T0> task0 ,Task<T1> task1)
		{
			var data = new Data2Values<T0 ,T1>();

			await Task.WhenAll(
				task0.ContinueWith(result => data.Item0 = result.Result),
				task1.ContinueWith(result => data.Item1 = result.Result)
				);

			return (data.Item0 ,data.Item1);
		}

		private struct Data2Values<T0 ,T1>
		{
			public T0 Item0;
			public T1 Item1;
		}
		public static async Task<(T0 ,T1 ,T2)> WhenAll<T0 ,T1 ,T2>(Task<T0> task0 ,Task<T1> task1 ,Task<T2> task2)
		{
			var data = new Data2Values<T0 ,T1 ,T2>();

			await Task.WhenAll(
				task0.ContinueWith(result => data.Item0 = result.Result),
				task1.ContinueWith(result => data.Item1 = result.Result),
				task2.ContinueWith(result => data.Item2 = result.Result)
				);

			return (data.Item0 ,data.Item1 ,data.Item2);
		}

		private struct Data2Values<T0 ,T1 ,T2>
		{
			public T0 Item0;
			public T1 Item1;
			public T2 Item2;
		}
		public static async Task<(T0 ,T1 ,T2 ,T3)> WhenAll<T0 ,T1 ,T2 ,T3>(Task<T0> task0 ,Task<T1> task1 ,Task<T2> task2 ,Task<T3> task3)
		{
			var data = new Data2Values<T0 ,T1 ,T2 ,T3>();

			await Task.WhenAll(
				task0.ContinueWith(result => data.Item0 = result.Result),
				task1.ContinueWith(result => data.Item1 = result.Result),
				task2.ContinueWith(result => data.Item2 = result.Result),
				task3.ContinueWith(result => data.Item3 = result.Result)
				);

			return (data.Item0 ,data.Item1 ,data.Item2 ,data.Item3);
		}

		private struct Data2Values<T0 ,T1 ,T2 ,T3>
		{
			public T0 Item0;
			public T1 Item1;
			public T2 Item2;
			public T3 Item3;
		}
		public static async Task<(T0 ,T1 ,T2 ,T3 ,T4)> WhenAll<T0 ,T1 ,T2 ,T3 ,T4>(Task<T0> task0 ,Task<T1> task1 ,Task<T2> task2 ,Task<T3> task3 ,Task<T4> task4)
		{
			var data = new Data2Values<T0 ,T1 ,T2 ,T3 ,T4>();

			await Task.WhenAll(
				task0.ContinueWith(result => data.Item0 = result.Result),
				task1.ContinueWith(result => data.Item1 = result.Result),
				task2.ContinueWith(result => data.Item2 = result.Result),
				task3.ContinueWith(result => data.Item3 = result.Result),
				task4.ContinueWith(result => data.Item4 = result.Result)
				);

			return (data.Item0 ,data.Item1 ,data.Item2 ,data.Item3 ,data.Item4);
		}

		private struct Data2Values<T0 ,T1 ,T2 ,T3 ,T4>
		{
			public T0 Item0;
			public T1 Item1;
			public T2 Item2;
			public T3 Item3;
			public T4 Item4;
		}
		public static async Task<(T0 ,T1 ,T2 ,T3 ,T4 ,T5)> WhenAll<T0 ,T1 ,T2 ,T3 ,T4 ,T5>(Task<T0> task0 ,Task<T1> task1 ,Task<T2> task2 ,Task<T3> task3 ,Task<T4> task4 ,Task<T5> task5)
		{
			var data = new Data2Values<T0 ,T1 ,T2 ,T3 ,T4 ,T5>();

			await Task.WhenAll(
				task0.ContinueWith(result => data.Item0 = result.Result),
				task1.ContinueWith(result => data.Item1 = result.Result),
				task2.ContinueWith(result => data.Item2 = result.Result),
				task3.ContinueWith(result => data.Item3 = result.Result),
				task4.ContinueWith(result => data.Item4 = result.Result),
				task5.ContinueWith(result => data.Item5 = result.Result)
				);

			return (data.Item0 ,data.Item1 ,data.Item2 ,data.Item3 ,data.Item4 ,data.Item5);
		}

		private struct Data2Values<T0 ,T1 ,T2 ,T3 ,T4 ,T5>
		{
			public T0 Item0;
			public T1 Item1;
			public T2 Item2;
			public T3 Item3;
			public T4 Item4;
			public T5 Item5;
		}
		public static async Task<(T0 ,T1 ,T2 ,T3 ,T4 ,T5 ,T6)> WhenAll<T0 ,T1 ,T2 ,T3 ,T4 ,T5 ,T6>(Task<T0> task0 ,Task<T1> task1 ,Task<T2> task2 ,Task<T3> task3 ,Task<T4> task4 ,Task<T5> task5 ,Task<T6> task6)
		{
			var data = new Data2Values<T0 ,T1 ,T2 ,T3 ,T4 ,T5 ,T6>();

			await Task.WhenAll(
				task0.ContinueWith(result => data.Item0 = result.Result),
				task1.ContinueWith(result => data.Item1 = result.Result),
				task2.ContinueWith(result => data.Item2 = result.Result),
				task3.ContinueWith(result => data.Item3 = result.Result),
				task4.ContinueWith(result => data.Item4 = result.Result),
				task5.ContinueWith(result => data.Item5 = result.Result),
				task6.ContinueWith(result => data.Item6 = result.Result)
				);

			return (data.Item0 ,data.Item1 ,data.Item2 ,data.Item3 ,data.Item4 ,data.Item5 ,data.Item6);
		}

		private struct Data2Values<T0 ,T1 ,T2 ,T3 ,T4 ,T5 ,T6>
		{
			public T0 Item0;
			public T1 Item1;
			public T2 Item2;
			public T3 Item3;
			public T4 Item4;
			public T5 Item5;
			public T6 Item6;
		}
		public static async Task<(T0 ,T1 ,T2 ,T3 ,T4 ,T5 ,T6 ,T7)> WhenAll<T0 ,T1 ,T2 ,T3 ,T4 ,T5 ,T6 ,T7>(Task<T0> task0 ,Task<T1> task1 ,Task<T2> task2 ,Task<T3> task3 ,Task<T4> task4 ,Task<T5> task5 ,Task<T6> task6 ,Task<T7> task7)
		{
			var data = new Data2Values<T0 ,T1 ,T2 ,T3 ,T4 ,T5 ,T6 ,T7>();

			await Task.WhenAll(
				task0.ContinueWith(result => data.Item0 = result.Result),
				task1.ContinueWith(result => data.Item1 = result.Result),
				task2.ContinueWith(result => data.Item2 = result.Result),
				task3.ContinueWith(result => data.Item3 = result.Result),
				task4.ContinueWith(result => data.Item4 = result.Result),
				task5.ContinueWith(result => data.Item5 = result.Result),
				task6.ContinueWith(result => data.Item6 = result.Result),
				task7.ContinueWith(result => data.Item7 = result.Result)
				);

			return (data.Item0 ,data.Item1 ,data.Item2 ,data.Item3 ,data.Item4 ,data.Item5 ,data.Item6 ,data.Item7);
		}

		private struct Data2Values<T0 ,T1 ,T2 ,T3 ,T4 ,T5 ,T6 ,T7>
		{
			public T0 Item0;
			public T1 Item1;
			public T2 Item2;
			public T3 Item3;
			public T4 Item4;
			public T5 Item5;
			public T6 Item6;
			public T7 Item7;
		}
		public static async Task<(T0 ,T1 ,T2 ,T3 ,T4 ,T5 ,T6 ,T7 ,T8)> WhenAll<T0 ,T1 ,T2 ,T3 ,T4 ,T5 ,T6 ,T7 ,T8>(Task<T0> task0 ,Task<T1> task1 ,Task<T2> task2 ,Task<T3> task3 ,Task<T4> task4 ,Task<T5> task5 ,Task<T6> task6 ,Task<T7> task7 ,Task<T8> task8)
		{
			var data = new Data2Values<T0 ,T1 ,T2 ,T3 ,T4 ,T5 ,T6 ,T7 ,T8>();

			await Task.WhenAll(
				task0.ContinueWith(result => data.Item0 = result.Result),
				task1.ContinueWith(result => data.Item1 = result.Result),
				task2.ContinueWith(result => data.Item2 = result.Result),
				task3.ContinueWith(result => data.Item3 = result.Result),
				task4.ContinueWith(result => data.Item4 = result.Result),
				task5.ContinueWith(result => data.Item5 = result.Result),
				task6.ContinueWith(result => data.Item6 = result.Result),
				task7.ContinueWith(result => data.Item7 = result.Result),
				task8.ContinueWith(result => data.Item8 = result.Result)
				);

			return (data.Item0 ,data.Item1 ,data.Item2 ,data.Item3 ,data.Item4 ,data.Item5 ,data.Item6 ,data.Item7 ,data.Item8);
		}

		private struct Data2Values<T0 ,T1 ,T2 ,T3 ,T4 ,T5 ,T6 ,T7 ,T8>
		{
			public T0 Item0;
			public T1 Item1;
			public T2 Item2;
			public T3 Item3;
			public T4 Item4;
			public T5 Item5;
			public T6 Item6;
			public T7 Item7;
			public T8 Item8;
		}
		public static async Task<(T0 ,T1 ,T2 ,T3 ,T4 ,T5 ,T6 ,T7 ,T8 ,T9)> WhenAll<T0 ,T1 ,T2 ,T3 ,T4 ,T5 ,T6 ,T7 ,T8 ,T9>(Task<T0> task0 ,Task<T1> task1 ,Task<T2> task2 ,Task<T3> task3 ,Task<T4> task4 ,Task<T5> task5 ,Task<T6> task6 ,Task<T7> task7 ,Task<T8> task8 ,Task<T9> task9)
		{
			var data = new Data2Values<T0 ,T1 ,T2 ,T3 ,T4 ,T5 ,T6 ,T7 ,T8 ,T9>();

			await Task.WhenAll(
				task0.ContinueWith(result => data.Item0 = result.Result),
				task1.ContinueWith(result => data.Item1 = result.Result),
				task2.ContinueWith(result => data.Item2 = result.Result),
				task3.ContinueWith(result => data.Item3 = result.Result),
				task4.ContinueWith(result => data.Item4 = result.Result),
				task5.ContinueWith(result => data.Item5 = result.Result),
				task6.ContinueWith(result => data.Item6 = result.Result),
				task7.ContinueWith(result => data.Item7 = result.Result),
				task8.ContinueWith(result => data.Item8 = result.Result),
				task9.ContinueWith(result => data.Item9 = result.Result)
				);

			return (data.Item0 ,data.Item1 ,data.Item2 ,data.Item3 ,data.Item4 ,data.Item5 ,data.Item6 ,data.Item7 ,data.Item8 ,data.Item9);
		}

		private struct Data2Values<T0 ,T1 ,T2 ,T3 ,T4 ,T5 ,T6 ,T7 ,T8 ,T9>
		{
			public T0 Item0;
			public T1 Item1;
			public T2 Item2;
			public T3 Item3;
			public T4 Item4;
			public T5 Item5;
			public T6 Item6;
			public T7 Item7;
			public T8 Item8;
			public T9 Item9;
		}
	}
}