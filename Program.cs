using System.Text.Json;
using PuppeteerSharp;
using PuppeteerSharp.Input;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

var url = "https://eminus.uv.mx/eminus4/";
var chrome = @"c:\Program Files\Google\Chrome\Application\chrome.exe";
var password = "HTML5semantics";
var username = "zS20020854";
var delay = 100;
var usernameSelector = "#mat-input-0";
var passwordSelector = "#mat-input-1";

using var browserFetcher = new BrowserFetcher();
await browserFetcher.DownloadAsync();
await using var browser = await Puppeteer.LaunchAsync(
    new LaunchOptions
    {
        Headless = false,
        ExecutablePath = chrome,
    }
);

var page = await browser.NewPageAsync();
await page.GoToAsync(url);
//insertar username y password
await TypeFieldValue(page, usernameSelector,username , delay);
await TypeFieldValue(page, passwordSelector,password , delay);
//hacer click y esperar respuesta
await ClickAndAwaitPage(page, ".mat-stroked-button");
//recuperar cursos

var result =
    await page.EvaluateExpressionAsync("Array.from(document.querySelectorAll('.example-card')).map(a=> a.innerText);");

Console.WriteLine(result.ToString());

//hacer clic a un curso
await ClickAndAwaitPage(page,".example-card");
//ir a las tareas
await page.EvaluateExpressionAsync("document.querySelectorAll('a.m-menu__link.m-menu__toggle')[2].click()");
await Task.Delay(2000);
//recuperar tareas
List<string> tareas = new List<string>();
var result1 =
    await page.EvaluateExpressionAsync("Array.from(document.querySelectorAll('.m-alert')).map(a=> a.innerText);");
foreach (var e in result1)
{
    tareas.Add(e.ToString());
}
foreach (var e in tareas)
{
    Console.WriteLine(e);
}

Console.ReadLine();

static async Task TypeFieldValue(Page page, string fieldSelector, string value, int delay = 0)
{
    await page.FocusAsync(fieldSelector);
    await page.TypeAsync(fieldSelector, value, new TypeOptions { Delay = delay });
    await page.Keyboard.PressAsync("Tab");
}

static async Task ClickAndAwaitPage(Page page, String buttonSelector)
{
    var navigationTask = page.WaitForNavigationAsync();
    await page.ClickAsync(buttonSelector);
    await navigationTask;
    await Task.Delay(1000);
}


