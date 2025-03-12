
# Places in Portugal - Web Portal

## Description
This project is a web portal designed to provide information about villages and historical places in Portugal. The portal allows two types of users:

1. **Anonymous Users**: Can view information about the historical sites and register an account.
2. **Authenticated Users**: Can publish information about historical sites, comment, and rate the sites published by other users.

The portal's home page displays a list of sites published by users, and upon selecting a site, users are redirected to a form with detailed information about the location, including name, description, address (if applicable), locality, and district. A Bootstrap carousel displays images associated with the location, as well as comments and ratings. Optionally, a map centered on the site can be displayed. The home page may also showcase the top 10 highest-rated sites.

### User Capabilities:
- **Create a Site**: Authenticated users can add a site with name, description, address (if applicable), locality, district, and images (optionally with captions). They can also include the latitude and longitude for map display.
- **Edit a Site**: Users can edit all site information, including adding/removing photos and changing captions.
- **Edit Personal Data**: Registered users can update their personal information, including name and email address.

### Features:
- **Login and Registration**: Users must create an account with a username, email, and password to access the system. Upon login, the user ID is stored in the session variable for access to personal area forms.
- **Personal Area**: A personal area page displays all sites published by the logged-in user. It includes options to create or edit sites.
- **Site Forms**: The Create and Edit site forms allow users to enter and modify site information, including images and descriptions.
- **Data Management**: The project makes use of SQL Server for data storage and has a set of queries provided in text files for creating the database.

## Features Implemented:
1. **Home Page**: List of user-published sites with images, links to individual site details, and the top-rated sites.
2. **Site Details Page**: Displays information about the site, including images, comments, ratings, and an optional map.
3. **Personal Area**: Displays the user's published sites and provides the option to create or edit a site.
4. **CRUD Operations for Sites**: Users can create, update, and manage sites, including images and captions.
5. **Authentication**: Registration, login, and session management for authenticated users.
6. **Bootstrap Carousel**: Displaying images related to each site on the details page.
7. **SQL Server Database**: All site information, images, comments, and ratings are stored in an SQL Server database.

## Technologies Used:
- **C#**: Backend programming language for the application logic.
- **SQL Server**: Database management system for storing user and site data.
- **Visual Studio Code**: Integrated development environment (IDE) used for developing the project.
- **Bootstrap**: Frontend framework for styling and the carousel component.
- **HTML/CSS/JavaScript**: For frontend development and interactivity.

## Database Setup
The database structure is defined using queries, which are provided in three text files. These queries create tables, relationships, and necessary data for the application to function correctly.

## How to Run:
1. Clone this repository to your local machine.
2. Open the project in Visual Studio Code.
3. Set up the SQL Server database using the provided SQL queries.
4. Build and run the application using Visual Studio.

## Data Sources:
The information about the historical villages and sites is sourced from various online resources.
