# OZone

#This is set up commit

## Install EF commands in dotnet
dotnet tool install --global dotnet-ef

dotnet ef database update

## Add migration
dotnet ef migrations add InitialCreate

### Create Event
http://localhost:5160/Events
#### Request json
{
    "name": "First Event",
    "date": "2023-09-16T06:35:25.471Z",
    "mode": 0,
    "modelDetails": "joining info",
    "topic": "Demo topic",
    "speakers": "ozone",
    "details": "This is a demo topic, anything will be discussed here",
    "personOfContact": "kishan.vaishnav@thoughtworks.com",
    "rules": "Everyone must come in party wear",
    "deadline": "2023-09-18T06:35:25.471Z",
    "community": "fun@thoguhtworks.com",
    "capacity": 123,
    "type": 0,
    "tags": "fun, devs"
}


# Data Setup
## Events
[
{
"name": "EF core hands-on",
"date": "2023-09-20T06:35:25.471",
"mode": 0,
"modelDetails": "joining info",
"topic": "Introduction, Setup, Code First Approach, Migrations, Lazy Loading, Advance Features",
"speakers": "Raju, Srini",
"details": "Working with database is always hard, in this event we will go through all aspects of DB connectivity using EF Core",
"personOfContact": "shevala.raju@thoughtworks.com",
"rules": "Dotnet 7 and MSSQL should be installed",
"deadline": "2023-09-18T06:35:25.471",
"community": "ozone@thoughtworks.com",
"capacity": 100,
"type": 1,
"tags": "dotnet, devs, csharp",
},
{
"name": "Dotnet Api design",
"date": "2023-09-20T06:35:25.471",
"mode": 0,
"modelDetails": "joining info",
"topic": "Introduction, Setup, Hands-on, Advance Features",
"speakers": "Raju, Venu",
"details": "This event will give basics of API in dotnet",
"personOfContact": "shevala.raju@thoughtworks.com",
"rules": "Dotnet 7 must be installed in the laptop",
"deadline": "2023-09-18T06:35:25.471",
"community": "ozone@thoughtworks.com",
"capacity": 123,
"type": 0,
"tags": "dotnet, devs, api"
},
{
"name": "Basics of data engineering",
"date": "2023-09-21T06:35:25.471",
"mode": 0,
"modelDetails": "joining info",
"topic": "Demo topic",
"speakers": "Srini",
"details": "Interactive discussion on how data engineering can be useful",
"personOfContact": "pro.kishan16@gmail.com",
"rules": "No rules",
"deadline": "2023-09-18T06:35:25.471",
"community": "ozone@thoughtworks.com",
"capacity": 123,
"type": 0,
"tags": "fun, devs, data",
"subscriptions": []
},
{
"name": "Multi-threading in programming language",
"date": "2023-09-21T06:35:25.471",
"mode": 0,
"modelDetails": "joining info",
"topic": "Demo topic",
"speakers": "Thejaswini, Venu",
"details": "This event will discuss about how multi-threading can improve performance",
"personOfContact": "kishan.vaishnav@thoughtworks.com",
"rules": "Everyone must carry laptop",
"deadline": "2023-09-18T06:35:25.471",
"community": "ozone@thoughtworks.com",
"capacity": 123,
"type": 0,
"tags": "fun, devs"
},
{
"name": "Implementing e2e Testing in a Web App",
"date": "2023-09-21T06:35:25.471",
"mode": 0,
"modelDetails": "joining info",
"topic": "Demo topic",
"speakers": "Srini",
"details": "Learning how to test web app",
"personOfContact": "srini@thoughtworks.com",
"rules": "NA",
"deadline": "2023-09-18T06:35:25.471",
"community": "ozone@thoughtworks.com",
"capacity": 123,
"type": 0,
"tags": "qa, devs, ba"
},
{
"name": "Creating UML diagrams",
"date": "2023-09-21T06:35:25.471",
"mode": 0,
"modelDetails": "joining info",
"topic": "Demo topic",
"speakers": "Jyothi",
"details": "This is a demo topic, anything will be discussed here",
"personOfContact": "jyothi@thoughtworks.com",
"rules": "Everyone must come in party wear",
"deadline": "2023-09-18T06:35:25.471",
"community": "qa",
"capacity": 123,
"type": 1,
"tags": "qa, devs, ba, fun"
}
]