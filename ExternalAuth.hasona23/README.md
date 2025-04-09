
# Books Store Management System
Project done to learn about identity and role authorization in C# and EF with the asp.net core MVC Architecture

There are 3 roles in the Application 
1- Admin:
	- Manages Books (CRUD)
	- Manages Users(Demote and Promotes from Customer -> Staff or Staff <- Customer)
2- Staff:
	-Manages Books (CRUD)
	- Have No access to users not even List

3- Customers:
	-Only Allowed to See Books
	- Role of Least CONTROL
	- **NO ACCESS TO USERS data like ADMIN**
	-**NO BOOK MANAGEMENT  like ADMIN/STAFF

## How to Run

use 
```
git clone https://github.com/hasona23/ProductManagement/hasona23.git
```
or use the ssh version

then go to Directory of .sln file and type in cmd

```
dotnet build
dotnet run
```
the project file is Product Management.hasona23

for Loggin in with pre-made accounts
1- Admin

```
email: admin@example.com
password : Admin123!
```
2- Staff 

```
email : staff{any number from 1 to 10}@example.com 
ex: staff7@example.com
password: Staff123!
```
3- Customer

```
email : customer{any number from 1 to 20}@example.com 
ex: customer12@example.com
password: Customer123!
```

- For External Login services aren't configured (Removed keys and secrets) but the functions itself are setup so please add them if intend to use
## Lessons Learned

What did you learn while building this project? What challenges did you face and how did you overcome them?

1- I need to try give more time for frontend insteas of the plain layout made by bootstrap
2- Usage of asp-route attribute 
3- Roles and Authorization
4- Using SMTP for mail services

Thank you.