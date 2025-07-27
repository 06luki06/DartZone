# DartZone

[![Build](https://github.com/06luki06/DartZone/actions/workflows/pipeline.yml/badge.svg)](https://github.com/06luki06/DartZone/actions/workflows/pipeline.yml)
[![Code Coverage](https://codecov.io/gh/06luki06/DartZone/graph/badge.svg?token=REFR0T6JKW)](https://codecov.io/gh/06luki06/DartZone)

## Routes

`Response Body` marks the return type, if the request is successful.

### /api/auth Routes

| Method | Route          | Requires Token | Description                           | Request Body        | Response Body         |
|--------|----------------|----------------|---------------------------------------|---------------------|-----------------------|
| POST   | /auth/register | ❌ No         | Registers user, returns JWT Token     | ✅ `UserRequestDto` | ✅ `TokenResponseDto` |
| POST   | /auth/login    | ❌ No         | Logs in and returns a JWT token       | ✅ `UserRequestDto` | ✅ `TokenResponseDto` |
| GET    | /auth/me       | ✅ Yes        | Returns info about the logged-in user | ❌ –                | ✅ `UserResponseDto`  |
| DELETE | /auth/delete   | ✅ Yes        | Deletes the currently logged-in user  | ❌ –                | ❌ –                  |

### /api/games Routes

| Method  | Route         | Requires Token | Description                          | Request Body  | Response Body              |
|---------|---------------|----------------|--------------------------------------|---------------|----------------------------|
| POST    | /games        | ✅ Yes        | Starts a new 301 game                | ❌ –          | ✅ `GameResponseDto`       |
| GET     | /games        | ✅ Yes        | Gets a list of your games            | ❌ –          | ✅ `List<GameResponseDto>` |
| GET     | /games/{id}   | ✅ Yes        | Gets details of a specific game      | ❌ –          | ✅ `GameResponseDto`       |

*not implemented*

| Method | Route          | Requires Token | Description                           | Request Body |
|--------|----------------|----------------|---------------------------------------|--------------|
| DELETE  | /games/{id}   | ✅ Yes        | Deletes/Aborts a game                | ❌ –          |
| POST    | /games/{id}/throws | ✅ Yes   | Adds a dart throw to the game        | ✅ `ThrowDto` |
| GET     | /games/{id}/throws | ✅ Yes   | Returns all throws of a game         | ❌ –          |
