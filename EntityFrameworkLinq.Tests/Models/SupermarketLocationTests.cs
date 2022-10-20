﻿using System.Reflection;
using EntityFrameworkLinq.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using NUnit.Framework;

namespace EntityFrameworkLinq.Tests.Models
{
    [TestFixture]
    public class SupermarketLocationTests : ModelTestBase<SupermarketLocation>
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
            Assert.AreEqual(6, this.ClassType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Length);

            Assert.AreEqual(0, this.ClassType.GetConstructors(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length);
            Assert.AreEqual(1, this.ClassType.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length);
            Assert.AreEqual(0, this.ClassType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).Length);

            Assert.AreEqual(0, this.ClassType.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length);
            Assert.AreEqual(6, this.ClassType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Length);
            Assert.AreEqual(0, this.ClassType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic).Length);

            Assert.AreEqual(0, this.ClassType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly).Length);
            Assert.AreEqual(0, this.ClassType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).Length);

            Assert.AreEqual(12, this.ClassType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Length);
            Assert.AreEqual(0, this.ClassType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).Length);

            Assert.AreEqual(0, this.ClassType.GetEvents(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length);
        }

        [TestCase("supermarket_locations")]
        public void HasTableAttribute(string tableName)
        {
            this.AssertThatHasTableAttribute(tableName);
        }

        [TestCase("Id", typeof(int), "supermarket_location_id")]
        [TestCase("SupermarketId", typeof(int), "supermarket_id")]
        [TestCase("LocationId", typeof(int), "location_id")]
        [TestCase("Supermarket", typeof(Supermarket), null)]
        [TestCase("Location", typeof(Location), null)]
        [TestCase("Orders", typeof(IList<Order>), null)]
        public void HasProperty(string propertyName, Type propertyType, string columnName)
        {
            this.AssertThatClassHasProperty(propertyName, propertyType, columnName);
        }

        [TestCase("Id")]
        public void HasKeyAttribute(string propertyName)
        {
            this.AssertThatPropertyHasKeyAttribute(propertyName);
        }

        [TestCase("SupermarketId", "Supermarket")]
        [TestCase("LocationId", "Location")]
        public void HasForeignKeyAttribute(string propertyName, string navigationPropertyName)
        {
            this.AssertThatPropertyHasForeignKeyAttribute(propertyName, navigationPropertyName);
        }
    }
}
