// Include all standalone Javascript files from the Views/Shared folder.
// This is so as to include any Component Javascript files from the EditorTemplates
// and TemplateComponents folders. This allows us to define the .js file for a component
// alongside the .cshtml file in the same file structure e.g.:
//
//  Shared
//    +- EditorTemplates
//    +- TemplateComponents
//         +- Chart.cshtml
//         +- Chart.js
//         +- Table.cshtml
//         +- Table.js

function requireAll(r) {
    r.keys().forEach(r);
} 

requireAll(
    require.context(
        "../Views/Shared", // context folder
        true,              // include subdirectories
        /.*\.js/           // RegExp
    )
);