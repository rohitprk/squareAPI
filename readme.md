# Square API
API to store co-ordinate points, X and Y, find sets of points that make squares in 2D plane and also return number of squares can be drawn from provided points. JWT Bearer authentication is being used to validate API requests.
## Operations
- Register - Register new user
   ```
    POST api/v1/user/register
    {
        "Name": "Test1",
        "Password":"zIEzHB4Fq+3g0pHWVEns2A=="  // AES ECB Encrypted Password
    }
    ```
- Authenticate - Validate user and generate access token
    ```
    POST api/v1/user/authenticate
    {
        "Name": "Test1",
        "Password":"zIEzHB4Fq+3g0pHWVEns2A=="  // AES ECB Encrypted Password
    }
    ```
- Add - Add single/bulk co-ordinate points
    ```
    POST api/v1/points/add
    {
    "points": [
        {
            "x":-1,
            "y":-1
        }]
    }
    ```
- FileUpload - Upload csv file and insert points data
    ```
    POST api/v1/points/fileUpload
    Content-type:multipart/form-data
    ```
- Delete - Delete single/bulk co-ordinate points
    ```
    DELETE api/v1/points/delete
    {
    "points": [
        {
            "x":-1,
            "y":-1
        }]
    }
    ```
- Squares - Get points that form square on 2D plane and the no. of squares that can be formed from co-ordinate points.
    ```
    GET api/v1/square
    ```
---
## Packages used
 - Microsoft.Data.SqlClient - To connect with MS SQL Database
 - Dapper - Micro ORM to manage database operation
 - Swashbuckle.AspNetCore - Swagger documentation
 - MSTest - Unit test framework
 - CSVHelper - To read CSV file for bulk Upload
 - Microsoft.AspNetCore.Authentication.JwtBearer - JWT Bearer Authentication
 ---
## Prerequisites
- .NET SDK 6.0.201
- GIT
- Visual Studio Code or Visual Studio (if you want to deep dive in colorful code)
- Microsoft SQL Server
- Microsoft SQL Management Studio or any Other Sql Server Client
- Docker Desktop (To run docker images)
- Postman (kind of optional, to test WebAPI)
---
## Build solution
- Go to folder and clone this repository using below command
    ```
    git clone https://github.com/rohitprk/squareAPI.git
    ```
- Open terminal with location where git repository cloned and Run below command to restore packages
    ```
    dotnet restore
    ```
- To build project, run
    ```
    dotnet build
    ```
## Run Web API project
 - Run Sql script DBStructure.sql, provided in solution folder, in MS SQL Server 
 - Update connection string in SquareAPI.Web/appsettings.json to point your database server.
- ### Using development server
    To run project using development Server(Kestrel), navigate to SquareAPI.Web folder and open command prompt to this location.
    Run command
    ```
    dotnet run
    ```
- ### Using Docker image
    To build and run Docker image, navigate to solution directory and run command (if you want to change port open dockerfile and Update ports mentioned in Expose and ENV ASPNETCORE_URLS)
    ```
    docker build . -t squareapi
    ```
    To run Docker image, use below command
    ```
    docker run -p 5000:5000 squareapi:latest
    ```
 - After running the project, Open http://localhost:{port} to view swagger documentation
## Run Unit Test
  - To run unit tests, navigate to SquareAPI.Business.Test folder and open command prompt and run below command
    ```
    dotnet test
    ```
---