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
      selectedFoods = Set.empty
      expandedPanel = None }

let update msg model =
    match msg with
    | SetActiveView view ->
        { model with activeView = view }
    | ToggleExpansion ->
        { model with expanded = not model.expanded }
    | TextInput text ->
        { model with text = text }
    | ToggleMedia ->
        { model with showMedia = not model.showMedia }
    | SelectFood id ->
        { model with
            selectedFoods =
                if model.selectedFoods.Contains id then
                    Set.remove id model.selectedFoods
                else
                    Set.add id model.selectedFoods }
    | SelectAllFoods ->
        { model with
            selectedFoods =
                if model.allFoodsSelected then
                    Set.empty
                else
                    model.foods |> List.map fst |> set }
    | ChangeExpandedPanel panel ->
        { model with
            expandedPanel = panel }

Program.mkSimple init update View.view
|> Program.withReact "app"
|> Program.withConsoleTrace
|> Program.run
