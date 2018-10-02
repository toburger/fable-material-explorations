module View

open Fable.Core.JsInterop
open Fable.Import.React
open Fable.MaterialUI.Themes
open Fable.MaterialUI.Props
open Fable.Helpers.React.Props

module R = Fable.Helpers.React
module Mui = Fable.Helpers.MaterialUI

let tabContainer children =
    Mui.typography [
        MaterialProp.Component !^"div"
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
            Index (int props.model.activeView)
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
                Elmish.React.Common.lazyView3
                    ExpansionPanelView.view
                    props.model.expandedPanel
                    props.model.timerEnabled
                    props.dispatch
            ]
        ]
    ]
let withStyles<'a> = Mui.withStyles (StyleType.Func Styles.styles) []

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
