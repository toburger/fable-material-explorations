[<AutoOpen>]
module Utils

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import.React
open Fable.Helpers.React.Props
open Fable.Helpers.MaterialUI

module R = Fable.Helpers.React

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

let node x =
    ReactNode.Case1 (ReactChild.Case1 x)

let AriaLabel (content: string) = HTMLAttr.Custom ("aria-label", content)
let AriaExpanded (expanded: bool) = HTMLAttr.Custom ("aria-expanded", expanded)

module Icons =
    let inline private icon path =
        let icon = importDefault<Fable.Import.React.ComponentClass<IHTMLProp>> path
        materialEl icon [] []

    let favoriteIcon = icon "@material-ui/icons/Favorite"
    let shareIcon = icon "@material-ui/icons/Share"
    let expandMoreIcon = icon "@material-ui/icons/ExpandMore"
    let moreVertIcon = icon "@material-ui/icons/MoreVert"
