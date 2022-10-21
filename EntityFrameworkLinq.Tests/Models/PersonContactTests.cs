﻿using System.Reflection;
using EntityFrameworkLinq.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using NUnit.Framework;

namespace EntityFrameworkLinq.Tests.Models
{
    [TestFixture]
    public class PersonContactTests : ModelTestBase<PersonContact>
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
            Assert.AreEqual(0, this.ClassType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length, "Checking fields number");
            Assert.AreEqual(0, this.ClassType.GetFields(BindingFlags.Instance | BindingFlags.Public).Length, "Checking fields number");
            Assert.AreEqual(6, this.ClassType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic).Length, "Checking fields number");

            Assert.AreEqual(0, this.ClassType.GetConstructors(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length, "Checking constructor number");
            Assert.AreEqual(1, this.ClassType.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length, "Checking constructor number");
            Assert.AreEqual(0, this.ClassType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).Length, "Checking constructor number");

            Assert.AreEqual(0, this.ClassType.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length, "Checking properties number");
            Assert.AreEqual(6, this.ClassType.GetProperties(BindingFlags.Instance | BindingFlags.Public).Length, "Checking properties number");
            Assert.AreEqual(0, this.ClassType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic).Length, "Checking properties number");

            Assert.AreEqual(0, this.ClassType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly).Length, "Checking methods number");
            Assert.AreEqual(0, this.ClassType.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).Length, "Checking methods number");

            Assert.AreEqual(12, this.ClassType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Length, "Checking methods number");
            Assert.AreEqual(0, this.ClassType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).Length, "Checking methods number");

            Assert.AreEqual(0, this.ClassType.GetEvents(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length, "Checking events number");
        }

        [TestCase("person_contacts")]
        public void HasTableAttribute(string tableName)
        {
            this.AssertThatHasTableAttribute(tableName);
        }

        [TestCase("Id", typeof(int), "person_contact_id")]
        [TestCase("PersonId", typeof(int), "person_id")]
        [TestCase("ContactTypeId", typeof(int), "contact_type_id")]
        [TestCase("Value", typeof(string), "contact_value")]
        [TestCase("Person", typeof(Person), null)]
        [TestCase("Type", typeof(ContactType), null)]
        public void HasProperty(string propertyName, Type propertyType, string columnName)
        {
            this.AssertThatClassHasProperty(propertyName, propertyType, columnName);
        }

        [TestCase("Id")]
        public void HasKeyAttribute(string propertyName)
        {
            this.AssertThatPropertyHasKeyAttribute(propertyName);
        }

        [TestCase("PersonId", "Person")]
        [TestCase("ContactTypeId", "ContactType")]
        public void HasForeignKeyAttribute(string propertyName, string navigationPropertyName)
        {
            this.AssertThatPropertyHasForeignKeyAttribute(propertyName, navigationPropertyName);
        }
    }
}
