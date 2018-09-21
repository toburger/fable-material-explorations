module fable_material

open Fable.Helpers.MaterialUI
open Fable.MaterialUI.Props
open Fable.Import
open Fable.Import.React
open Fable.MaterialUI.Themes
open Fable.Core.JsInterop
open Fable.Core
open Fable.Helpers.React.Props
open Fable.Import.React

importSideEffects "./App.css"

module R = Fable.Helpers.React
module Mui = Fable.Helpers.MaterialUI
module Colors = Fable.MaterialUI.Colors

type [<StringEnum>] [<RequireQualifiedAccess>] AxisType =
    | X
    | [<CompiledName "x-reverse">] XReverse
    | Y
    | [<CompiledName "y-reverse">] YReverse

type SwitchType = Move | End

type SwipeableViewsProps =
    | Axis of AxisType
    | Index of int
    | AnimateTransitions of bool
    | Disabled of bool
    | EnableMouseEvents of bool
    | Resistance of bool
    | OnChangeIndex of ((int * int) -> unit)
    | OnSwitching of ((int * SwitchType) -> unit)
    | OnTransitionEnd of (unit -> unit)

let swipeableViews (props: SwipeableViewsProps list) children =
    R.ofImport "default" "react-swipeable-views" (keyValueList CaseRules.LowerFirst props) children

let toObj = keyValueList CaseRules.LowerFirst

let AriaLabel (content: string) = HTMLAttr.Custom ("aria-label", content)
let AriaExpanded (expanded: bool) = HTMLAttr.Custom ("aria-expanded", expanded)

let styles (theme: ITheme): IStyles list =
    let smBreakpoint = theme.breakpoints.up(U2.Case1 MaterialSize.Sm)
    [
        Styles.Root [
            Display "flex"
            FlexGrow 1
            FlexDirection "column"
            AlignItems "center"
        ]
        Styles.Custom
            ("appbar", [
                MarginBottom "10px"
            ] |> toObj)
        Styles.Custom
            ("flex", [
                FlexGrow 1
            ] |> toObj)
        Styles.Custom
            ("card", [
                MaxWidth 400
                FlexAlign "center"
            ] |> toObj)
        Styles.Custom
            ("media", [
                PaddingTop "56.25%"
                TransitionProperty "height"
                TransitionDuration "1s"
            ] |> toObj)
        Styles.Custom
            ("avatar", [
                BackgroundColor Colors.red.``500``
            ] |> toObj)
        Styles.Custom
            ("expand", [
                Transform "rotate(0deg)"
                Transition
                    (theme?transitions?create("transform",
                        createObj [
                            "duration" ==> theme?transitions?duration?shortest
                        ]))
                MarginLeft "auto"
                CSSProp.Custom(
                    smBreakpoint,
                    [ MarginRight "-8" ] |> toObj
                )
            ] |> toObj)
        Styles.Custom
            ("expandOpen", [
                Transform "rotate(180deg)"
            ] |> toObj)
        Styles.Custom
            ("highlight",
                (match theme.palette.``type`` with
                 | PaletteType.Light ->
                    [ Color theme.palette.secondary.main
                      BackgroundColor (ColorManipulator.lighten(theme.palette.secondary.light, 0.85)) ] |> toObj
                 | PaletteType.Dark ->
                    [ Color theme.palette.text.primary
                      BackgroundColor theme.palette.secondary.dark ] |> toObj))
    ]

let node x =
    ReactNode.Case1 (ReactChild.Case1 x)

module Icons =
    let inline private icon path =
        let icon = importDefault<Fable.Import.React.ComponentClass<IHTMLProp>> path
        materialEl icon [] []

    let favoriteIcon = icon "@material-ui/icons/Favorite"
    let shareIcon = icon "@material-ui/icons/Share"
    let expandMoreIcon = icon "@material-ui/icons/ExpandMore"
    let moreVertIcon = icon "@material-ui/icons/MoreVert"

type Food =
    { name: string
      calories: float
      fat: float
      carbs: float
      protein: float }

// [<StringEnum>]
type View =
    | TableView
    | CardView
    member self.index =
        match self with
        | TableView -> 0
        | CardView -> 1
    static member getByIndex(idx) =
        match idx with
        | 0 -> TableView
        | 1 -> CardView
        | _ -> failwithf "unknown view index %i" idx

type Model =
    { activeView: View
      expanded: bool
      showMedia: bool
      text: string
      foods: (int * Food) list
      selectedFoods: Set<int> }
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

type RootProps =
    abstract member model: Model with get, set
    abstract member dispatch: (Msg -> unit) with get, set
    inherit IClassesProps

let paragraph content =
    Mui.typography [
        TypographyProp.Paragraph true
    ] [
        content
    ]
let avatar class' onClick =
    Mui.avatar [
        Class class'
        AriaLabel "Recipe"
        OnClick (ignore >> onClick)
    ] [ R.str "R" ]

