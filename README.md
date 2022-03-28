# Library Book
<img src="https://camo.githubusercontent.com/e8e7b06ecf583bc040eb60e44eb5b8e0ecc5421320a92929ce21522dbc34c891/68747470733a2f2f6d656469612e67697068792e636f6d2f6d656469612f6876524a434c467a6361737252346961377a2f67697068792e676966" width="32" height="32">Hiii!, Thanks for checking our repo. This app is a simple **Library Book** borrowing management project. It only has two main tables:
 - Books
 - Borrowers

The final app will let borrowers see the list of available books in the library and borrow some of them. This project is for learning purposes so none of these books, borrowers, or even the Library itself are real! You are very welcome to join our learning journey on this project and join our team. We are looking forward to making new friends!
<br />

## Project Goals
Goals of this project:
 - Practice teamwork and project management based on Scrum Framework.
 - Learn and practice various skills and tools.
<br />

## Project Description:
We are using server-client architecture for this project. For now! In the frontend, We are using React and Redux.  We are mainly using ReactBootstrap to style our project but some custom CSS is also being used. In the backend, we are using the WCF framework to create RESTful API by creating a self-hosted service (On Windows or Azure ServiceBus)
<br />
<br />

✨**Frontend:**

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;![Visual Studio Code](https://img.shields.io/badge/Visual%20Studio%20Code-0078d7.svg?style=for-the-badge&logo=visual-studio-code&logoColor=white) ![JavaScript](https://img.shields.io/badge/javascript-%23323330.svg?style=for-the-badge&logo=javascript&logoColor=%23F7DF1E) ![Bootstrap](https://img.shields.io/badge/bootstrap-%23563D7C.svg?style=for-the-badge&logo=bootstrap&logoColor=white) ![React](https://img.shields.io/badge/react-%2320232a.svg?style=for-the-badge&logo=react&logoColor=%2361DAFB) ![Redux](https://img.shields.io/badge/redux-%23593d88.svg?style=for-the-badge&logo=redux&logoColor=white) ![Firefox](https://img.shields.io/badge/Firefox-FF7139?style=for-the-badge&logo=Firefox-Browser&logoColor=white) ![Windows](https://img.shields.io/badge/Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white) 

We are using React v17.0.2, react-router-dom v6.2.1 and Axios v0.26.1. We make async API calls using Axios and Thunk middleware. (more technical details are in the [frontend folder](https://github.com/MrkTheCoder/BookLibrary_WCF_React/tree/master/BookLibrary_Frontend))
<br />
<br />

👻**Backend:**

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;![Visual Studio](https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?style=for-the-badge&logo=visual-studio&logoColor=white) ![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white) ![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white) ![MicrosoftSQLServer](https://img.shields.io/badge/Microsoft%20SQL%20Sever-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white) ![SQLite](https://img.shields.io/badge/sqlite-%2307405e.svg?style=for-the-badge&logo=sqlite&logoColor=white) ![Azure](https://img.shields.io/badge/azure-%230072C6.svg?style=for-the-badge&logo=microsoftazure&logoColor=white) ![Windows](https://img.shields.io/badge/Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white) 

We are using: C# as the main programming language. Since we are using WCF, our .Net Framework is set on v4.8 but for Entity Framework, we used EF Core Sqlite. For creating entities we did a Database First Approach design & Try to use and practice on various Design Patterns and DI, IoC, SOLID and etc. (more technical details are in the [backend folder](https://github.com/MrkTheCoder/BookLibrary_WCF_React/tree/master/BookLibrary_Backend)) 
<br />
<br />

⚡ **Common Tools and Skills:**

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;![Jira](https://img.shields.io/badge/jira-%230A0FFF.svg?style=for-the-badge&logo=jira&logoColor=white) ![Git](https://img.shields.io/badge/git-%23F05033.svg?style=for-the-badge&logo=git&logoColor=white) ![GitHub](https://img.shields.io/badge/github-%23121011.svg?style=for-the-badge&logo=github&logoColor=white)
<br />
<br />

🌐 **Other Resources:**

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;![Google](https://img.shields.io/badge/google-4285F4?style=for-the-badge&logo=google&logoColor=white) ![Stack Overflow](https://img.shields.io/badge/-Stackoverflow-FE7A16?style=for-the-badge&logo=stack-overflow&logoColor=white) ![Reddit](https://img.shields.io/badge/Reddit-%23FF4500.svg?style=for-the-badge&logo=Reddit&logoColor=white) ![Udemy](https://img.shields.io/badge/Udemy-A435F0?style=for-the-badge&logo=Udemy&logoColor=white) ![Pluralsight](https://img.shields.io/badge/Pluralsight-EE3057?style=for-the-badge&logo=pluralsight&logoColor=white)
<br />

## User Story:
The "Book Library" project is created based on an imaginary user story! Jane wants a website to manage her Library with these features:
<details>
<summary>"Click to see Jane features request"</summary>
 
- Show the list of all library books to site visitors.
- The list of books should have these items in each row:
  - Thumbnail of the book cover at the left and 2 text lines on its right side.
  - Book title in bold font.
  - ISBN code in normal font under it.
  - An icon to indicate if this book is available for borrowing or not.
  - When a user clicks on any book, a new page should open with the big image of the book cover along with other above items and the book description.
- Visitors can search between books based on ISBN/Book titles/Availabilities.
- Visitors can create an account for themselves or sign in.
  - Require user information at the signup process:
    - First/Middle*/Last name.
    - Phone Number.
    - Email address.
  - User after login:
    - Registring a request for borrowing some books and set a date to pick them up. (need approval from the manager)
    - When the user is online, Show a notification about stat changing of her/his borrowing request(s).
    - See the list of previous and currently borrowed books.
    - Notify the user about reaching the due date of the borrowed book(s).
    - Able to set a request to be notified when some books become available for borrowing.
    - It would be nice to send some of these notifications as an email to the user. (and not only show them popup notifications while they are online)
- Manager can have its own account and be abale to:
  - ... (to be continued)
*: optional.
 
</details>

<br />

## Project Starters:

The project started @ 22/02/2022 by these members:
1. [Thermite10k](https://github.com/Thermite10k) : Frontend Developer.
   - Skills & Tools: JavaScript, JSX, HTML, React, VSCode, Git
2. [MrkTheCoder](https://github.com/MrkTheCoder): Backend Developer.  <a href="https://www.linkedin.com/in/mrkthecoder/"><img src="https://raw.githubusercontent.com/peterthehan/peterthehan/master/assets/linkedin.svg" width="24"></a>
   - Skills & Tools: C# .Net v4.8, WCF, SQLite, EF Core, Visual Studio, Git
<br />

 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; ![visitors](https://visitor-badge.glitch.me/badge?page_id=MrkTheCoder.BookLibrary_WCF_React&left_color=gray&right_color=blue)
