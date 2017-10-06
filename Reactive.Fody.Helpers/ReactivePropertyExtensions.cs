﻿using System;
using Reactive.Fody.Helpers.Settings;
using ReactiveUI;

namespace Reactive.Fody.Helpers
{
    public static class ReactivePropertyExtensions
    {
        public static TRet RaiseAndSetIfChanged<TObj, TRet>(this TObj @this, ref TRet backingField, TRet newValue, string propertyName = null) where TObj : IReactiveObject
        {
            if (GlobalSettings.IsLogPropertyOnErrorEnabled)
            {
                try
                {
                    return IReactiveObjectExtensions.RaiseAndSetIfChanged(@this, ref backingField, newValue, propertyName);
                }
                catch (Exception ex)
                {
                    throw new LogPropertyOnErrorException(@this, propertyName, ex);
                }
            }
            else
            {
                return IReactiveObjectExtensions.RaiseAndSetIfChanged(@this, ref backingField, newValue, propertyName);
            }
        }
    }
}