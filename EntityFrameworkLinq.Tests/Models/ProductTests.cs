using System.Reflection;
using EntityFrameworkLinq.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using NUnit.Framework;

namespace EntityFrameworkLinq.Tests.Models
{
    [TestFixture]
    public class ProductTests : ModelTestBase<Product>
    {
        [Test]
        public void IsPublicClass()
        {
            this.AssertThatClassIsPublic(false);
        }

        [Test]
        public void InheritsObject()
        {
            this.AssertThatClassInheritsObject();
        }

        [Test]
        public void HasRequiredMembers()
        {
            Assert.AreEqual(0, this.ClassType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length);
            Assert.AreEqual(0, this.ClassType.GetFields(BindingFlags.Instance | BindingFlags.Public).Length);
            Assert.AreEqual(10, this.ClassType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Length);

            Assert.AreEqual(0, this.ClassType.GetConstructors(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length);
            Assert.AreEqual(1, this.ClassType.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length);
            Assert.AreEqual(0, this.ClassType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).Length);

            Assert.AreEqual(0, this.ClassType.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length);
            Assert.AreEqual(10, this.ClassType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Length);
            Assert.AreEqual(0, this.ClassType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic).Length);

            Assert.AreEqual(0, this.ClassType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly).Length);
            Assert.AreEqual(0, this.ClassType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).Length);

            Assert.AreEqual(20, this.ClassType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Length);
            Assert.AreEqual(0, this.ClassType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).Length);

            Assert.AreEqual(0, this.ClassType.GetEvents(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length);
        }

        [TestCase("shop_products")]
        public void HasTableAttribute(string tableName)
        {
            this.AssertThatHasTableAttribute(tableName);
        }

        [TestCase("Id", typeof(int), "product_id")]
        [TestCase("TitleId", typeof(int), "product_title_id")]
        [TestCase("ManufacturerId", typeof(int), "product_manufacturer_id")]
        [TestCase("SupplierId", typeof(int), "product_supplier_id")]
        [TestCase("UnitPrice", typeof(decimal), "unit_price")]
        [TestCase("Description", typeof(string), "comment")]
        [TestCase("Title", typeof(ProductTitle), null)]
        [TestCase("Manufacturer", typeof(Manufacturer), null)]
        [TestCase("Supplier", typeof(Supplier), null)]
        [TestCase("OrderDetails", typeof(IList<OrderDetail>), null)]
        public void HasProperty(string propertyName, Type propertyType, string columnName)
        {
            this.AssertThatClassHasProperty(propertyName, propertyType, columnName);
        }

        [TestCase("Id")]
        public void HasKeyAttribute(string propertyName)
        {
            this.AssertThatPropertyHasKeyAttribute(propertyName);
        }

        [TestCase("TitleId", "Title")]
        [TestCase("ManufacturerId", "Manufacturer")]
        [TestCase("SupplierId", "Supplier")]
        public void HasForeignKeyAttribute(string propertyName, string navigationPropertyName)
        {
            this.AssertThatPropertyHasForeignKeyAttribute(propertyName, navigationPropertyName);
        }
    }
}
