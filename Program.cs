using ElevatorChallenge.src.Helpers;
using ElevatorChallenge.src.Models.Enums;
using ElevatorChallenge.src.Services;

namespace ElevatorChallenge
{
    class Program
    {
        public static void Main()
        {
            var building = new Building(floors: Constants.maxFloors, standardElevatorCount: 5, freightElevatorCount: 1, highSpeedElevatorCount: 1);
            Thread.Sleep(1000);
            building.UpdateStatus();

            while (true)
            {
                Console.WriteLine("\nWelcome to the Elevator System\n");
                Console.WriteLine("1. Passenger Menu");
                Console.WriteLine("2. Management Menu");
                Console.WriteLine("3. Exit");
                Console.Write("\nChoose an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        PassengerMenu(building);
                        break;
                    case "2":
                        ManagementMenu(building);
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }

        static void PassengerMenu(Building building)
        {
            while (true)
            {
                Console.WriteLine("\nPassenger Menu");
                Console.WriteLine("1. Call Elevator");
                Console.WriteLine("2. Return to Main Menu");
                Console.Write("\nChoose an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        HandleCallElevator(building);
                        break;
                    case "2":
                        return;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }

        static void ManagementMenu(Building building)
        {
            while (true)
            {
                Console.WriteLine("\nElevator System Management");
                Console.WriteLine("1. Refresh View");
                Console.WriteLine("2. Move Elevator");
                Console.WriteLine("3. Load People into Elevator");
                Console.WriteLine("4. Unload People from Elevator");
                Console.WriteLine("5. Load Freight into Elevator");
                Console.WriteLine("6. Unload Freight from Elevator");
                Console.WriteLine("7. Set Elevator Out of Order");
                Console.WriteLine("8. Reactivate Elevator");
                Console.WriteLine("9. Return to Main Menu");
                Console.Write("\nChoose an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        building.UpdateStatus();
                        break;
                    case "2":
                        HandleMoveElevator(building);
                        break;
                    case "3":
                        HandleLoadPeople(building);
                        break;
                    case "4":
                        HandleUnloadPeople(building);
                        break;
                    case "5":
                        HandleLoadFreight(building);
                        break;
                    case "6":
                        HandleUnloadFreight(building);
                        break;
                    case "7":
                        HandleSetElevatorOutOfOrder(building);
                        break;
                    case "8":
                        HandleReactivateElevator(building);
                        break;
                    case "9":
                        return;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }

        private static void HandleCallElevator(Building building)
        {
            int floor = ReadInteger("\nEnter your current floor: ", 1, building.MaxFloors);
            Console.WriteLine("\nPlease choose the type of elevator:");
            Console.WriteLine("1. Standard Passenger Elevator");
            Console.WriteLine("2. High-Speed Passenger Elevator");
            Console.WriteLine("3. Freight Elevator");
            Console.Write("\nEnter choice: ");

            var elevatorType = Console.ReadLine() switch
            {
                "1" => ElevatorType.Standard,
                "2" => ElevatorType.HighSpeed,
                "3" => ElevatorType.Freight,
                _ => ElevatorType.Standard
            };

            building.CallElevator(floor, elevatorType);
        }

        private static void HandleMoveElevator(Building building)
        {
            int elevatorId = ReadInteger("\nEnter Elevator ID to move: ");
            int floor = ReadInteger("Enter floor to move to: ", 1, building.MaxFloors);
            building.UpdateStatus();
            building.MoveElevator(elevatorId, floor);
        }

        private static void HandleLoadPeople(Building building)
        {
            int elevatorId = ReadInteger("\nEnter Elevator ID to load people: ");
            int people = ReadInteger("Enter number of people to load: ", 1);
            building.LoadPeopleToElevator(elevatorId, people);
            building.UpdateStatus();
        }

        private static void HandleUnloadPeople(Building building)
        {
            int elevatorId = ReadInteger("\nEnter Elevator ID to unload people: ");
            int people = ReadInteger("Enter number of people to unload: ", 1);
            building.UnloadPeopleFromElevator(elevatorId, people);
            building.UpdateStatus();
        }

        private static void HandleLoadFreight(Building building)
        {
            int elevatorId = ReadInteger("\nEnter Elevator ID to load freight: ");
            int weight = ReadInteger("Enter weight of freight to load: ", 1);
            building.LoadFreightToElevator(elevatorId, weight);
            building.UpdateStatus();
        }

        private static void HandleUnloadFreight(Building building)
        {
            int elevatorId = ReadInteger("\nEnter Elevator ID to unload freight: ");
            int weight = ReadInteger("Enter weight of freight to unload: ", 1);
            building.UnloadFreightFromElevator(elevatorId, weight);
            building.UpdateStatus();
        }

        private static void HandleSetElevatorOutOfOrder(Building building)
        {
            int elevatorId = ReadInteger("\nEnter Elevator ID to set out of order: ");
            building.SetElevatorOutOfOrder(elevatorId);
            building.UpdateStatus();
        }
        private static void HandleReactivateElevator(Building building)
        {
            int elevatorId = ReadInteger("\nEnter Elevator ID to reactivate: ");
            building.ReactivateElevator(elevatorId);
            building.UpdateStatus();
        }

        private static int ReadInteger(string prompt, int minValue = int.MinValue, int maxValue = int.MaxValue)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value) && value >= minValue && value <= maxValue)
                {
                    return value;
                }
                Console.WriteLine($"Invalid input. Please enter a number between {minValue} and {maxValue}.");
            }
        }
    }
}
