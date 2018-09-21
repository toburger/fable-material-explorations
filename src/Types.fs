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

type Model =
    { activeView: SelectedView
      expanded: bool
      showMedia: bool
      text: string
      foods: (int * Food) list
      selectedFoods: Set<int>
      expandedPanel: ExpandedPanel option }
    member self.allFoodsSelected =
        let foods = self.foods |> List.map fst |> set
        self.selectedFoods.Count = foods.Count &&
        Set.isSubset self.selectedFoods foods &&
        Set.isSubset foods self.selectedFoods

type Msg =
    | SetActiveView of SelectedView
    | ToggleExpansion
    | ToggleMedia
    | TextInput of string
    | SelectFood of int
    | SelectAllFoods
    | ChangeExpandedPanel of ExpandedPanel option

type RootProps =
    abstract member model: Model with get, set
    abstract member dispatch: (Msg -> unit) with get, set
    inherit Fable.Helpers.MaterialUI.IClassesProps
