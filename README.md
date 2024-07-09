# User Management App
  A user management web app that use <b>ASP.NET</b> for its
  <i>back-end</i>, <b>Angular</b> and <b>Bootstrap</b> for its
  <i>front-end</i>, <b>Postgresql</b> for its <i>database</i>. Users
  must login as an employee or admin. An <i>Admin</i> can
  create,update and delete users. <i>Employee</i> can only navigate the website.

# Running the App with Docker Compose
  1. Install `Docker Desktop for Windows`.
  2. Clone the repository.
    ```
     git clone https://github.com/kevin-balauro-git/user-management.git 
    ```
  3. Navigate to the `user-management` folder in a console window.
  4. Run the following commands at the root of the folder.
     
     ```
      docker compose build
     ```
     ```
      docker compose run
     ```
  6. Open your browser and navigate to `http://localhost:4200`
