# Elevator Challenge - Console Application

## Overview
This is a console-based Elevator Management System that simulates the operation of elevators in a building. It includes functionalities for passengers to call elevators and for management to control them.

## Prerequisites
- .NET SDK 8.0 or later installed on your system
- A terminal or command prompt

## Setup Instructions

### Clone the Repository
If you haven't already cloned the repository, run the following command:
```sh
git clone https://github.com/your-repository/ElevatorChallenge.git
```

### Navigate to the Project Directory
```sh
cd ElevatorChallenge
```

### Build the Project
```sh
dotnet build
```

### Run the Application

dotnet run

## How to Use

### Main Menu
When the application starts, you will see the main menu with the following options:
1. **Passenger Menu** - Allows users to call an elevator to floor (closes available elevator selected).
2. **Management Menu** - Allows administrators to manage elevators (move, set to out of order, etc).
3. **Exit** - Closes the application.

### Passenger Menu
1. **Call Elevator** - Enter your current floor and select the type of elevator (Standard, High-Speed, or Freight).
2. **Return to Main Menu** - Go back to the main menu.

### Management Menu
1. **Refresh View** - Updates the elevator status.
2. **Move Elevator** - Enter the elevator ID and destination floor.
3. **Load People into Elevator** - Enter the elevator ID and the number of people to load.
4. **Unload People from Elevator** - Enter the elevator ID and the number of people to unload.
5. **Load Freight into Elevator** - Enter the elevator ID and the weight of the freight.
6. **Unload Freight from Elevator** - Enter the elevator ID and the weight of the freight.
7. **Set Elevator Out of Order** - Enter the elevator ID to mark it as out of service.
8. **Return to Main Menu** - Go back to the main menu.

## Running Tests

You can run tests using:

dotnet test


## Troubleshooting
- If you encounter errors, ensure you have the required .NET SDK installed.
- If elevators do not respond, check if they are set as "Out of Order" in the management menu.
- If inputs are invalid, follow the prompts carefully and enter numeric values where required.

## Contributions
Feel free to submit pull requests to improve the application.



