#nullable enable

namespace DarkSail.Refs
{
	public abstract class SharedRef<T> : ReadOnlySharedRef<T>, IRef<T>
	{
		public new T Value
		{
			get => base.Value;
			set => valueRef.Value = value;
		}
	}
}
