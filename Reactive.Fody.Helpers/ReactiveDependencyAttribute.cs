using System;

namespace Reactive.Fody.Helpers
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ReactiveDependencyAttribute : Attribute
    {
        readonly string _targetName;

        public ReactiveDependencyAttribute(string targetName)
        {
            _targetName = targetName;
        }

        /// <summary>
        /// The name of the backing property
        /// </summary>
        public string Target => _targetName;

        /// <summary>
        /// Target property on the backing property
        /// </summary>
        public string TargetProperty { get; set; }
    }
}
