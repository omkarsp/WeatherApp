using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using ToastSnackbarPackage;

namespace ToastSnackbarPackage.Tests
{
    public class ToastSnackbarTests
    {
        private GameObject testGameObject;
        private ToastSnackbarManager manager;

        [SetUp]
        public void Setup()
        {
            testGameObject = new GameObject();
            manager = testGameObject.AddComponent<ToastSnackbarManager>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(testGameObject);
        }

        [Test]
        public void ShowMessage_WhenInstanceExists_DoesNotThrowException()
        {
            Assert.DoesNotThrow(() => ToastSnackbarManager.Instance.ShowMessage("Test"));
        }

        [Test]
        public void ShowMessage_WithNullMessage_DoesNotThrowException()
        {
            Assert.DoesNotThrow(() => ToastSnackbarManager.Instance.ShowMessage(null));
        }

        [Test]
        public void ShowMessage_WithEmptyMessage_DoesNotThrowException()
        {
            Assert.DoesNotThrow(() => ToastSnackbarManager.Instance.ShowMessage(string.Empty));
        }

        [Test]
        public void Instance_WhenMultipleCreated_OnlyOneInstanceExists()
        {
            GameObject secondObject = new GameObject();
            ToastSnackbarManager secondManager = secondObject.AddComponent<ToastSnackbarManager>();

            Assert.AreEqual(ToastSnackbarManager.Instance, manager);
            Assert.AreNotEqual(ToastSnackbarManager.Instance, secondManager);

            Object.Destroy(secondObject);
        }
    }
}