let viewCard (props: RootProps) =
    let classes = props?classes
    Mui.card [
        Class !!classes?card
    ] [
        Mui.cardHeader [
            CardHeaderProp.Avatar (node (avatar !!classes?avatar (fun () -> props.dispatch ToggleMedia)))
            CardHeaderProp.Action (node (Mui.iconButton [] [ Icons.moreVertIcon ]))
            CardHeaderProp.Title (node (R.str "Shrimp and Chorizo Paella"))
            CardHeaderProp.Subheader (node (R.str "September 14, 2016"))
        ] []
        (if props.model.showMedia then
            Mui.cardMedia [
                Class !!classes?media
                CardMediaProp.Image "https://material-ui.com/static/images/cards/paella.jpg"
                Title "Shrimp and Chorizo Paella"
            ]
         else
            null)
        Mui.cardContent [] [
            Mui.typography [
                HTMLAttr.Custom ("component", "p")
            ] [
                R.str """This impressive paella is a perfect party dish and a fun meal to cook together with your
guests. Add 1 cup of frozen peas along with the mussels, if you like."""
            ]
        ]
        Mui.cardActions [
            Class !!classes?action
            DisableActionSpacing true
        ] [
            Mui.iconButton [ AriaLabel "Add to favorites" ] [ Icons.favoriteIcon ]
            Mui.iconButton [ AriaLabel "Share" ] [ Icons.shareIcon ]
            Mui.iconButton [
                OnClick (fun _ -> props.dispatch ToggleExpansion)
                R.classBaseList !!classes?expand [!!classes?expandOpen, props.model.expanded]
                AriaLabel "Show more"
                AriaExpanded props.model.expanded
            ] [ Icons.expandMoreIcon ]
        ]
        Mui.collapse [
            Timeout (AutoTransitionDuration.Case3 AutoEnum.Auto)
            MaterialProp.In props.model.expanded
        ] [
            Mui.cardContent [] [
                Mui.typography [
                    TypographyProp.Paragraph true
                    TypographyProp.Variant TypographyVariant.Body2
                ] [ R.str "Method:" ]
                paragraph (R.str """Heat 1/2 cup of the broth in a pot until simmering, add saffron and set aside for 10 minutes.""")
                paragraph (R.str """Heat oil in a (14- to 16-inch) paella pan or a large, deep skillet over medium-high
heat. Add chicken, shrimp and chorizo, and cook, stirring occasionally until lightly
browned, 6 to 8 minutes. Transfer shrimp to a large plate and set aside, leaving
chicken and chorizo in the pan. Add pimentón, bay leaves, garlic, tomatoes, onion,
salt and pepper, and cook, stirring often until thickened and fragrant, about 10
minutes. Add saffron broth and remaining 4 1/2 cups chicken broth; bring to a boil."""
                )
                paragraph (R.str """Add rice and stir very gently to distribute. Top with artichokes and peppers, and cook
without stirring, until most of the liquid is absorbed, 15 to 18 minutes. Reduce heat
to medium-low, add reserved shrimp and mussels, tucking them down into the rice, and
cook again without stirring, until mussels have opened and rice is just tender, 5 to 7
minutes more. (Discard any mussels that don’t open.)""")
                paragraph (R.str """Set aside off of the heat to let rest for 10 minutes, and then serve.""")
                Mui.formControl [
                    MaterialProp.FullWidth true
                ] [
                    Mui.textField [
                        MaterialProp.Label (node (R.str "Enter some text"))
                        OnInput (fun e -> props.dispatch (TextInput (string e.target?value)))
                        TextFieldProp.Variant TextFieldVariant.Standard
                        TextFieldProp.Select false
                        Value props.model.text
                    ] []
                ]
            ]
        ]
    ]

let tabContainer children =
    Mui.typography [
        MaterialProp.Component (ReactType.Case1 "div")
        Style [
            Padding (8*3)
            AlignItems "center"
            JustifyContent "center"
            FlexDirection "column"
            Display "flex"
        ]
    ] children

let viewAppBar (props: RootProps) =
    let classes = props?classes
    Mui.appBar [
        Class !!props.classes?appbar
        AppBarProp.Position AppBarPosition.Static
        MaterialProp.Color ComponentColor.Default
    ] [
        Mui.toolbar [] [
            Mui.typography [
                TypographyProp.Variant TypographyVariant.Title
                MaterialProp.Color ComponentColor.Inherit
            ] [ R.str "App" ]
            Mui.tabs [
                TabsProp.Centered true
                TabsProp.OnChange (fun _ idx ->
                    props.dispatch (SetActiveView (View.getByIndex idx)))
                Class !!classes?flex
                MaterialProp.Value props.model.activeView.index
            ] [
                Mui.tab [ MaterialProp.Label (node (R.str "Table")) ]
                Mui.tab [ MaterialProp.Label (node (R.str "Card")) ]
            ]
            R.div [] [
                Mui.button [
                    MaterialProp.Color ComponentColor.Inherit
                ] [ R.str "Login" ]
            ]
        ]
    ]

