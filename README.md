# Dotes
This project allows you to create document templates using a web interface and then generate docx files from the template using the API. The template is a docx file with content control tags.

Dotes contains 3 things:
* Web interface for creating templates and then storing them in a SQLite database.
* API for generating documents based on [TemplateEngine.Docx](https://github.com/UNIT6-open/TemplateEngine.Docx).
* Angular example showing how to use the API to generate a document.

## Web Interface
The main page contains a list of templates stored in the SQLite database.

![Templates list page](https://github.com/alsetor/dotes/blob/main/templates-page.png)

To create a template, you need to upload a docx file with the content control tags, and then describe this tags. There are 3 types of tags: **String**, **Image**, **Table**. The template has a Uid that you should use in the API methods.

![Template page](https://github.com/alsetor/dotes/blob/main/template-page.png)

## API
API has 3 methods:
* GenerateDocument
* GetTemplateByUid
* GetTagsByTemplateUid

![Swagger API](https://github.com/alsetor/dotes/blob/main/swagger-page.png)

## Angular Example
Click the "Create Document" button next to a template on the template list page to navigate to the example page, which contains a dynamic form for generating a document.

![Dynamic form](https://github.com/alsetor/dotes/blob/main/dynamic-form-example.png)

Fill out the form and click the submit button to generate the document. The generated docx document will be downloaded.

![Word example](https://github.com/alsetor/dotes/blob/main/word-example.png)

[DotesService](https://github.com/alsetor/dotes/blob/main/Dotes.Web/ClientApp/src/app/modules/templates/services/dotes.service.ts) provides methods for using the API, described below.
[CreateDocumentByTemplateComponent](https://github.com/alsetor/dotes/blob/main/Dotes.Web/ClientApp/src/app/modules/templates/components/create-document/create-document.component.ts) uses the _getTagsByTemplateUid_ API method to create a dynamic form that the user should fill out to generate a document from a template. Submitting the form calls the _generateDocument_ API method to download the docx document with filled tags.
