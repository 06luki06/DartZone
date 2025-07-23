# DartZone

## Routes

### /api/auth Routes

| Method | Route          | Requires Token | Description                           | Request Body |
|--------|----------------|----------------|---------------------------------------|--------------|
| POST   | /auth/register | ❌ No         | Registers user, returns JWT Token     | ✅ `AuthDto` |
| POST   | /auth/login    | ❌ No         | Logs in and returns a JWT token       | ✅ `AuthDto` |
| GET    | /auth/me       | ✅ Yes        | Returns info about the logged-in user | ❌ –         |
| DELETE | /auth/delete   | ✅ Yes        | Deletes the currently logged-in user  | ❌ –         |

### /api/games Routes

| Method  | Route         | Requires Token | Description                          | Request Body  |
|---------|---------------|----------------|--------------------------------------|---------------|
| POST    | /games        | ✅ Yes        | Starts a new 301 game                | ❌ –          |
| GET     | /games        | ✅ Yes        | Gets a list of your games            | ❌ –          |
| GET     | /games/{id}   | ✅ Yes        | Gets details of a specific game      | ❌ –          |
| DELETE  | /games/{id}   | ✅ Yes        | Deletes/Aborts a game                | ❌ –          |
| POST    | /games/{id}/throws | ✅ Yes   | Adds a dart throw to the game        | ✅ `ThrowDto` |
| GET     | /games/{id}/throws | ✅ Yes   | Returns all throws of a game         | ❌ –          |