let cellWithTooltip props content =
    Mui.tableCell
        ([ TableCellProp.SortDirection TableCellSortDirection.Asc ] @ props) [
        Mui.tooltip [
            Title "Sort"
            Placement PlacementType.BottomStart
            EnterDelay 300
        ] [
            Mui.tableSortLabel [
                MaterialProp.Active false
                TableSortLabelProp.Direction TableSortDirection.Asc
            ] content
        ]
    ]

let viewTable classes allFoodsSelected selectedFoods foods dispatch =
    Mui.paper [ Style [ Width "100%" ] ] [
        Mui.toolbar [
            R.classList [ !!classes?highlight, not (Set.isEmpty selectedFoods)]
        ] [
            R.div [] [
                (if Set.isEmpty selectedFoods then
                    Mui.typography [
                        TypographyProp.Color TypographyColor.Inherit
                        TypographyProp.Variant TypographyVariant.Title
                    ] [ R.str "Nutrition" ]
                 else
                    Mui.typography [
                        TypographyProp.Color TypographyColor.Inherit
                        TypographyProp.Variant TypographyVariant.Subheading
                    ] [ R.str (sprintf "%i selected" selectedFoods.Count) ])
            ]
        ]
        Mui.table [] [
            Mui.tableHead [] [
                Mui.tableRow [] [
                    Mui.tableCell [
                        TableCellProp.Padding TableCellPadding.Checkbox
                    ] [
                        Mui.checkbox [
                            CheckboxProp.Indeterminate (not (Set.isEmpty selectedFoods) && not allFoodsSelected)
                            Checked allFoodsSelected
                            OnChange (fun _ -> dispatch SelectAllFoods)
                        ]
                    ]
                    cellWithTooltip [
                        TableCellProp.SortDirection TableCellSortDirection.Asc
                    ] [ R.str "Dessert (100g serving)"]
                    cellWithTooltip [ TableCellProp.Numeric true ] [ R.str "Calories" ]
                    cellWithTooltip [ TableCellProp.Numeric true ] [ R.str "Fat (g)" ]
                    cellWithTooltip [ TableCellProp.Numeric true ] [ R.str "Carbs (g)" ]
                    cellWithTooltip [ TableCellProp.Numeric true ] [ R.str "Protein (g)" ]
                ]
            ]
            (if List.length foods > 0 then
                Mui.tableBody [] [
                    for (id, food) in foods ->
                        Mui.tableRow [ Key (string id) ] [
                            Mui.tableCell [
                                TableCellProp.Padding TableCellPadding.Checkbox
                            ] [
                                Mui.checkbox [
                                    CheckboxProp.Indeterminate false
                                    Checked (selectedFoods |> Set.contains id)
                                    OnChange (fun _ -> dispatch (SelectFood id))
                                ]
                            ]
                            Mui.tableCell [
                                MaterialProp.Component (ReactType.Case1 "th")
                                Scope "row"
                            ] [ R.str food.name ]
                            Mui.tableCell [
                                TableCellProp.Numeric true
                            ] [ R.str (string food.calories) ]
                            Mui.tableCell [
                                TableCellProp.Numeric true
                            ] [ R.str (string food.fat) ]
                            Mui.tableCell [
                                TableCellProp.Numeric true
                            ] [ R.str (string food.carbs) ]
                            Mui.tableCell [
                                TableCellProp.Numeric true
                            ] [ R.str (string food.protein) ]
                        ]
                ]
            else null)
        ]

    ]

let rootView (props: RootProps) =
    let classes = props?classes
    R.div [ Class !!classes?root ] [
        viewAppBar props
        swipeableViews [
            Axis AxisType.X
            Index props.model.activeView.index
        ] [
            tabContainer [
                Elmish.React.Common.lazyView3
                    (viewTable classes props.model.allFoodsSelected)
                    props.model.selectedFoods
                    props.model.foods
                    props.dispatch
            ]
            tabContainer [
                Elmish.React.Common.lazyViewWith
                    (fun (a: RootProps) b ->
                        a.model.expanded = b.model.expanded &&
                        a.model.showMedia = b.model.showMedia &&
                        a.model.text = b.model.text)
                    viewCard
                    props
            ]
        ]
    ]

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
    { activeView = TableView
      expanded = false
      showMedia = true
      text = ""
      foods = List.indexed (List.collect id (List.replicate 20 foods))
      selectedFoods = Set.empty }

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

let withStyles<'a> = Mui.withStyles (StyleType.Func styles) []

type RootComponent(p) =
    inherit PureComponent<RootProps,unit>(p)
    let ws = R.from (rootView |> withStyles)
    override this.render() =
        ws this.props []

let view model dispatch =
    let props = createEmpty<RootProps>
    props.dispatch <- dispatch
    props.model <- model
    R.ofType<RootComponent, _, _> props []

open Elmish
open Elmish.React

Program.mkSimple init update view
|> Program.withReact "app"
|> Program.withConsoleTrace
|> Program.run
