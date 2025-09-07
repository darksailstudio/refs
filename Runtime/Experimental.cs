#nullable enable

using System;
using System.Threading;

namespace DarkSail.Refs.Experimental
{
	public static class RefExtensions
	{
		public static IDisposable Subscribe<T>(
			this IReadOnlyRef<T> valueRef,
			Action<T> onChanged
		) => Subscribe(valueRef, onChanged, CancellationToken.None);

		public static IDisposable Subscribe<T>(
			this IReadOnlyRef<T> valueRef,
			Action<T> onChanged,
			CancellationToken cancellationToken
		)
		{
			if (valueRef == null)
				throw new ArgumentNullException(nameof(valueRef));

			if (onChanged == null)
				throw new ArgumentNullException(nameof(onChanged));

			if (cancellationToken.IsCancellationRequested)
				return Subscription.Canceled;

			valueRef.Changed += onChanged;

			try
			{
				onChanged(valueRef.Value);
			}
			catch
			{
				valueRef.Changed -= onChanged;
				throw;
			}

			return new Subscription(() => valueRef.Changed -= onChanged, cancellationToken);
		}

		public static IDisposable Subscribe(this IReadOnlyEventRef eventRef, Action onInvoked) =>
			Subscribe(eventRef, onInvoked, CancellationToken.None);

		public static IDisposable Subscribe(
			this IReadOnlyEventRef eventRef,
			Action onInvoked,
			CancellationToken cancellationToken
		)
		{
			if (eventRef == null)
				throw new ArgumentNullException(nameof(eventRef));

			if (onInvoked == null)
				throw new ArgumentNullException(nameof(onInvoked));

			if (cancellationToken.IsCancellationRequested)
				return Subscription.Canceled;

			eventRef.Invoked += onInvoked;
			return new Subscription(() => eventRef.Invoked -= onInvoked, cancellationToken);
		}

		sealed class Subscription : IDisposable
		{
			Action? unsubscribe;
			CancellationTokenRegistration registration;

			public static readonly Subscription Canceled = new();

			Subscription() { }

			public Subscription(Action unsubscribe, CancellationToken token)
			{
				this.unsubscribe = unsubscribe;

				if (token.CanBeCanceled)
					registration = token.Register(dispose, this);
			}

			static readonly Action<object> dispose = static subscription =>
				((Subscription)subscription).Dispose();

			public void Dispose()
			{
				var unsubscribe = Interlocked.Exchange(ref this.unsubscribe, null);

				try
				{
					unsubscribe?.Invoke();
				}
				finally
				{
					registration.Dispose();
				}
			}
		}
	}
}
