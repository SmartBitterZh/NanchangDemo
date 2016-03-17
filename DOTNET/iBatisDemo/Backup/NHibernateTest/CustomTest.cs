using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BLL;
using System.Collections;
using Model;

namespace NHibernateTest
{
    [TestFixture]
    public class CustomTest
    {
        CustomBLL _customBll;

        [SetUp]
        public void InitData()
        {
            _customBll = new CustomBLL();
        }

        [Test]
        public void SelectAllTest()
        {
            IList customs = _customBll.SelectAll();
            Assert.AreEqual(3, customs.Count);
        }

        [Test]
        public void SelectTest()
        {
            Custom custom = _customBll.Select(3);
            Assert.IsNotNull(custom);

            custom = _customBll.Select(1);
            Assert.AreEqual(1, custom.ID);
        }

        [Test]
        public void InsertTest()
        {
            Custom custom = new Custom();
            custom.CustomName = "444";
            custom.Address = "444 address";
            _customBll.Insert(custom);

            Assert.AreEqual(4, _customBll.SelectAll().Count);

            custom = _customBll.Select(6);
            Assert.AreEqual("444", custom.CustomName);
        }

        [Test]
        public void UpdateTest()
        {
            Custom custom = new Custom();
            custom.CustomName = "555";
            custom.Address = "555 Address";
            custom.ID = 6;
            _customBll.Update(custom);

            custom = _customBll.Select(6);
            Assert.AreEqual("555", custom.CustomName);
            Assert.AreEqual("555 Address", custom.Address);
        }

        [Test]
        public void DeleteTest()
        {
            _customBll.Delete(6);

            IList customs = _customBll.SelectAll();
            Assert.AreEqual(3, customs.Count);

            Custom custom = _customBll.Select(5);
            Assert.IsNull(custom);
        }
    }
}
