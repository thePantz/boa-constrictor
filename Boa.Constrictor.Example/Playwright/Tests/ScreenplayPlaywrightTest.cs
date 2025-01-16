using System.Text.RegularExpressions;

namespace Boa.Constrictor.Example;

using System.Threading.Tasks;
using Boa.Constrictor.Playwright;
using Boa.Constrictor.Playwright.Abilities;
using Boa.Constrictor.Screenplay;
using FluentAssertions;
using Microsoft.Playwright;
using NUnit.Framework;

public class ScreenplayPlaywrightTest
{
    private IActor Actor;

    [SetUp]
    public async Task Setup()
    {
        var options = new BrowserTypeLaunchOptions()
        {
            Headless = false
        };
        Actor = new Actor("ThePantz", new ConsoleLogger());
        Actor.Can(await BrowseTheWebSynchronously.UsingChromium(options));
    }

    [Test]
    public async Task CheckboxTest()
    {
        await Actor.AttemptsToAsync(OpenNewPage.ToUrl(CheckboxPage.Url));
        
        await Actor.AttemptsToAsync(SetChecked.On(CheckboxPage.Checkbox1));
        await Actor.AttemptsToAsync(SetChecked.Off(CheckboxPage.Checkbox2));

        await Actor.Expects(CheckboxPage.Checkbox1).ToBeCheckedAsync();
        await Actor.Expects(CheckboxPage.Checkbox2).Not.ToBeCheckedAsync();
    }
    
    [Test]
    public async Task DropdownTest()
    {
        await Actor.AttemptsToAsync(OpenNewPage.ToUrl(DropdownPage.Url));
        
        await Actor.AttemptsToAsync(SelectOption.ByLabel(DropdownPage.Dropdown, "Option 1"));
        await Actor.Expects(DropdownPage.Dropdown).ToHaveValueAsync("1");
        
        await Actor.AttemptsToAsync(SelectOption.ByValue(DropdownPage.Dropdown, "2"));
        await Actor.Expects(DropdownPage.Dropdown).ToHaveValueAsync("2");
        
        await Actor.AttemptsToAsync(SelectOption.ByIndex(DropdownPage.Dropdown, 1));
        await Actor.Expects(DropdownPage.Dropdown).ToHaveValueAsync("1");
    }

    [Test]
    public async Task LoginTest()
    {
        await Actor.AttemptsToAsync(OpenNewPage.ToUrl(LoginPage.Url));

        await Actor.AttemptsToAsync(Fill.ValueTo(LoginPage.UsernameInput, "tomsmith"));
        await Actor.AttemptsToAsync(Fill.ValueTo(LoginPage.PasswordInput, "SuperSecretPassword!"));
        await Actor.AttemptsToAsync(Click.On(LoginPage.LoginButton));

        await Actor.ExpectsPage().ToHaveURLAsync(new Regex(".*/secure"));
    }
}