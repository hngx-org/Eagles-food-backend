# authentication

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
  
