﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RxPropTest
{
    [TestClass]
    public class ReactivePropertyStaticTest
    {
        [TestMethod]
        public void FromObject()
        {
            var homuhomu = new Person { Name = "homuhomu", Age = 13 };
            var rxName = Reactive.Bindings.ReactiveProperty.FromObject(homuhomu, x => x.Name);
            rxName.Value.Is("homuhomu");
            rxName.Value = "mami";
            homuhomu.Name.Is("mami");

            var rxAge = Reactive.Bindings.ReactiveProperty.FromObject(homuhomu, x => x.Age);
            rxAge.Value.Is(13);
            rxAge.Value = 20;
            homuhomu.Age.Is(20);
        }

        [TestMethod]
        public void FromObject_NestedPath()
        {
            var homuhomu = new Person { Child = new Person { Name = "homuhomu", Age = 13 } };
            var rxName = Reactive.Bindings.ReactiveProperty.FromObject(homuhomu, x => x.Child.Name);
            rxName.Value.Is("homuhomu");
            rxName.Value = "mami";
            homuhomu.Child.Name.Is("mami");

            var rxAge = Reactive.Bindings.ReactiveProperty.FromObject(homuhomu, x => x.Child.Age);
            rxAge.Value.Is(13);
            rxAge.Value = 20;
            homuhomu.Child.Age.Is(20);
        }

        [TestMethod]
        public void FromObjectIgnoreValidationErrorValue()
        {
            var homuhomu = new Person { Name = "homuhomu", Age = 13 };
            var rxName = Reactive.Bindings.ReactiveProperty
                .FromObject(homuhomu, x => x.Name, ignoreValidationErrorValue: true)
                .SetValidateNotifyError((string x) => string.IsNullOrEmpty(x) ? "error" : null);
            rxName.Value.Is("homuhomu");
            rxName.Value = "mami";
            homuhomu.Name.Is("mami");

            rxName.Value = null; // validation error
            rxName.Value.IsNull();
            homuhomu.Name.Is("mami");

            var rxAge = Reactive.Bindings.ReactiveProperty
                .FromObject(homuhomu, x => x.Age, ignoreValidationErrorValue: true)
                .SetValidateNotifyError((int x) => x >= 0 ? null : "error");

            rxAge.Value.Is(13);
            rxAge.Value = 20;
            homuhomu.Age.Is(20);

            rxAge.Value = -1; // validation error
            rxAge.Value.Is(-1);
            homuhomu.Age.Is(20);

            rxAge.Value = 10;
            homuhomu.Age.Is(10);
        }

        [TestMethod]
        public void FromObjectIgnoreValidationErrorValue_NestedProperty()
        {
            var homuhomu = new Person { Child = new Person { Name = "homuhomu", Age = 13 } };
            var rxName = Reactive.Bindings.ReactiveProperty
                .FromObject(homuhomu, x => x.Child.Name, ignoreValidationErrorValue: true)
                .SetValidateNotifyError((string x) => string.IsNullOrEmpty(x) ? "error" : null);
            rxName.Value.Is("homuhomu");
            rxName.Value = "mami";
            homuhomu.Child.Name.Is("mami");

            rxName.Value = null; // validation error
            rxName.Value.IsNull();
            homuhomu.Child.Name.Is("mami");

            var rxAge = Reactive.Bindings.ReactiveProperty
                .FromObject(homuhomu, x => x.Child.Age, ignoreValidationErrorValue: true)
                .SetValidateNotifyError((int x) => x >= 0 ? null : "error");

            rxAge.Value.Is(13);
            rxAge.Value = 20;
            homuhomu.Child.Age.Is(20);

            rxAge.Value = -1; // validation error
            rxAge.Value.Is(-1);
            homuhomu.Child.Age.Is(20);

            rxAge.Value = 10;
            homuhomu.Child.Age.Is(10);
        }

        [TestMethod]
        public void FromObjectConverter()
        {
            var homuhomu = new Person { Name = "homuhomu", Age = 13 };
            var rxAge = Reactive.Bindings.ReactiveProperty.FromObject(homuhomu, x => x.Age,
                x => Convert.ToString(x, 16), x => Convert.ToInt32(x, 16));

            rxAge.Value.Is("d");
            rxAge.Value = "3f";
            homuhomu.Age.Is(63);
        }

        [TestMethod]
        public void FromObjectConverter_NestedProperty()
        {
            var homuhomu = new Person { Child = new Person { Name = "homuhomu", Age = 13 } };
            var rxAge = Reactive.Bindings.ReactiveProperty.FromObject(homuhomu, x => x.Child.Age,
                x => Convert.ToString(x, 16), x => Convert.ToInt32(x, 16));

            rxAge.Value.Is("d");
            rxAge.Value = "3f";
            homuhomu.Child.Age.Is(63);
        }

        [TestMethod]
        public void FromObjectConverterIgnoreValidationErrorValue()
        {
            var homuhomu = new Person { Name = "homuhomu", Age = 13 };
            var rxAge = Reactive.Bindings.ReactiveProperty.FromObject(homuhomu, x => x.Age,
                x => Convert.ToString(x, 16), x => Convert.ToInt32(x, 16),
                ignoreValidationErrorValue: true)
                .SetValidateNotifyError((string x) =>
                {
                    try
                    {
                        Convert.ToInt32(x, 16);
                        return null;
                    }
                    catch
                    {
                        return "error";
                    }
                });

            rxAge.Value.Is("d");
            rxAge.Value = "3f";
            homuhomu.Age.Is(63);

            rxAge.Value = "XXX"; // validation error;
            rxAge.Value.Is("XXX");
            homuhomu.Age.Is(63);
        }

        [TestMethod]
        public void FromObjectConverterIgnoreValidationErrorValue_NestedProperty()
        {
            var homuhomu = new Person { Child = new Person { Name = "homuhomu", Age = 13 } };
            var rxAge = Reactive.Bindings.ReactiveProperty.FromObject(homuhomu, x => x.Child.Age,
                x => Convert.ToString(x, 16), x => Convert.ToInt32(x, 16),
                ignoreValidationErrorValue: true)
                .SetValidateNotifyError((string x) =>
                {
                    try
                    {
                        Convert.ToInt32(x, 16);
                        return null;
                    }
                    catch
                    {
                        return "error";
                    }
                });

            rxAge.Value.Is("d");
            rxAge.Value = "3f";
            homuhomu.Child.Age.Is(63);

            rxAge.Value = "XXX"; // validation error;
            rxAge.Value.Is("XXX");
            homuhomu.Child.Age.Is(63);
        }

        private class Person
        {
            public string Name { get; set; }

            public int Age { get; set; }

            public Person Child { get; set; }
        }
    }
}
