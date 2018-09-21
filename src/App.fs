module App

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

let toObj = keyValueList CaseRules.LowerFirst

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

let rootView (props: RootProps) =
    let classes = props?classes
    R.div [ Class !!classes?root ] [
        AppBarView.view props
        swipeableViews [
            Axis AxisType.X
            Index props.model.activeView.index
        ] [
            tabContainer [
                Elmish.React.Common.lazyView3
                    (TableView.view classes props.model.allFoodsSelected)
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
                    CardView.view
                    props
            ]
            tabContainer [
                Elmish.React.Common.lazyView2
                    ExpansionPanelView.view
                    props.model.expandedPanel
                    props.dispatch
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
    { activeView = ExpansionView
      expanded = false
      showMedia = true
      text = ""
      foods = List.indexed (List.collect id (List.replicate 20 foods))
      selectedFoods = Set.empty
      expandedPanel = ExpandedPanel.Panel1 }

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
