module ExpansionPanelView

open Fable.Core.JsInterop
open Fable.MaterialUI.Props
open Fable.Helpers.React.Props

module Mui = Fable.Helpers.MaterialUI
module R = Fable.Helpers.React

let changeExpansion panel dispatch =
    ExpansionPanelProp.OnChange (fun _ expanded ->
        let panel = if expanded then Some panel else None
        dispatch (ChangeExpandedPanel panel))

let view expanded timerEnabled dispatch =
    R.div [ Style [ Width "100%" ] ] [
        Mui.formControlLabel [
            MaterialProp.Label (node (R.str "Enable timer"))
            FormControlLabelProp.Control
                (Mui.checkbox [
                    Checked timerEnabled
                    OnChange (fun e -> dispatch (EnableTimer (!!e.target?``checked``)))
                 ])
        ] []
        Mui.expansionPanel [
            ExpansionPanelProp.Expanded (expanded = Some ExpandedPanel.Panel1)
            (changeExpansion ExpandedPanel.Panel1 dispatch)
        ] [
            Mui.expansionPanelSummary [
                ExpansionPanelSummaryProp.ExpandIcon (node Icons.expandMoreIcon)
            ] [
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
            ExpansionPanelProp.Expanded (expanded = Some ExpandedPanel.Panel2)
            (changeExpansion ExpandedPanel.Panel2 dispatch)
        ] [
            Mui.expansionPanelSummary [
                ExpansionPanelSummaryProp.ExpandIcon (node Icons.expandMoreIcon)
            ] [
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
            ExpansionPanelProp.Expanded (expanded = Some ExpandedPanel.Panel3)
            (changeExpansion ExpandedPanel.Panel3 dispatch)
         ] [
            Mui.expansionPanelSummary [
                ExpansionPanelSummaryProp.ExpandIcon (node Icons.expandMoreIcon)
            ] [
                Mui.typography [] [
                    R.str "Expansion Panel 3"
                ]
            ]
            Mui.expansionPanelDetails [] [
                Mui.typography [] [
                    R.str """Nunc vitae orci ultricies, auctor nunc in, volutpat nisl. Integer sit amet egestas
eros, vitae egestas augue. Duis vel est augue."""
                ]
            ]
        ]
        Mui.expansionPanel [
            ExpansionPanelProp.Expanded (expanded = Some ExpandedPanel.Panel4)
            (changeExpansion ExpandedPanel.Panel4 dispatch)
         ] [
            Mui.expansionPanelSummary [
                ExpansionPanelSummaryProp.ExpandIcon (node Icons.expandMoreIcon)
            ] [
                Mui.typography [] [
                    R.str "Expansion Panel 4"
                ]
            ]
            Mui.expansionPanelDetails [] [
                Mui.typography [] [
                    R.str """Nunc vitae orci ultricies, auctor nunc in, volutpat nisl. Integer sit amet egestas
eros, vitae egestas augue. Duis vel est augue."""
                ]
            ]
        ]
    ]
