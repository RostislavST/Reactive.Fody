﻿using System;
using System.Linq.Expressions;
using System.Reactive.Concurrency;
using System.Reflection;

namespace ReactiveUI.Fody.Helpers
{
    public static class ObservableAsPropertyExtensions
    {
        public static ObservableAsPropertyHelper<TRet> ToPropertyEx<TObj, TRet>(this IObservable<TRet> @this, TObj source, Expression<Func<TObj, TRet>> property, TRet initialValue = default(TRet), IScheduler scheduler = null) where TObj : ReactiveObject
        {
            var result = @this.ToProperty(source, property, initialValue, scheduler);

            // Now assign the field via reflection.
            var propertyInfo = property.GetPropertyInfo();
            if (propertyInfo == null)
                throw new Exception("Could not resolve expression " + property + " into a property.");
            var field = propertyInfo.DeclaringType.GetField("$" + propertyInfo.Name, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null)
                throw new Exception("Backing field not found for " + propertyInfo);
            field.SetValue(source, result);

            return result;
        }

        static PropertyInfo GetPropertyInfo(this LambdaExpression expression)
        {
            var current = expression.Body;
            var unary = current as UnaryExpression;
            if (unary != null)
                current = unary.Operand;
            var call = (MemberExpression)current;
            return (PropertyInfo)call.Member;
        }
    }
}