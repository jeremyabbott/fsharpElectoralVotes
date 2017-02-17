#r "./packages/FSharp.Data/lib/net40/FSharp.Data.dll"
open FSharp.Data
// Load the FSharp.Charting library
#load "./packages/FSharp.Charting/FSharp.Charting.fsx"
open FSharp.Charting

let [<Literal>] ElectoralVotes = 
  "https://en.wikipedia.org/wiki/List_of_United_States_presidential_elections_by_Electoral_College_margin"

let electoralVotes = new HtmlProvider<ElectoralVotes>()

let top10ElectoralVoteWinners =
    electoralVotes.Tables.``Table of election results``.Rows
    |> Seq.filter(fun r -> r.Column1 <> "—")
    |> Seq.toList
    |> Seq.map(fun r -> r.Column3, r.``Number of electors voting - winner - (w)``)
    |> Seq.toList
    |> List.sortWith (fun a b -> compare (snd b) (snd a))
    |> List.take 10

let top10Chart =
  Chart.Column(top10ElectoralVoteWinners)
    .WithYAxis(Title = "Electoral Votes")
    .WithXAxis(Title = "President", MajorGrid=ChartTypes.Grid(Enabled=true))

let electoralWinner2016 =
  electoralVotes.Tables.``Table of election results``.Rows
  |> Seq.find(fun r -> r.Column3.Contains("Trump"))

let chart2016 = Chart.Column([electoralWinner2016.Column3, electoralWinner2016.``Number of electors voting - winner - (w)``])

Chart.Combine([top10Chart; chart2016])  