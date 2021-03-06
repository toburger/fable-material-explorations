module AppBarView

open Fable.Core.JsInterop
open Fable.Helpers.React.Props
open Fable.MaterialUI.Props

module Mui = Fable.Helpers.MaterialUI
module R = Fable.Helpers.React

let view (props: RootProps) =
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
                    props.dispatch (SetActiveView (enum idx)))
                Class !!classes?flex
                MaterialProp.Value (int props.model.activeView)
            ] [
                Mui.tab [ MaterialProp.Label (node (R.str "Table")) ]
                Mui.tab [ MaterialProp.Label (node (R.str "Card")) ]
                Mui.tab [ MaterialProp.Label (node (R.str "Expansion")) ]
            ]
            R.div [] [
                Mui.button [
                    MaterialProp.Color ComponentColor.Inherit
                ] [ R.str "Login" ]
            ]
        ]
    ]
