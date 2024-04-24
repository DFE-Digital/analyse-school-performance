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

function requireAll(folders) {
    for (var f of folders) {
        f.keys().forEach(f);
    }
}

requireAll([
    require.context(
        "../Areas",   // context folder
        true,         // include subdirectories
        /.*\.scss/    // RegExp
    ),
    require.context(
        "../Features",   // context folder
        true,            // include subdirectories
        /.*\.scss/       // RegExp
    ),
    require.context(
        // TODO: Make this dynamic somehow
        "../../ASP.Web.Components/Components", // context folder
        true,                                  // include subdirectories
        /.*\.scss/                             // RegExp
    )
]);