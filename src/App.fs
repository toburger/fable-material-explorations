module App

open Fable.Core
open Elmish
open Elmish.React

JsInterop.importSideEffects "./App.css"

let food name calories fat carbs protein =
    { name = name
      calories = calories
      fat = fat
      carbs = carbs
      protein = protein }

let foods = [
    food "Frozen yoghurt" 159. 6.0 24. 4.0
    food "Ice cream sandwich" 237. 9.0 37. 4.3
    food "Eclair" 262. 16.0 24. 6.0
    food "Cupcake" 305. 3.7 67. 4.3
    food "Gingerbread" 356. 16.0 49. 3.9
]

let init () =
    { activeView = SelectedView.ExpansionView
      expanded = false
      showMedia = true
      text = ""
      foods = List.indexed (List.collect id (List.replicate 20 foods))
    //   foods = List.indexed foods
      selectedFoods = Set.empty
      expandedPanel = None
      timerEnabled = false },
    Cmd.none

let update msg model =
    match msg with
    | SetActiveView view ->
        { model with activeView = view },
        Cmd.none
    | ToggleExpansion ->
        { model with expanded = not model.expanded },
        Cmd.none
    | TextInput text ->
        { model with text = text },
        Cmd.none
    | ToggleMedia ->
        { model with showMedia = not model.showMedia },
        Cmd.none
    | SelectFood id ->
        { model with
            selectedFoods =
                if model.selectedFoods.Contains id then
                    Set.remove id model.selectedFoods
                else
                    Set.add id model.selectedFoods },
        Cmd.none
    | SelectAllFoods ->
        { model with
            selectedFoods =
                if model.allFoodsSelected then
                    Set.empty
                else
                    model.foods |> List.map fst |> set },
        Cmd.none
    | ChangeExpandedPanel panel ->
        { model with
            expandedPanel = panel },
        Cmd.none
    | EnableTimer enabled ->
        { model with
            timerEnabled = enabled },
        Cmd.none
    | Tick _ ->
        if model.timerEnabled then
            let rnd = System.Random()
            let expandedPanel =
                match rnd.Next(0, 5) with
                | 1 -> Some ExpandedPanel.Panel1
                | 2 -> Some ExpandedPanel.Panel2
                | 3 -> Some ExpandedPanel.Panel3
                | 4 -> Some ExpandedPanel.Panel4
                | _ -> None
            let randomFoodIdx =
                let foods =
                    model.foods
                    |> List.map fst
                    |> List.toArray
                foods.[rnd.Next(0, foods.Length)]
            let selectedFoods =
                if model.selectedFoods.Contains randomFoodIdx then
                    model.selectedFoods.Remove randomFoodIdx
                else
                    model.selectedFoods.Add randomFoodIdx
            { model with
                selectedFoods = selectedFoods
                expandedPanel = expandedPanel },
            Cmd.none
        else
            model,
            Cmd.none

let timerTick dispatch =
    Fable.Import.Browser.window.setInterval(fun _ -> 
        dispatch (Tick System.DateTime.Now)
    , 500) |> ignore

let subscription _ =
    Cmd.ofSub timerTick

Program.mkProgram init update View.view
|> Program.withSubscription subscription
|> Program.withReact "app"
// |> Program.withConsoleTrace
|> Program.run
