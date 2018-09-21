module Styles

open Fable
open Fable.Core
open Fable.Core.JsInterop
open Fable.Helpers.React.Props
open Fable.Helpers.MaterialUI

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
