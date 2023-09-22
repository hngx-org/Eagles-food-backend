# Authentication

- POST: `/api/auth/user/signup`

  takes:

  ```json
  {
    "lastName": "john",
    "firstName": "doe",
    "email": "john@doe.com",
    "address": "123 Main St.",
    "password": "pass",
    "phone": "123456"
  }
  ```

  if successful, returns:

  ```json
  {
    "message": "User signed up successfully",
    "statusCode": 201,
    "success": true,
    "data": {
        "email": "john@doe.com",
        "id": "1",
    }
  }
  ```

  if invalid (e.g. due to invalid email, email not unique), returns:

  ```json
  {
    "message": "Invalid email",
    "statusCode": 400,
    "success": false,
    "data": {
        "email": "not-an-email",
    }
  }
  ```

- POST `/api/auth/login`

  takes:
  ```json
  {
    "email": "john@doe.com",
    "password": "pass"
  }
  ```
  
  if successful, returns:

  ```json
  {
  "message": "User authenticated successfully",
  "statusCode": 200,
  "data": {
    "access_token": "your-auth-token-here",
	"email": "john@doe.com",
    "id": "1",
	"isAdmin": false
    }
  }
  ```

  if invalid (e.g. invalid email, wrong password), returns:
  
  ```json
  {
    "message": "Incorrect password",
    "statusCode": 401,
    "data": {
      "email": "john@doe.com"
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

# Lunch

### Endpoints

**POST** `/api/lunch/send`

### Headers

|Content-Type|Value|
|---|---|
|Authorization|Bearer <token>|

Creates a new lunch request.

**Request body:**

```json
{
  "receivers": [1, 2, 3],
  "quantity": 5,
  "note": "This is a note for the lunch request."
}
```

**Response:**

```json
{
  "message": "Lunch request created successfully",
  "data": null,
  "statusCode": 201,
  "success": true
}
```
⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

**GET** `/api/lunch/all`

### Headers

|Content-Type|Value|
|---|---|
|Authorization|Bearer <token>|

Retrieves all lunch requests for the given user.

**Response:**

```json
{
  "message": "Success",
  "data": [
    {
      "Id": 1,
      "ReceiverName": "John Doe",
      "SenderName": "Jane Doe",
      "SenderId": 2,
      "ReceiverId": 1,
      "CreatedAt": "2023-09-21T23:05:03.000Z",
      "Note": "This is a note for the lunch request.",
      "Quantity": 5,
      "Redeemed": false
    },
    {
      "Id": 2,
      "ReceiverName": "Jane Doe",
      "SenderName": "John Doe",
      "SenderId": 1,
      "ReceiverId": 2,
      "CreatedAt": "2023-09-21T23:05:03.000Z",
      "Note": "This is another note for the lunch request.",
      "Quantity": 10,
      "Redeemed": true
    }
  ],
  "statusCode": 200,
  "success": true
}
```
⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

**GET** `/api/lunch/{id}`

### Headers

|Content-Type|Value|
|---|---|
|Authorization|Bearer <token>|

Retrieves a single lunch request by its ID.

**Request parameters:**

* `id`: The ID of the lunch request to retrieve.

**Response:**

```json
{
  "message": "Success",
  "data": {
    "Id": 1,
    "ReceiverName": "John Doe",
    "SenderName": "Jane Doe",
    "SenderId": 2,
    "ReceiverId": 1,
    "CreatedAt": "2023-09-21T23:05:03.000Z",
    "Note": "This is a note for the lunch request.",
    "Quantity": 5,
    "Redeemed": false
  },
  "statusCode": 200,
  "success": true
}
```
⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