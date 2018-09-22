[<AutoOpen>]
module Types

type Food =
    { name: string
      calories: float
      fat: float
      carbs: float
      protein: float }

type SelectedView =
    | TableView = 0
    | CardView = 1
    | ExpansionView = 2

[<RequireQualifiedAccess>]
type ExpandedPanel =
    | Panel1
    | Panel2
    | Panel3
    | Panel4

type Model =
    { activeView: SelectedView
      expanded: bool
      showMedia: bool
      text: string
      foods: (int * Food) list
      selectedFoods: Set<int>
      expandedPanel: ExpandedPanel option
      timerEnabled: bool }

    member self.allFoodsSelected =
        if self.selectedFoods.Count = self.foods.Length then
            self.foods
            |> List.map fst
            |> set
            |> Set.difference self.selectedFoods
            |> Set.isEmpty
        else
            false

type Msg =
    | SetActiveView of SelectedView
    | ToggleExpansion
    | ToggleMedia
    | TextInput of string
    | SelectFood of int
    | SelectAllFoods
    | ChangeExpandedPanel of ExpandedPanel option
    | EnableTimer of bool
    | Tick of System.DateTime

type RootProps =
    abstract member model: Model with get, set
    abstract member dispatch: (Msg -> unit) with get, set
    inherit Fable.Helpers.MaterialUI.IClassesProps
