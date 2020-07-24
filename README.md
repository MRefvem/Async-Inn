# Async-Inn

## Author
*Michael Refvem*

## Description
The owners of “Async Inn” have approached you with plans to renovate their hotel chain. Currently they are tracking all the different locations and rooms in spreadsheets and binders. They currently have about 10 binders full of paperwork that consists of the difference between each location and the pricing for each room. The amount of time and paperwork it takes to manage the rooms and locations is costing the company both time and money. They are currently looking for a “better way” to maintain their business model.

## Architecture

This app is an API containing data about hotels stored in a Database. Originally this was accomplished by having the controllers depend on the db context but in a later version was moved over to using interfaces instead. This was done because it allows the way for information to be stored and accessed from the database to be more dynamic in later versions (having a narrower mode of access could cause problems later). Using interfaces allows our classes to follow directions rather than creating directions themselves. This makes future changes to the app easier later.

---

### Getting Started
Clone this repository to your local machine.

```
$ git clone https://github.com/MRefvem/Async-Inn.git
```

### To run the program from Visual Studio:
Select ```File``` -> ```Open``` -> ```Project/Solution```

Next navigate to the location you cloned the Repository.

Double click on the ```Lab-Hotel-Asset-Management``` directory.

Then select and open ```Async-Inn.sln```

---

### Visuals

#### Entity Relationship Diagram (ERD) (Revised - 21 Jul 2020)
![Entity Relationship Diagram](assets/ERD-Amanda.png)
#### Entity Relationship Diagram (ERD) (Original - 20 Jul 2020)
![Entity Relationship Diagram](assets/ERD.png)

### Detail of Routes



---

### Change Log 
4.0: *Built navigation properties and routes. Created new interfaces, services and tables for RoomAmenities and HotelRooms. Added the ability to add and remove amenities to a specific room. Satisfied CRUD requirements. README updated.* - 23 Jul 2020
3.1: *README updated, .gitignore added* - 22 Jul 2020
3.0: *Refactored, Hotels, Rooms and Amenities controllers to depend on an interface rather than the db context. Built an interface for each of the controllers that contain the required method signatures for all four CRUD operations to the database directly. Updated each of the controllers to inject the interface rather than the DBContext. Create a service for each of the controllers that implement the appropriate interface.* - 22 Jul 2020
2.2: *README updated, includes revised ERD* - 21 Jul 2020  
2.1: *All feature tasks complete, was able to replicate all CRUD operations in Postman* - 21 Jul 2020  
2.0: *Async Inn Management System, application specifications: Startup File, Simple Models & the Database, Seeded Data* - 21 Jul 2020  
1.2: *Added ERD to project (planning database stage)* - 20 Jul 2020  
1.1: *Initial commit* - 20 Jul 2020