﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Codeplex.Reactive;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reactive.Concurrency;

namespace RxPropTest
{
    // from http://d.hatena.ne.jp/okazuki/20111019/1318985756
    // thanks @okazuki

    [TestClass]
    public class ReactivePropertyValidationTest
    {
        [TestMethod]
        public void NoErrorValidationTest_SetValidateAttribute()
        {
            var target = new Target();
            target.Name.Value = "sample";
            ((IDataErrorInfo)target.Name)["Value"].IsNull();
            target.Name.Error.IsNull();
        }

        [TestMethod]
        public void ErrorValidationTest_SetValidateAttribute()
        {
            var target = new Target();
            target.Name.Value = "";
            ((IDataErrorInfo)target.Name)["Value"].Is("ErrorMessage");
            target.Name.Error.Is("ErrorMessage");
        }

        [TestMethod]
        public void NoErrorValidationTest_SetValidateError()
        {
            var target = new Target();
            target.Age.Value = 13;
            ((IDataErrorInfo)target.Age)["Value"].IsNull();
            target.Age.Error.IsNull();
        }

        [TestMethod]
        public void ErrorValidationTest_SetValidateError()
        {
            var target = new Target();
            target.Age.Value = -1;
            ((IDataErrorInfo)target.Age)["Value"].Is("ErrorMessage");
            target.Age.Error.Is("ErrorMessage");
        }
    }

    class Target
    {
        [Required(ErrorMessage = "ErrorMessage")]
        public ReactiveProperty<string> Name { get; private set; }

        public ReactiveProperty<int> Age { get; private set; }

        public Target()
        {
            this.Name = new ReactiveProperty<string>()
                .SetValidateAttribute(() => Name);

            this.Age = new ReactiveProperty<int>()
                .SetValidateError(i => i < 0 ? "ErrorMessage" : null);
        }
    }
}