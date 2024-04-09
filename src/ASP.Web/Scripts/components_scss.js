// Include all standalone Scss files from the Views/Shared folder.
// This is so as to include any Component Scss files from the EditorTemplates
// and TemplateComponents folders. This allows us to define the .js and .scss files for a component
// alongside the .cshtml file in the same file structure e.g.:
//
//  Shared
//    +- EditorTemplates
//    +- TemplateComponents
//         +- Chart.cshtml
//         +- Chart.js
//         +- Chart.scss
//         +- Table.cshtml
//         +- Table.js
//         +- Table.scss

function requireAll(r) {
    r.keys().forEach(r);
} 

requireAll(
    require.context(
        "../Views/Shared", // context folder
        true,              // include subdirectories
        /.*\.scss/         // RegExp
    )
);