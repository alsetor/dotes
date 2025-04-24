# Dotes - Document Template System

Dotes is a full-stack application for creating and managing DOCX document templates enriched with dynamic tags. It provides a user-friendly interface and a powerful API for generating DOCX files using predefined templates and tag values.

---

##  Features

-  Upload and manage .docx templates with dynamic tag support
-  Generate documents dynamically by replacing tags with values
-  Download generated documents and original templates
-  RESTful API for integration into other systems
-  Angular frontend app to display how it works
-  Docker support for easy deployment


##  Technologies

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- Angular
- MSSQL
- [TemplateEngine.Docx](https://github.com/UNIT6-open/TemplateEngine.Docx)
- Docker for containerization


##  Project Structure

Dotes
- Web -> *ASP.NET Core Web API project with Angular client*
- Domain -> *Entities*
- Application -> *Interfaces and app logic*
- Infrastructure -> *Implements interfaces from the Application layer, dealing with data access and template engine*
- README.md
- Dockerfile

---

##  Running the App

###  Locally (Development mode)

```bash
cd Web
dotnet run --launch-profile "Web"
```
Project runs at: https://localhost:5001, and Swagger UI is available on https://localhost:5001/swagger

###  With Docker
```bash
cd Dotes
docker build -t dotes .
docker run -d -p 8112:80 --name dotes dotes
```

Visit the app at: http://localhost:8112. Note: HTTPS not enabled in Docker by default.

---

##  API Overview
Launch your browser at
https://localhost:5001/swagger

![Swagger API](https://github.com/alsetor/dotes/blob/main/swagger-page.png)

## Web Interface
The main page contains a list of templates stored in the MSSQL database.

![Templates list page](https://github.com/alsetor/dotes/blob/main/templates-page.png)

To create a template, you need to upload a docx file with the content control tags, and then describe this tags. There are 3 types of tags: **String**, **Image**, and **Table**.

![Template page](https://github.com/alsetor/dotes/blob/main/template-page.png)

## Angular Example
Click the "Create Document" button next to a template on the template list page to navigate to the example page, which contains a dynamic form for generating a document.

![Dynamic form](https://github.com/alsetor/dotes/blob/main/dynamic-form-example.png)

Fill out the form and click the submit button to generate the document. The generated docx document will be downloaded.

![Word example](https://github.com/alsetor/dotes/blob/main/word-example.png)

---

Created by Aleksandr Toroshchin, 2022-2025
