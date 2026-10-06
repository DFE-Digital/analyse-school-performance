[**Note:** This functionality is not yet implemented in ASP 2.0]

To display a DataSet Report for an Establishment in the Web Application UI a number of things need to happen:

1. **Data applicability**
  For this Establishment type, what DataSets are applicable? (DataSet definitions)
2. **Data availability**
  Out of those applicable DataSets, what is the latest data that has been released for this Establishment?
3. **DataSet selection**
  Out of those DataSets which should we default to?
4. **Report selection**
  From the reports defined on the DataSet select the first one (the summary report)
5. **Data query**
  From the report definition, query the relevant DataSet data
7. **Page template query**
  From the report definition, retrieve the page content template to populate
6. **Data processing**
  From the report definition, create a data processing pipeline and feed the data into it
8. **Page template population**
  Populate the page template with the output from the data processing pipeline

   ================================================================ 

9. **Page template rendering**
  Render the components defined in the page template UI
  
Of all the steps above only the last one (page template rendering) has any specific dependency on Web technologies. In a hypothetical future we could imagine substituting the last step with a native mobile app, and the rest of the business logic would remain exactly the same. For this reason steps 1-8 are deemed part of the core application logic and should be accessible through an API.

Although an API is not directly necessary for the functioning of the application as laid out above, it does give a number of advantages:
1. **Separation of concerns**
  Having a strong boundary around the core application logic forces us to identify what are core application concerns and what are specific to the rendering client. This helps with code clarity and structure
2. **Testability**
  Exposing the core application logic through an API allows more possibilities for testing, such as data testing and automated acceptance tests
3. **Flexibility**
  Having a solid suite of tests around the core application boundary creates confidence by being able to quickly spot and fix regressions, and allows us more freedom to refactor the core application logic to progressively improve design and maintainability of the code
3. **Extensibility**
  It allows us the possibility of developing alternative rendering clients such as mobile apps without having to extract deeply coupled application logic from the Web application

Here is a diagram of the above process:

![Page template rendering.png](/docs/.attachments/Page%20template%20rendering-6f49a33f-9bd0-4c55-8f63-fe0a5a65c0d3.png)

## Data processing
ASP has some complex components which require some pre-processing of the raw data into a form that can be used to populate the page template. Also the data structures and formulas used to present them can change from year to year, so the more flexibility we have to make those changes without having to make any changes to code, the better.

The page template engine is a service that is shared between ASP and other projects such as CSCP. As such it only has the ability to feed a pure data object into a page template and populate its components with data. For example this page template (note the `~#` and `#~` to indicate the "holes" for data to go into):
```
{
  "components": [
    {
      "componentId": "Heading1",
      "content": "~#name#~'s Data"
    },
    {
      "componentId": "Paragraph",
      "content": "~#name#~ is ~#age#~ years old and his favourite fruit is ~#favouriteFruit#~."
    }
  ]
}
```
when fed with this data object:
```
{
  "name": "Fred",
  "age": 12,
  "favouriteFruit": "oranges"
}
```
will produce this populated template:
```
{
  "components": [
    {
      "componentId": "Heading1",
      "content": "Fred's Data"
    },
    {
      "componentId": "Paragraph",
      "content": "Fred is 12 years old and his favourite fruit is oranges."
    }
  ]
}
```
which will ultimately be converted into HTML by the Web UI:
```
<h1>Fred's Data</h1>
<p>Fred is 12 years old and his favourite fruit is oranges.</p>
```
Obviously the components can be much more complex than this, being charts or interactive tables

![Data population.png](/docs/.attachments/Data%20population-731ff1c0-c17f-46fc-a6cf-c348ffcb1888.png)
![Data processing.png](/docs/.attachments/Data%20processing-d78e5f92-74a4-41c0-be33-dcf35dcd9634.png)
