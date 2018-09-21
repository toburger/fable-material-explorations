[<AutoOpen>]
module Types

type Food =
    { name: string
      calories: float
      fat: float
      carbs: float
      protein: float }

type View =
    | TableView
    | CardView
    | ExpansionView
    member self.index =
        match self with
        | TableView -> 0
        | CardView -> 1
        | ExpansionView -> 2
    static member getByIndex(idx) =
        match idx with
        | 0 -> TableView
        | 1 -> CardView
        | 2 -> ExpansionView
        | _ -> failwithf "unknown view index %i" idx

[<RequireQualifiedAccess>]
type ExpandedPanel =
    | Panel1
    | Panel2
    | Panel3

type Model =
    { activeView: View
      expanded: bool
      showMedia: bool
      text: string
      foods: (int * Food) list
      selectedFoods: Set<int>
      expandedPanel: ExpandedPanel }
    member self.allFoodsSelected =
        let foods = self.foods |> List.map fst |> set
        self.selectedFoods.Count = foods.Count &&
        Set.isSubset self.selectedFoods foods &&
        Set.isSubset foods self.selectedFoods

type Msg =
    | SetActiveView of View
    | ToggleExpansion
    | ToggleMedia
    | TextInput of string
    | SelectFood of int
    | SelectAllFoods
    | ChangeExpandedPanel of ExpandedPanel

type RootProps =
    abstract member model: Model with get, set
    abstract member dispatch: (Msg -> unit) with get, set
    inherit Fable.Helpers.MaterialUI.IClassesProps
