# Project Deployment Guide

## Prerequisites

* **Docker** ([https://docs.docker.com/desktop/install/windows-install/](https://docs.docker.com/desktop/install/windows-install/))
* **Docker Compose** ([https://docs.docker.com/compose/install/](https://docs.docker.com/compose/install/))
* **.NET SDK** ([https://dotnet.microsoft.com/en-us/download](https://dotnet.microsoft.com/en-us/download))
* **Entity Framework Core Tools** ([https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)) 

## Deploying with Docker


1. **Clone the Repository**
    ````
    git clone https://github.com/FoxiDen/PublicHolidaysApi.git
    cd <repository-directory>
    ````
2. **Build and start docker container**

*   ***Initial Setup***

    ````
    docker-compose up --build
    ````
*   ***Subsequent deployments***

    ````
    docker-compose up
    ````

## Web Api Details

### Live Web API

Hosted on: [https://publicholidaysapi-b2dkevacbydfbqdq.northeurope-01.azurewebsites.net/swagger/index.html](https://publicholidaysapi-b2dkevacbydfbqdq.northeurope-01.azurewebsites.net/swagger/index.html)

### Local Web API

**Exposed Port:** The application is running on port 8080 inside the Docker container.  
**Access Swagger UI:** To view the Swagger API documentation, navigate to http://localhost:8080/swagger/index.html.

### Available endpoints

* /api/SupportedCountries     
* /api/DayStatus/{countryCode}/{date}     
* /api/Holidays/{countryCode}/{year}          
* /api/MaximumConsecutiveFreeDays/{countryCode}/{year}


## Managing the MSSQL Database

The database is hosted on MS Azure. If you'd like to reset it to clear state, run these commands for migrations from the repository-directory/PublicHolidaysApi folder:

    dotnet ef database update 0
    dotnet ef database update

**Connecting to the SQL server:**

* Server name: publichloidaysdb.database.windows.net  
* Authentication: SQL Server Authentication  
* Login: civitta  
* Password: Password1!
