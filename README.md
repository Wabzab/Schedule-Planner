# Schedule-Planner
WPF application for managing self-study hours for modules in a semester.

## Requirements
  - .NET Framework v4.7.2
  - Windows OS
### Soft Requirement
  - Visual Studio 2022

## Compiling
Using an IDE, such as Visual Studio, makes this process simple.
Visual Studio has a shortcut to build the solution: "CTRL + Shift + B".
To start debugging with Visual Studio press the shortcut key: "F5".

## Usage
This application uses a SQL datase to persist data between application runtimes and to manage individual accounts.

This application has not been exported into an exectable, as such it requires any user to compile the codebase in their own environment. Upon doing so you will be met with a welcome page where you may proceed to input your username and password if you already have an account on the application. If this is your first time launching the app, you will need to create an account by clicking the 'Sign up' button. This will take you to another page where you may input the username and password for your profile. Clicking 'Create profile' will take you back to the welcome page where you may now sign in with your freshly created account. If this is your first time logging in to your account, you will need to input date information for your semester before moving on to the module managing aspect of the app. After doing so you will be taken to the module managing page.

In this page you may add new modules by selecting the 'Add module' button, which will take you to a page for inputting information on the module. There is also a button to remove a module which works by removing whichever module you have selected in the list of modules on the left. Finally, there is a button to add hours studied for a module on a specific day. At times it may seem that adding hours does not seem to do anything, this is because the hours studied will only affect the displayed amount if the studying happens in the current week.

All pages that take you away from the main page (module manager or login) have a 'Cancel' button that will take you back to the previous page without saving any of the inputs made.

## Contact
Email me queries: patersonjones.max@gmail.com<br>
Find me on GitHub: https://github.com/Wabzab

