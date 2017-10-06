using System;

namespace Reactive.Fody.Helpers
{
    class LogPropertyOnErrorObservable<T> : IObservable<T>
    {
        readonly IObservable<T> @this;
        readonly object source;
        readonly string property;

        public LogPropertyOnErrorObservable(IObservable<T> @this, object source, string property)
        {
            this.@this = @this;
            this.source = source;
            this.property = property;
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return @this.Subscribe(new LogPropertyOnErrorObserver(observer, source, property));
        }

        class LogPropertyOnErrorObserver : IObserver<T>
        {
            readonly IObserver<T> @this;
            readonly object source;
            readonly string property;

            public LogPropertyOnErrorObserver(IObserver<T> @this, object source, string property)
            {
                this.@this = @this;
                this.source = source;
                this.property = property;
            }

            public void OnCompleted()
            {
                @this.OnCompleted();
            }

            public void OnError(Exception error)
            {
                @this.OnError(new LogPropertyOnErrorException(source, property, error));
            }

            public void OnNext(T value)
            {
                @this.OnNext(value);
            }
        }
    }
}
