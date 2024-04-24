Imports System
Imports System.Threading.Tasks
Imports PuppeteerSharp


Module Program
    Sub Main()
        ' Call the asynchronous method and block until it completes.
        MainAsync().GetAwaiter().GetResult()
        Console.WriteLine("Press any key to exit...")
        Console.ReadKey()
    End Sub

    Async Function MainAsync() As Task
        Dim googleSearch As String = "James Spencer Locknet"

        Await New BrowserFetcher().DownloadAsync()

        Dim browser As Browser = Await Puppeteer.LaunchAsync(New LaunchOptions With {.Headless = False})
        Dim page As Page = Await browser.NewPageAsync()
        Await page.GoToAsync("https://www.google.com")
        Await page.TypeAsync("[name='q']", googleSearch)
        'This didnt work so I used EvaluateFunctionAsync to use JavaScript to submit the form
        'Await page.Keyboard.PressAsync("Enter")
        Await page.EvaluateFunctionAsync("() => { document.querySelector('[name=q]').form.submit(); }")

        Dim selector As String = "h3.LC20lb.MBeuO.DKV0Md"
        Await page.WaitForSelectorAsync(selector)
        Dim elements = Await page.QuerySelectorAllAsync(selector)

        If elements.Length > 0 Then
            Dim searchResults As New List(Of String)()
            For Each element In elements
                Dim resultText As String = Await element.EvaluateFunctionAsync(Of String)("node => node.textContent")
                searchResults.Add(resultText)
            Next

            For Each result In searchResults
                Console.WriteLine(result)
            Next

            ' Clicking on LinkedIn profile
            Await elements(0).ClickAsync()
            Await Task.Delay(2000) 
        End If


    End Function
End Module

