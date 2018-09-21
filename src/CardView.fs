module CardView

open Fable.Core
open Fable.Core.JsInterop
open Fable.Helpers.MaterialUI
open Fable.MaterialUI.Props
open Fable.Helpers.React.Props

module Mui = Fable.Helpers.MaterialUI
module R = Fable.Helpers.React

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

let view (props: RootProps) =
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
