using System;

namespace Reactive.Fody.Helpers
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class ObservableAsPropertyAttribute : Attribute
    {
    }
}