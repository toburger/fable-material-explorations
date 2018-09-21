module ExpansionPanelView

open Fable.Helpers.React.Props
open Fable.MaterialUI.Props

module Mui = Fable.Helpers.MaterialUI
module R = Fable.Helpers.React

let changeExpansion panel dispatch =
    ExpansionPanelProp.OnChange (fun _ expanded ->
        if expanded then
            dispatch (ChangeExpandedPanel panel))

let view expanded dispatch =
    R.div [ Style [ Width "100%" ] ] [
        Mui.expansionPanel [
            ExpansionPanelProp.Expanded (expanded = ExpandedPanel.Panel1)
            (changeExpansion ExpandedPanel.Panel1 dispatch)
        ] [
            Mui.expansionPanelSummary [] [
                Mui.typography [] [
                    R.str "Expansion Panel 1"
                ]
            ]
            Mui.expansionPanelDetails [] [
                Mui.typography [] [
                    R.str """Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse malesuada lacus ex,
sit amet blandit leo lobortis eget."""
                ]
            ]
        ]
        Mui.expansionPanel [
            ExpansionPanelProp.Expanded (expanded = ExpandedPanel.Panel2)
            (changeExpansion ExpandedPanel.Panel2 dispatch)
        ] [
            Mui.expansionPanelSummary [] [
                Mui.typography [] [
                    R.str "Expansion Panel 2"
                ]
            ]
            Mui.expansionPanelDetails [] [
                Mui.typography [] [
                    R.str """Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse malesuada lacus ex,
sit amet blandit leo lobortis eget."""
                ]
            ]
        ]
        Mui.expansionPanel [
            ExpansionPanelProp.Expanded (expanded = ExpandedPanel.Panel3)
            (changeExpansion ExpandedPanel.Panel3 dispatch)
            HTMLAttr.Disabled true
         ] [
            Mui.expansionPanelSummary [] [
                Mui.typography [] [
                    R.str "Disabled Panel 3"
                ]
            ]
            Mui.expansionPanelDetails [] [
                Mui.typography [] [
                    R.str """"""
                ]
            ]
        ]
    ]
