The API is an Azure Function App hosted at the following locations:

**Dev:** https://s192d01-func-dev-01.azurewebsites.net
**Test:** https://s192t01-func-test-01.azurewebsites.net

To access and test the endpoints you can use an API tool such as [Postman](https://www.postman.com/), or you can view the API endpoints and try them out from within the Swagger OpenAPI documentation:

![image.png](/docs/.attachments/image-1e0f7d08-9892-4985-bba1-ed86e7afeabc.png)

The Swagger UI can be reached here:

**Dev:** https://s192d01-func-dev-01.azurewebsites.net/api/swagger/ui
**Test:** https://s192t01-func-test-01.azurewebsites.net/api/swagger/ui

In order to test the API endpoints from within the Swagger UI, you'll need to set up the page to authorize requests to the API using the API function key. For security reasons this will have to be done every time the page is loaded or refreshed.

To locate the function key, log into Azure Portal and locate the function app for either dev or test - `s192d01-func-dev-01` or `s192t01-func-test-01` (make sure it matches the URL at the top of the page), and navigate to **Functions > App keys > Host keys** and copy the `default` key:

![image.png](/docs/.attachments/image-e7d7db32-d1da-477b-b4d4-f989b1d4e75a.png)

To add this to the Swagger authorization, click on the `Authorize` button to open the Available Authorizations popup:

![image.png](/docs/.attachments/image-c75252bd-5a8b-4036-bdcc-8610627c5f14.png)

Then paste the key you just copied into the textbox and click `Authorize` - this will remember the value for every request until the page is reloaded:

![image.png](/docs/.attachments/image-6f0b6ce0-a9fb-4762-b9a0-99cd4a6321d3.png)

Now to try out an API endpoint, click on one of the endpoints to expand it, this will show a description and the parameters available - click on the `Try it out` button:

![image.png](/docs/.attachments/image-d25d0810-7603-45c4-b504-8c09d5bcb2c9.png)

Once appropriate values have been entered for the parameters, click `Execute` and the response status code and response body will be displayed in the `Responses` section:

![image.png](/docs/.attachments/image-54edc469-b847-4bb8-a996-c2bc8c6c27ab.png)