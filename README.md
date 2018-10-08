# Schibsted .net backend test

This is a MVC WEB API 2 application with Home page accessible to registered users and three private pages accessible per role. User and role administration is made through the API using Basic authentication.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

To get started in the Team Explorer, Manage Connections, clone a Git repository 
https://github.com/magdalenakovalikova/SchibstedNetBackendTest.git to local Git repository. Recursively.
Open Solution SchibstedBackendTest.sln

Assuming that LocalDB is installed, to recreate the database file follow these steps:
1. In the Solution Explorer click on Show All Files icon right click an existing .mdf file and then delete it.
2. In the Package Manager Console window set as Default project SchibstedBackendTest.EFModels and run command
```
update-database
```
Then run the application with Ctrl-F5.

To access to pages Home, Page 1, Page 2, Page 3 log in as admin, user1, user2 or user3 username respectively.
Use the "secret" test password is 12345678. :)
```
 - Home (Users list): Is accessible to all registered users -> admin, user1, user2, user3
 - Page 1: To access this page, the logged user needs to have the role PAGE_1 -> user1
 - Page 2: To access this page, the logged user needs to have the role PAGE_2 -> user2
 - Page 3: To access this page, the logged user needs to have the role PAGE_3 -> user3
```
### Prerequisites

What things you need to install the software and how to install them

```
Visual Studio (it's tested on VS2017 Professional)
```

### Installing


## Running the tests
In te Test Explorer window click on Run All.

## Deployment


## Built With


## Contributing


## Versioning


## Authors
Magdaléna Kovalíková
## License


## Acknowledgments

* Hat tip to anyone whose code was used
* Inspiration
* etc
