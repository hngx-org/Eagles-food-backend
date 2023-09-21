# Authentication

- POST `/api/auth/login` 

  takes:

  ```json
  {
    "email": "user@example.com",
    "password": "password123"
  }
  ```

  returns:

  if valid:

  ```json
  {
    "message": "User authenticated successfully",
    "statusCode": 200,
    "data": {
        "access_token": "your-auth-token-here",
        "email": "email@mail.com",
        "id": "random_id",
        "isAdmin": true | false
    }
  }
  ```
  
  else:
  
  ```json
  {
    "message": "User failed to authenticate",
    "statusCode": 400,
    "errors": {
        "something went wrong"
    }
  }
  ```

## End-point: /api/user/profile
### Method: GET
### Headers

|Content-Type|Value|
|---|---|
|Authorization|Bearer <token>|

### Response
```json
{
    "data": {
        "user_id": "2",
        "name": "John Doe",
        "email": "john@gmail.com",
        "phone_number": "9800",
        "profile_picture": null,
        "isAdmin": false
    },
    "message": "User data fetched successfully",
    "success": true,
    "statusCode": 200
}
```
⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: /api/user/bank
### Method: PUT
### Headers

|Content-Type|Value|
|---|---|
|Authorization|Bearer <token>|

### Body (**raw**)

```json
{
	"bank_region":"Kenya",
	"bank_number": "1234-5678-9012-3456",
    "bank_code": "123456",
    "bank_name": "Bank Name"
}
```
### Response
```json
{
    "data": null,
    "message": "Successfully created bank account",
    "success": true,
    "statusCode": 200
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: /api/user/all
### Method: GET
### Headers

|Content-Type|Value|
|---|---|
|Authorization|Bearer <token>|

### Response
```json
{
    "data": [
        {
            "name": "John Doe",
            "email": "doe@gmail.com",
            "profile_picture": null,
            "user_id": "1"
        },
        {
            "name": "John Doe",
            "email": "john@gmail.com",
            "profile_picture": null,
            "user_id": "2"
        }
    ],
    "message": "Users fetched successfully",
    "success": true,
    "statusCode": 200
}
```
⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: /api/user/search/{email}
### Headers

|Content-Type|Value|
|---|---|
|Authorization|Bearer <token>|
### Method: GET

### Response
```json
{
    "data": {
        "name": "John Doe",
        "email": "john@gmail.com",
        "profile_picture": null,
        "user_id": "2"
    },
    "message": "User found",
    "success": true,
    "statusCode": 200
}
```
⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃
