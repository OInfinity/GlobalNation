namespace GlobalNation

open WebSharper
open WebSharper.UI
open WebSharper.UI.Templating
open WebSharper.UI.Notation

[<JavaScript>]
module Templates =

    type MainTemplate = Templating.Template<"Main.html", ClientLoad.FromDocument, ServerLoad.WhenChanged>

[<JavaScript>]
module Client =

    let Main () =
        // Reactive variable to hold the search query
        let searchQuery = Var.Create ""

        // Function to filter country names based on search query
        let filterCountryNames query =
            let filteredQuery = query.ToLower()
            // Replace this with actual data or a server call to fetch country names
            let countryNames = ["Afghanistan"; "Albania"; "Algeria"; "Andorra"; "Angola"; "Antigua and Barbuda"; "Argentina"; "Armenia"; "Australia"; "Austria"; "Azerbaijan"]
            countryNames
            |> List.filter (fun name -> name.ToLower().StartsWith(filteredQuery))

        // Function to update search results based on search query
        let updateSearchResults () =
            let query = searchQuery.Value
            let filteredNames = filterCountryNames query
            let resultHtml = filteredNames |> List.map (fun name -> sprintf "<div class='search-results-item'>%s</div>" name) |> String.concat ""
            Browser.Document.querySelector(".search-results").innerHTML <- resultHtml
            Browser.Document.querySelector(".search-results").style.display <- if String.IsNullOrWhiteSpace query || List.isEmpty filteredNames then "none" else "block"

        // Attach event listener to search input for keyup event
        Browser.Document.querySelector("#search-input").addEventListener("keyup", fun _ ->
            searchQuery.Value <- Browser.Document.querySelector("#search-input").value
            updateSearchResults()
        )

        // Attach event listener to search button for click event
        Browser.Document.querySelector("#search-button").addEventListener("click", fun _ ->
            searchQuery.Value <- Browser.Document.querySelector("#search-input").value
            updateSearchResults()
        )

        // Update search results when the page loads
        updateSearchResults()

        // Render the main form template
        Templates.MainTemplate.MainForm()
            .OnSend(fun _ ->
                // No server-side action needed for search, so just return a default message
                async { return "Search complete" }
            )
            .Reversed(searchQuery.View) // Update form with search query value
            .Doc()
