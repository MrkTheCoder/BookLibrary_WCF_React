![LibraryBook Architecture - Backend](https://user-images.githubusercontent.com/25789969/160426164-aeccd559-1d1d-44a0-b8a0-dcc99e96040f.png)

# Book Library Backend

The main topics are:

 - How to run it
 - APIs
 - Folders
 - Layers

## How to run it
There are two ways that you can have backend and run it for yourself:
 - You can always download the latest binary files from the [released channel](https://github.com/MrkTheCoder/BookLibrary_WCF_React/releases). 
 - Of course, you can download the source code and compile it yourself in Visual Studio 2019+. You can eighter get it from the released channel or from the "[master](https://github.com/MrkTheCoder/BookLibrary_WCF_React/tree/master/BookLibrary_Backend)" branch.

## APIs

More info in detail you can find at here "[Business.BookLibrary.Business.Services.Managers](https://github.com/MrkTheCoder/BookLibrary_WCF_React/tree/master/BookLibrary_Backend/Business/BookLibrary.Business.Services/Managers)". But in brief, we have these APIs (Latest update 04/19/2022):
| API | Description |
|--|--|
| http://.../api/BookManager/books | Return the first 10 books on page 1. (Default behavior) |
| http://.../api/BookManager/books?page=p&item=i&category=c | Return 'i' books at c category on page p. |
| http://.../api/BorrowerManager/borrowers | Return the first 10 borrowers on page 1. (Default behavior) |
| http://.../api/BorrowerManager/borrowers?page=p&item=i | Return 'i' borrowers at c category on page p. |
| http://.../api/CategoryManager/categories | Return all categories. |

**Notes:**
 - All APIs with parameters can use mix of parameters.
 - Item: 'Items per Page' valid values 10, 20, 30, 40, 50 items.

## Folders
Brief description of each folder:
  - **Business**: business layer. IoC Bootstrapper, Services, Contracts, Entities.
  - **Client**: *~~Not used yet~~*! Reserved for Desktop Client.
  - **Core**: Common data. Contracts, Helpers, Exceptions.
  - **Data**: Data Access Layer. DbContext, Repositories, Data Seeds.
  - **Host**: WCF Hosts. Windows Console Local Host. Windows Console Host (Access to Azure ServiceBus). 
  - **Tests**: Integration and Unit Tests.

## Layers
We have these layers
  - Database:
    - SQLite: database file created in the root of the app. (LocalDb.sqlite)
  - Data Access:
    - EF Core / DbContext
    - Repositories
    - Contracts
    - Dtos
  - Business: 
    - Bootstrapper
    - WCF Contracts
    - WCF Services
    - Entities
  - Core: Common things!
  - Host: WCF self host:
    - Console Hosts:
      - Local network host.
      - Azure host.
  - Tests:
    - (Client) Console Tests
    - Integrated Tests
    - Unit Tests
