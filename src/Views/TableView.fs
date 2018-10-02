module TableView

open Fable.MaterialUI.Props
open Fable.Import.React
open Fable.Core.JsInterop
open Fable.Helpers.React.Props

module R = Fable.Helpers.React
module Mui = Fable.Helpers.MaterialUI

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

let view classes allFoodsSelected selectedFoods foods dispatch =
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
            (if not (List.isEmpty foods) then
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
                                MaterialProp.Component !^"th"
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
