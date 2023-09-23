# Authentication

## Users

<summary>
Users are regular users of the app, they cannot transfer, redeem, etc. unless they're in an org.
</summary>

<details>

- **POST** `/api/auth/user/signup`, make a user

  takes:

  ```json
  {
    "lastName": "john",
    "firstName": "doe",
    "email": "john@doe.com",
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
      "Id": "8",
      "OrgId": "",
      "FirstName": "doe",
      "LastName": "john",
      "ProfilePic": "",
      "Email": "john@doe.com",
      "Phone": "123456",
      "IsAdmin": "False",
      "LunchCreditBalance": "",
      "RefreshToken": "",
      "BankNumber": "",
      "BankCode": "",
      "BankName": "",
      "BankRegion": "",
      "Currency": "",
      "CurrencyCode": "",
      "CreatedAt": "22/09/2023 12:38:46",
      "UpdatedAt": "22/09/2023 12:38:46",
      "access_token": "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiOCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6InVzZXIiLCJleHAiOjE2OTU0NzI3NTF9.JTdKZ1nfuA2SI0oQnCe2y5D5eTeTDIWsaCZdZk-Rh0qVdnc7jNs8r7NnN7q_l58Z_jsSeboUBEUogaTO-e2LnQ",
      "Org": ""
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
      "email": "not-an-email"
    }
  }
  ```

- **POST** `/api/auth/login`, login as a user

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
    "success": true,
    "data": {
      "Id": "8",
      "OrgId": "",
      "FirstName": "doe",
      "LastName": "john",
      "ProfilePic": "",
      "Email": "john@doe.com",
      "Phone": "123456",
      "IsAdmin": "False",
      "LunchCreditBalance": "",
      "RefreshToken": "",
      "BankNumber": "",
      "BankCode": "",
      "BankName": "",
      "BankRegion": "",
      "Currency": "",
      "CurrencyCode": "",
      "CreatedAt": "22/09/2023 12:38:46",
      "UpdatedAt": "22/09/2023 12:38:46",
      "access_token": "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiOCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6InVzZXIiLCJleHAiOjE2OTU0NzI3NTF9.JTdKZ1nfuA2SI0oQnCe2y5D5eTeTDIWsaCZdZk-Rh0qVdnc7jNs8r7NnN7q_l58Z_jsSeboUBEUogaTO-e2LnQ",
      "organization_name": "x hng's Organization"
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

  > [!WARNING] TODO: updating user info

</details>

## Organizations

<summary>
Organizations are like banks, they hold users and allow them to transfer and redeem lunches
</summary>

<details>

- **POST** `/api/organization/staff/signup`, make an admin of an org.

  this makes a new org. called "name's org." and sets them as an admin over it.

  takes:

  ```json
  {
    "lastName": "john",
    "firstName": "doe",
    "email": "john@doe.com",
    "password": "pass",
    "phone": "123456"
  }
  ```

  if successful, returns:

  ```json
  {
    "message": "Staff signed up successfully",
    "statusCode": 201,
    "success": true,
    "message": "User authenticated successfully",
    "statusCode": 200,
    "data": {
      "Id": "8",
      "OrgId": "1",
      "FirstName": "doe",
      "LastName": "john",
      "ProfilePic": "",
      "Email": "john@doe.com",
      "Phone": "123456",
      "IsAdmin": "False",
      "LunchCreditBalance": "",
      "RefreshToken": "",
      "BankNumber": "",
      "BankCode": "",
      "BankName": "",
      "BankRegion": "",
      "Currency": "",
      "CurrencyCode": "",
      "CreatedAt": "22/09/2023 12:38:46",
      "UpdatedAt": "22/09/2023 12:38:46",
      "access_token": "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiOCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6InVzZXIiLCJleHAiOjE2OTU0NzI3NTF9.JTdKZ1nfuA2SI0oQnCe2y5D5eTeTDIWsaCZdZk-Rh0qVdnc7jNs8r7NnN7q_l58Z_jsSeboUBEUogaTO-e2LnQ",
      "Org": "doe john's org."
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
      "email": "not-an-email"
    }
  }
  ```

- **PUT** `/api/organization/modify`, modifies an organisation

  after a staff (admin) is signed up, they can modify their org. with this if they want to change the name, currency, etc.

  takes:

  ```json
  {
    "organisationName": "Big Bank PLC",
    "lunchPrice": "200",
    "currency": "$"
  }
  ```

  if successful (claims match, user is an admin), returns:

  ```json
  {
    "message": "Organisation modified successfully",
    "statusCode": 200,
    "success": true,
    "data": {
      "orgId": "4",
      "name": "Big Bank PLC",
      "lunchPrice": "200",
      "currency": "$"
    }
  }
  ```

  if invalid (e.g. due to invalid permissions), returns:

  ```json
  {
    "message": "User unauthorised",
    "statusCode": 401,
    "success": false,
    "data": {
      "id": "2"
    }
  }
  ```

- **PATCH** `api/organization/lunch/update`, updates the lunch price of an org.

  takes:

  ```json
  {
    "lunchPrice": "200"
  }
  ```

  if successful (claims match, user is an admin), returns:

  ```json
  {
    "message": "Organisation lunch price updated successfully",
    "statusCode": 200,
    "success": true,
    "data": null
  }
  ```

  if invalid (e.g. due to invalid permissions), returns:

  ```json
  {
    "message": "User unauthorised",
    "statusCode": 401,
    "success": false,
    "data": {
      "id": "2"
    }
  }
  ```

- **PATCH** `api/organization/wallet`, updates the wallet of an org.

  takes:

  ```json
  {
    "amount": "200"
  }
  ```

  if successful (claims match, user is an admin), returns:

  ```json
  {
    "message": "Organisation wallet updated successfully",
    "statusCode": 200,
    "success": true,
    "data": null
  }
  ```

  if invalid (e.g. due to invalid permissions), returns:

  ```json
  {
    "message": "User unauthorised",
    "statusCode": 401,
    "success": false,
    "data": {
      "id": "2"
    }
  }
  ```

- **POST** `api/organization/invite`, adds a user to an org.

  ```json
  {
    "message": "User unauthorised",
    "statusCode": 401,
    "success": false,
    "data": {
      "id": "2"
    }
  }
  ```

- **PATCH** `api/organizations/wallet`, updates the wallet of an org.

  takes:

  ```json
  {
    "amount": "200"
  }
  ```

  if successful (claims match, user is an admin), returns:

  ```json
  {
    "message": "Wallet updated successfully",
    "statusCode": 200,
    "success": true,
    "data": null
  }
  ```

  if invalid (e.g. due to invalid permissions), returns:

  ```json
  {
    "message": "User unauthorised",
    "statusCode": 401,
    "success": false,
    "data": {
      "id": "2"
    }
  }
  ```

- **POST** `api/organizations/invite`, invites a user to an org.

  takes:

  ```json
  {
    "email": "john@doe.com"
  }
  ```

  if successful (claims match, user is an admin, email not invited before), returns:

  ```json
  {
    "message": "User Added To Organisation successfully",
    "statusCode": 200,
    "success": true,
    "data": null
  }
  ```

  if invalid (e.g. due to invalid permissions, email already invited), returns:

  ```json
  {
    "message": "User unauthorised",
    "statusCode": 401,
    "success": false,
    "data": {
      "id": "2"
    }
  }
  ```

</details>

## End-point: /api/user/profile

### Method: GET

### Headers

| Content-Type  | Value          |
| ------------- | -------------- |
| Authorization | Bearer <token> |

### Response

```json
{
  "data": {
    "user_id": "2",
    "name": "John Doe",
    "email": "john@gmail.com",
    "phone_number": "9800",
    "profile_picture": null,
    "isAdmin": false,
    "organization": "x hng's Organization"
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

| Content-Type  | Value          |
| ------------- | -------------- |
| Authorization | Bearer <token> |

### Body (**raw**)

```json
{
  "bank_region": "Kenya",
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

## End-point: /api/user/update

### Method: PUT

### Headers

| Content-Type  | Value          |
| ------------- | -------------- |
| Authorization | Bearer <token> |

### Body (**raw**)

```json
{
  "email": "johndoe@gmail.com",
  "lastname": "Doe",
  "firstname": "John",
  "phone": "0803327019",
  "profilePic": "happy picture"
}
```

### Response

```json
{
  "data": {
    "email": "johndoe@gmail.com",
    "name": "John Doe",
    "phone": "0803327019",
    "profile_picture": "happy picture"
  },
  "message": "User Profile updated successfully",
  "success": true,
  "statusCode": 200
}
```

⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃ ⁃

## End-point: /api/user/all

### Method: GET

### Headers

| Content-Type  | Value          |
| ------------- | -------------- |
| Authorization | Bearer <token> |

### Response

```json
{
  "data": [
    {
      "name": "John Doe",
      "email": "doe@gmail.com",
      "profile_picture": null,
      "user_id": "1",
      "role": "User"
    },
    {
      "name": "John Doe",
      "email": "john@gmail.com",
      "profile_picture": null,
      "user_id": "2",
      "role": "Admin"
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

| Content-Type  | Value          |
| ------------- | -------------- |
| Authorization | Bearer <token> |

### Method: GET

### Response

```json
{
  "data": {
    "name": "John Doe",
    "email": "john@gmail.com",
    "profile_picture": null,
    "user_id": "2",
    "role": "Admin"
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

| Content-Type  | Value          |
| ------------- | -------------- |
| Authorization | Bearer <token> |

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

| Content-Type  | Value          |
| ------------- | -------------- |
| Authorization | Bearer <token> |

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

| Content-Type  | Value          |
| ------------- | -------------- |
| Authorization | Bearer <token> |

Retrieves a single lunch request by its ID.

**Request parameters:**

- `id`: The ID of the lunch request to retrieve.

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
