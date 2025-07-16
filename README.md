# DartZone

## Routes

Base: **/api**

| Method | Route          | Requires Token | Description                           | Request Body |
|--------|----------------|----------------|---------------------------------------|--------------|
| POST   | /auth/register | ❌ No         | Registers user, returns JWT Token     | ✅ `AuthDto` |
| POST   | /auth/login    | ❌ No         | Logs in and returns a JWT token       | ✅ `AuthDto` |
| GET    | /auth/me       | ✅ Yes        | Returns info about the logged-in user | ❌ –         |
| DELETE | /auth/delete   | ✅ Yes        | Deletes the currently logged-in user  | ❌ –         |
