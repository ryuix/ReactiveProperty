﻿using System;
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
#if WINDOWS_PHONE
    public delegate TResult Func<T1, T2, T3, T4, T5, TResult>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);
    public delegate TResult Func<T1, T2, T3, T4, T5, T6, TResult>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6);
    public delegate TResult Func<T1, T2, T3, T4, T5, T6, T7, TResult>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7);
#endif

    public static class CombineLatestObservableExtensions
    {
        public static IObservable<TResult> CombineLatest<T1, T2, T3, TResult>(
            this IObservable<T1> source1,
            IObservable<T2> source2,
            IObservable<T3> source3,
            Func<T1, T2, T3, TResult> selector)
        {
            return source1
                .CombineLatest(source2, (t1, t2) => new { t1, t2 })
                .CombineLatest(source3, (a, t3) => selector(
                    a.t1,
                    a.t2,
                    t3
                    ));
        }
        public static IObservable<TResult> CombineLatest<T1, T2, T3, T4, TResult>(
            this IObservable<T1> source1,
            IObservable<T2> source2,
            IObservable<T3> source3,
            IObservable<T4> source4,
            Func<T1, T2, T3, T4, TResult> selector)
        {
            return source1
                .CombineLatest(source2, (t1, t2) => new { t1, t2 })
                .CombineLatest(source3, (a, t3) => new { a, t3 })
                .CombineLatest(source4, (a, t4) => selector(
                    a.a.t1,
                    a.a.t2,
                    a.t3,
                    t4
                    ));
        }

        public static IObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, TResult>(
            this IObservable<T1> source1,
            IObservable<T2> source2,
            IObservable<T3> source3,
            IObservable<T4> source4,
            IObservable<T5> source5,
            Func<T1, T2, T3, T4, T5, TResult> selector)
        {
            return source1
                .CombineLatest(source2, (t1, t2) => new { t1, t2 })
                .CombineLatest(source3, (a, t3) => new { a, t3 })
                .CombineLatest(source4, (a, t4) => new { a, t4 })
                .CombineLatest(source5, (a, t5) => selector(
                    a.a.a.t1,
                    a.a.a.t2,
                    a.a.t3,
                    a.t4,
                    t5
                    ));
        }

        public static IObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, TResult>(
            this IObservable<T1> source1,
            IObservable<T2> source2,
            IObservable<T3> source3,
            IObservable<T4> source4,
            IObservable<T5> source5,
            IObservable<T6> source6,
            Func<T1, T2, T3, T4, T5, T6, TResult> selector)
        {
            return source1
                .CombineLatest(source2, (t1, t2) => new { t1, t2 })
                .CombineLatest(source3, (a, t3) => new { a, t3 })
                .CombineLatest(source4, (a, t4) => new { a, t4 })
                .CombineLatest(source5, (a, t5) => new { a, t5 })
                .CombineLatest(source6, (a, t6) => selector(
                    a.a.a.a.t1,
                    a.a.a.a.t2,
                    a.a.a.t3,
                    a.a.t4,
                    a.t5,
                    t6
                    ));
        }

        public static IObservable<TResult> CombineLatest<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this IObservable<T1> source1,
            IObservable<T2> source2,
            IObservable<T3> source3,
            IObservable<T4> source4,
            IObservable<T5> source5,
            IObservable<T6> source6,
            IObservable<T7> source7,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> selector)
        {
            return source1
                .CombineLatest(source2, (t1, t2) => new { t1, t2 })
                .CombineLatest(source3, (a, t3) => new { a, t3 })
                .CombineLatest(source4, (a, t4) => new { a, t4 })
                .CombineLatest(source5, (a, t5) => new { a, t5 })
                .CombineLatest(source6, (a, t6) => new { a, t6 })
                .CombineLatest(source7, (a, t7) => selector(
                    a.a.a.a.a.t1,
                    a.a.a.a.a.t2,
                    a.a.a.a.t3,
                    a.a.a.t4,
                    a.a.t5,
                    a.t6,
                    t7
                    ));
        }
    }
}