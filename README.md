[Japanese](http://blog.okazuki.jp/entry/2015/12/05/221154)

# ReactiveProperty

ReactiveProperty provides MVVM and asynchronous support features under Reactive Extensions. Target framework is .NET Standard 2.0.

![](https://img.shields.io/nuget/v/ReactiveProperty.svg)
![](https://img.shields.io/nuget/dt/ReactiveProperty.svg)
![Build and Release](https://github.com/runceel/ReactiveProperty/workflows/Build%20and%20Release/badge.svg)

![ReactiveProperty overview](Images/rpsummary.png)

ReactiveProperty is a very powerful and simple library.

![Delay and Select](Images/launch-uwp-app.gif)

This sample app's ViewModel code is as below:

```cs
public class MainPageViewModel
{
    public ReactiveProperty<string> Input { get; }
    public ReadOnlyReactiveProperty<string> Output { get; }
    public MainPageViewModel()
    {
        Input = new ReactiveProperty<string>("");
        Output = Input
            .Delay(TimeSpan.FromSeconds(1))
            .Select(x => x.ToUpper())
            .ToReadOnlyReactiveProperty();
    }
}
```

It's LINQ and Rx magic.

All steps are written in the "Getting Started" section in the [ReactiveProperty documentation](https://runceel.github.io/ReactiveProperty/).

This library's concept is "Fun programing". 
ViewModel code using ReactiveProperties is very simple.


ViewModel's popular implementation:
```cs
public class AViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));

            // Update a command status
            DoSomethingCommand.RaiseCanExecuteChanged();
        }
    }

    private string _memo;
    public string Memo
    {
        get => _memo;
        set
        {
            _memo = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Memo)));

            // Update a command status
            DoSomethingCommand.RaiseCanExecuteChanged();
        }
    }

    // DelegateCommand is plane ICommand implementation.
    public DelegateCommand DoSomethingCommand { get; }

    public AViewModel()
    {
        DoSomethingCommand = new DelegateCommand(
            () => { ... },
            () => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Memo)
        );
    }
}
```

Binding code:
```xml
<TextBlock Text="{Binding Name}">
<TextBlock Text="{Binding Memo}">
```

ViewModel's implementation using ReactiveProperty:
```cs
public class AViewModel
{
    public ReactiveProperty<string> Name { get; }
    public ReactiveProperty<string> Memo { get; }
    public ReactiveCommand DoSomethingCommand { get; }

    public AViewModel()
    {
        Name = new ReactiveProperty<string>()
            .SetValidateNotifyError(x => string.IsNullOrEmpty(x) ? "Invalid value" : null);
        Memo = new ReactiveProperty<string>()
            .SetValidateNotifyError(x => string.IsNullOrEmpty(x) ? "Invalid value" : null);
        DoSomethingCommand = new[]
            {
                Name.ObserveHasErrors,
                Memo.ObserveHasErrors,
            }
            .CombineLatestValuesAreAllFalse()
            .ToReactiveCommand()
            .WithSubscribe(() => { ... });
    }
}
```

Binding code:
```xml
<TextBlock Text="{Binding Name.Value}">
<TextBlock Text="{Binding Memo.Value}">
```

It's very simple.

ReactiveProperty doesn't provide base class by ViewModel, which means that ReactiveProperty can be used together with another MVVM library like Prism, MVVMLight, etc...

## Help support ReactiveProperty

|Name|GitHub Sponsors|
|----|----|
|runceel|https://github.com/sponsors/runceel|

## Documentation

[ReactiveProperty documentation](https://runceel.github.io/ReactiveProperty/)

## NuGet packages

|Package Id|Version and downloads|Description|
|----|----|----|
|[ReactiveProperty](https://www.nuget.org/packages/ReactiveProperty/)|![](https://img.shields.io/nuget/v/ReactiveProperty.svg)![](https://img.shields.io/nuget/dt/ReactiveProperty.svg)|The package includes all core features, and the target platform is .NET Standard 2.0. It fits almost all situations.|
|[ReactiveProperty.Core](https://www.nuget.org/packages/ReactiveProperty.Core/)|![](https://img.shields.io/nuget/v/ReactiveProperty.Core.svg)![](https://img.shields.io/nuget/dt/ReactiveProperty.Core.svg)|The package includes minimum classes such as `ReactivePropertySlim<T>` and `ReadOnlyReactivePropertySlim<T>`. And this doesn't have any dependency even System.Reactive. If you don't need Rx features, then it fits.|
|[ReactiveProperty.WPF](https://www.nuget.org/packages/ReactiveProperty.WPF/)|![](https://img.shields.io/nuget/v/ReactiveProperty.WPF.svg)![](https://img.shields.io/nuget/dt/ReactiveProperty.WPF.svg)|The package includes EventToReactiveProperty and EventToReactiveCommand for WPF. This is for .NET Core 3.0 or later and .NET Framework 4.6.1 or later.|
|[ReactiveProperty.UWP](https://www.nuget.org/packages/ReactiveProperty.UWP/)|![](https://img.shields.io/nuget/v/ReactiveProperty.UWP.svg)![](https://img.shields.io/nuget/dt/ReactiveProperty.UWP.svg)|The package includes EventToReactiveProperty and EventToReactiveCommand for UWP.|
|[ReactiveProperty.XamarinAndroid](https://www.nuget.org/packages/ReactiveProperty.XamarinAndroid/)|![](https://img.shields.io/nuget/v/ReactiveProperty.XamarinAndroid.svg)![](https://img.shields.io/nuget/dt/ReactiveProperty.XamarinAndroid.svg)|The package includes many extension methods to create IObservable from events for Xamarin.Android native.|
|[ReactiveProperty.XamariniOS](https://www.nuget.org/packages/ReactiveProperty.XamariniOS/)|![](https://img.shields.io/nuget/v/ReactiveProperty.XamariniOS.svg)![](https://img.shields.io/nuget/dt/ReactiveProperty.XamariniOS.svg)|The package includes many extension methods to bind ReactiveProperty and ReactiveCommand to Xamarin.iOS native controls.|

## Support

I'm not watching StackOverflow and other forums to support ReactiveProperty, so please feel free to post questions at Github issues.
I'm available Japanese(1st language) and English(2nd language).

If too many questions are posted, then I plan to separate posting place about feature requests, issues, questions.

## Author info

Yoshifumi Kawai a.k.a. [@neuecc](https://twitter.com/neuecc) is Founder/CEO/CTO of Cysharp, Inc in Tokyo, Japan.
Awarded Microsoft MVP for Visual Studio and Development Technologies since April, 2011.
He is an original owner of ReactiveProperty.

Takaaki Suzuki a.k.a. [@xin9le](https://twitter.com/xin9le) software developer in Tokyo, Japan.
Awarded Microsoft MVP for Visual Studio and Development Technologies since July, 2012.

Kazuki Ota a.k.a. [@okazuki](https://twitter.com/okazuki) software developer in Tokyo, Japan.
Awarded Microsoft MVP for Windows Development since July 2011 to Feb 2017.
Now, working at Microsoft Japan.
