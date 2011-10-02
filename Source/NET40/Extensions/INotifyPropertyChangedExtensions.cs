﻿using System;
using System.ComponentModel;
using System.Linq.Expressions;
#if WINDOWS_PHONE
using Microsoft.Phone.Reactive;
#else
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
#endif

namespace Codeplex.Reactive.Extensions
{
    public static class INotifyPropertyChangedExtensions
    {
        public static IObservable<PropertyChangedEventArgs> PropertyChangedAsObservable<T>(this T subject)
            where T : INotifyPropertyChanged
        {
            return ObservableEx.FromEvent<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                h => (sender, e) => h(e),
                h => subject.PropertyChanged += h,
                h => subject.PropertyChanged -= h);
        }

        public static IObservable<TProperty> ObserveProperty<TSubject, TProperty>(
            this TSubject subject, Expression<Func<TSubject, TProperty>> propertySelector)
            where TSubject : INotifyPropertyChanged
        {
            string propertyName;
            var accessor = AccessorCache<TSubject>.Lookup(propertySelector, out propertyName);

            return subject.PropertyChangedAsObservable()
                .Where(e => e.PropertyName == propertyName)
                .Select(_ => ((Func<TSubject, TProperty>)accessor).Invoke(subject));
        }
    }
}