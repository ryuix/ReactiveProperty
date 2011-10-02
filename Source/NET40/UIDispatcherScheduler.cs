﻿using System;
using System.Windows.Threading;
using System.Windows;
#if WINDOWS_PHONE
using Microsoft.Phone.Reactive;
#else
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Disposables;
using System.Reactive.Concurrency;
#endif

namespace Codeplex.Reactive
{
    public class UIDispatcherScheduler : IScheduler
    {
        // static

        static readonly UIDispatcherScheduler defaultScheduler = new UIDispatcherScheduler(
#if SILVERLIGHT
Deployment.Current.Dispatcher
#else
            Dispatcher.CurrentDispatcher
#endif
);

        public static UIDispatcherScheduler Default
        {
            get { return defaultScheduler; }
        }

        public static void Initialize()
        {
            var _ = defaultScheduler; // initialize
        }

        // instance

        UIDispatcherScheduler(Dispatcher dispatcher)
        {
            this.Dispatcher = dispatcher;
        }

        public Dispatcher Dispatcher { get; private set; }

        public DateTimeOffset Now
        {
            get { return DateTimeOffset.Now; }
        }

#if !WINDOWS_PHONE

        public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null) throw new ArgumentNullException("action");

            return Schedule(state, dueTime - Now, action);
        }

        public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null) throw new ArgumentNullException("action");

            var interval = Scheduler.Normalize(dueTime);
            if (interval.Ticks == 0) return Schedule(state, action); // schedule immediately

            var cancelable = new MultipleAssignmentDisposable();
            DispatcherTimer timer = new DispatcherTimer(
#if !SILVERLIGHT
                DispatcherPriority.Background, Dispatcher
#endif
                )
            {
                Interval = interval
            };
            timer.Tick += (sender, e) =>
            {
                var _timer = timer;
                if (_timer != null) _timer.Stop();
                timer = null;
                cancelable.Disposable = action(this, state);
            };

            timer.Start();

            cancelable.Disposable = Disposable.Create(() =>
            {
                var _timer = timer;
                if (_timer != null) _timer.Stop();
                timer = null;
            });

            return cancelable;
        }

        public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
        {
            if (action == null) throw new ArgumentNullException("action");

            if (Dispatcher.CheckAccess())
            {
                return action(this, state);
            }
            else
            {
                var cancelable = new SingleAssignmentDisposable();

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (!cancelable.IsDisposed)
                    {
                        cancelable.Disposable = action(this, state);
                    }
                }));

                return cancelable;
            }
        }

#else

        public IDisposable Schedule(Action action, TimeSpan dueTime)
        {
            if (action == null) throw new ArgumentNullException("action");

            return Scheduler.Dispatcher.Schedule(action, dueTime);
        }

        public IDisposable Schedule(Action action)
        {
            if (action == null) throw new ArgumentNullException("action");

            if (Dispatcher.CheckAccess())
            {
                action();
                return Disposable.Empty;
            }
            else
            {
                var cancelable = new BooleanDisposable();

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (!cancelable.IsDisposed)
                    {
                        action();
                    }
                }));

                return cancelable;
            }
        }

#endif
    }
}