﻿using Boa.Constrictor.Screenplay;
using Boa.Constrictor.Selenium;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Boa.Constrictor.Selenium.UnitTests
{
    public class CssValueTest : BaseWebLocatorQuestionTest
    {
        #region Tests

        [Test]
        public void TestCssValue()
        {
            WebDriver.Setup(x => x.FindElement(It.IsAny<By>()).GetCssValue(It.IsAny<string>())).Returns("red");

            Actor.AsksFor(CssValue.Of(Locator, "color")).Should().Be("red");
        }

        [Test]
        public void TestZeroElements()
        {
            SetUpFindElementsReturnsEmpty();

            Actor.Invoking(x => x.AsksFor(CssValue.Of(Locator, "color"))).Should().Throw<WaitingException<bool>>();
        }

        #endregion
    }
}
