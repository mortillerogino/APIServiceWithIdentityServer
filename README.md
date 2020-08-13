# Identity Server With API Service

*Proof of Concept for an Identity Server 4.0 STS an API.*

## **Requirements to Use**

1. Visual Studio 2019
2. SQL Server
3. Postman


## **Steps to Use**

**Getting an Access token from the Identity Server**
1. Download code and open on Visual Studio
2. Set both Server and API as startup projects
3. On postman, create a new POST request and put url: https://localhost:5001/connect/token
4. Go to Body tab, select x-www-form-urlencoded
5. Fill in keys and values below (FORMAT: key - value)
    - grant_type - password 
    - username - pedro 
    - password - Pass123$ 
    - client_id - postman 
    - client_secret - secret 
    - scope - api1
6. Click SEND and it should respond with an "access_token"

**Using the Access Token to access the API Service**

7. Copy the "access_token"
8. Create a new GET request with url: https://localhost:6001/identity
9. Go to Authorization tab
10. For type, select "Bearer token"
11. On the field provided on the right for token, paste the "access_token"
12. Click Send
13. It should respond with the claims 

**Seeded User Accounts:**

Username: *pedro*

Password: *Pass123$*

Username: *maria*

Password: *Pass123$*
