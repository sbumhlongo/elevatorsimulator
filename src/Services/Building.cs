using ElevatorChallenge.src.Helpers;
using ElevatorChallenge.src.Models;
using ElevatorChallenge.src.Models.Enums;

namespace ElevatorChallenge.src.Services
{
    public class Building
    {
        public int MaxFloors { get; set; }
        public List<ElevatorContoller> Elevators { get; set; }
        public Dictionary<int, int> WaitingPeople { get; set; }
        public Dictionary<int, int>? Weight { get; set; }

        private readonly Random _random = new();

        public Building(int floors, int standardElevatorCount, int highSpeedElevatorCount, int freightElevatorCount)
        {
            MaxFloors = floors;
            Elevators = [];
            WaitingPeople = [];
            InitializeWaitingPeople();
            HashSet<int> occupiedFloors = [];

            for (int i = 0; i < standardElevatorCount; i++)
            {
                int startingFloor;
                do
                {
                    startingFloor = _random.Next(2, MaxFloors + 1);  //Randomly place standard elevators on differernt floors for simulation purposes
                }
                while (occupiedFloors.Contains(startingFloor));

                Elevators.Add(new StandardElevator(i + 1, Constants.stdMaxCapacity, MaxFloors, ElevatorType.Standard, this)
                {
                    CurrentFloor = startingFloor,
                    ElevatorState = State.Stationary
                });
                occupiedFloors.Add(startingFloor);
            }
            //Prioritize passenger elevator, added two elevator types as below:
            for (int i = 0; i < highSpeedElevatorCount; i++)
            {
                int startingFloor;
                do
                {
                    startingFloor = _random.Next(1, MaxFloors + 1);
                }
                while (occupiedFloors.Contains(startingFloor));

                Elevators.Add(new HighSpeedElevator(6, Constants.hsMaxCapacity, MaxFloors, ElevatorType.HighSpeed, this) // Can add mroe types of elevstors if neeed - or increase number of high speed or frieght.
                {
                    CurrentFloor = startingFloor,
                    ElevatorState = State.Stationary
                });
                occupiedFloors.Add(startingFloor);
            }
            for (int i = 0; i < freightElevatorCount; i++)
            {
                int startingFloor;
                do
                {
                    startingFloor = _random.Next(1, MaxFloors + 1);
                }
                while (occupiedFloors.Contains(startingFloor));

                Elevators.Add(new FreightElevator(7, Constants.maxWeightlimit, MaxFloors, ElevatorType.Freight, this)
                {
                    CurrentFloor = startingFloor,
                    ElevatorState = State.Stationary
                });
                occupiedFloors.Add(startingFloor);
            }

        }
        //Initialize people waiting on randoms floors
        private void InitializeWaitingPeople()
        {
            for (int i = 1; i <= MaxFloors; i++)
            {
                int peopleOnFloor = _random.Next(0, 20);
                WaitingPeople[i] = peopleOnFloor;
            }
        }
        //Constantnt update status pf elevators
        public virtual void UpdateStatus()
        {
            Console.Clear();
            Console.WriteLine("Elevator Status:\n");

            foreach (var elevator in Elevators)
            {
                elevator.DisplayStatus();
            }

            Console.WriteLine("\nWaiting People at Each Floor:");
            Console.WriteLine("--------------------------------------------------------------------------");
            string floorRow = "| Floors: ";
            string peopleRow = "| People: ";

            for (int i = 1; i <= MaxFloors; i++)
            {
                floorRow += $"{i,-4}";
                peopleRow += $"{WaitingPeople[i],-4}";
            }

            Console.WriteLine(floorRow);
            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine(peopleRow);

            Console.WriteLine("--------------------------------------------------------------------------");
            Console.WriteLine();
        }
        public void MoveElevator(int elevatorId, int floor)
        {
            var elevator = Elevators.FirstOrDefault(e => e.Id == elevatorId);
            if (elevator != null)
            {
                elevator.MoveToFloor(floor);
            }
            else
            {
                Console.WriteLine($"Error: Elevator with ID {elevatorId} does not exist.");
            }
        }
        public void LoadPeopleToElevator(int elevatorId, int people)
        {

            var elevator = Elevators.FirstOrDefault(e => e.Id == elevatorId);
            int peopleToLoad = 0;
            int weight = 0;
            if (elevator != null)
            {
                if (elevator.Type == ElevatorType.Standard)
                {
                    peopleToLoad += Math.Min(people, Constants.stdMaxCapacity - elevator.CurrentLoad);
                }
                else if (elevator.Type == ElevatorType.HighSpeed)
                {
                    peopleToLoad += Math.Min(people, Constants.hsMaxCapacity - elevator.CurrentLoad);

                }
                else
                {
                    weight += Math.Min(people, Constants.maxWeightlimit - elevator.CurrentLoad);
                }
                if (elevator.Type == ElevatorType.Standard || elevator.Type == ElevatorType.HighSpeed)
                {
                    if (peopleToLoad > 0)
                    {
                        elevator.LoadPeople(peopleToLoad);
                        int currentFloor = elevator.CurrentFloor;

                        if (WaitingPeople.TryGetValue(currentFloor, out int value) && value >= peopleToLoad)
                        {
                            WaitingPeople[currentFloor] -= peopleToLoad;
                            Console.WriteLine($"{peopleToLoad} people loaded from Floor {currentFloor}. {WaitingPeople[currentFloor]} remaining waiting.\n");

                        }
                        else
                        {
                            Console.WriteLine($"Not enough people waiting on Floor {currentFloor}. Attempted to load {peopleToLoad}. Remaining waiting: {value}");
                            Thread.Sleep(1500);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"There are currently no passengers waiting on this floor.\n");
                    }
                }

                if (elevator.Type == ElevatorType.Freight)
                {
                    if (weight > 0)
                    {
                        elevator.LoadFreight(weight);
                        int currentFloor = elevator.CurrentFloor;

                        if (Weight != null && (!Weight.TryGetValue(currentFloor, out int value) || value < weight))
                        {
                            Weight[currentFloor] -= weight;
                            Console.WriteLine($"{weight} freight {currentFloor}.\n");
                        }
                        else
                        {
                            Console.WriteLine($"Not freight on Floor {currentFloor}.");
                            UpdateStatus();
                            Thread.Sleep(1000);
                        }
                    }
                }

            }
            else
            {
                Console.WriteLine($"Error: Elevator with ID {elevatorId} does not exist.");
            }
        }

        public void UnloadPeopleFromElevator(int elevatorId, int people)
        {
            var elevator = Elevators.FirstOrDefault(e => e.Id == elevatorId);
            if (elevator != null)
            {
                elevator.UnloadPeople(people);
                int currentFloor = elevator.CurrentFloor;
                Console.WriteLine($"{people} people unloaded at Floor {currentFloor}.");
                elevator.Direction = Direction.Idle;
                elevator.ElevatorState = State.Stationary;
                Thread.Sleep(1000);
                UpdateStatus();
            }
            else
            {
                Console.WriteLine($"Error: Elevator with ID {elevatorId} does not exist.");
            }
        }

        public void LoadFreightToElevator(int elevatorId, int weight)
        {
            var elevator = Elevators.FirstOrDefault(e => e.Id == elevatorId);
            if (elevator != null)
            {
                elevator.LoadFreight(weight);
            }
            else
            {
                Console.WriteLine($"Error: Elevator with ID {elevatorId} does not exist.");
            }
        }
        public void SetElevatorOutOfOrder(int elevatorId)
        {
            var elevator = Elevators.FirstOrDefault(e => e.Id == elevatorId);
            if (elevator != null)
            {
                elevator.SetOutOfOrder();
                Console.WriteLine($"Elevator {elevatorId} has been set out of order.");
                UpdateStatus();
            }
            else
            {
                Console.WriteLine($"Error: Elevator {elevatorId} not found.");

            }
        }
        public void ReactivateElevator(int elevatorId)
        {
            var elevator = Elevators.FirstOrDefault(e => e.Id == elevatorId);
            if (elevator != null)
            {
                elevator.Reactivate();
                Console.WriteLine($"Elevator {elevatorId} is no operationalr.");
                UpdateStatus();
            }
            else
            {
                Console.WriteLine($"Error: Elevator {elevatorId} not found.");

            }
        }
        public void UnloadFreightFromElevator(int elevatorId, int weight)
        {
            var elevator = Elevators.FirstOrDefault(e => e.Id == elevatorId);
            if (elevator != null)
            {
                elevator.UnloadFreight(weight);

            }
            else
            {
                Console.WriteLine($"Error: Elevator with ID {elevatorId} does not exist.");
            }
        }
        public void CallElevator(int floor, ElevatorType elevatorType)
        {
            UpdateStatus();
            var nearestElevator = FindNearestElevator(floor, elevatorType);
            if (WaitingPeople.TryGetValue(floor, out int peopleWaiting) && peopleWaiting > 0)
            {
                if (nearestElevator != null)
                {
                    Console.WriteLine($"\nElevator {nearestElevator.Id} of type {elevatorType} has been dispatched to floor {floor}.");
                    nearestElevator.MoveToFloor(floor);
                    Thread.Sleep(1000);
                    UpdateStatus();
                    while (nearestElevator.CurrentFloor != floor)
                    {
                        Thread.Sleep(500);
                    }
                    int destinationFloor;
                    while (true)
                    {
                        Console.WriteLine($"\nPlease enter destination floor (1 to {MaxFloors}):");

                        if (int.TryParse(Console.ReadLine(), out destinationFloor) && destinationFloor >= 1 && destinationFloor <= MaxFloors)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid floor. Please enter a floor number between 1 and " + MaxFloors);
                        }
                    }
                    nearestElevator.MoveToFloor(destinationFloor);

                    while (nearestElevator.CurrentFloor != destinationFloor)
                    {
                        Thread.Sleep(1500);
                    }
                }

            }
            else
            {
                Console.WriteLine($"\nNo people are waiting on floor {floor}.");
                Thread.Sleep(1000);
            }
        }

        public ElevatorContoller? FindNearestElevator(int floor, ElevatorType elevatorType)
        {
            Console.WriteLine($"\nSearching for an elevator of type {elevatorType} near floor {floor}...");

            var availableElevators = Elevators
                .Where(e => e.Type == elevatorType && e.ElevatorState == State.Stationary)
                .OrderBy(e => Math.Abs(e.CurrentFloor - floor))
                .ToList();

            if (availableElevators.Count == 0)
            {
                Console.WriteLine("No matching elevator found.");
                return null;
            }

            return availableElevators.First();
        }

    }
}