using ElevatorChallenge.src.Models.Enums;

namespace ElevatorChallenge.src.Services
{
    public abstract class ElevatorContoller(int id, int maxCapacity, int maxFloors, ElevatorType Type, Building building)
    {
        public int Id { get; private set; } = id;
        public int MaxCapacity { get; set; } = maxCapacity;
        public int CurrentFloor { get; set; } = 1;
        public Direction Direction { get; set; } = Direction.Idle;
        public int CurrentLoad { get; set; } = 0;
        public int MaxFloors { get; set; } = maxFloors;
        public ElevatorType Type { get; set; } = Type;
        public State ElevatorState { get; set; } = State.Stationary;

        private readonly Building _building = building;

        public abstract void DisplayStatus();
        public virtual void MoveToFloor(int floor, Action<int>? onElevatorMove = null)
        {
            if (ElevatorState == State.OutOfOrder)
            {
                Console.WriteLine($"Error: Elevator {Id} is out of order and cannot move.\n");
                return;
            }

            if (floor < 1 || floor > MaxFloors)
            {
                Console.WriteLine($"Error: Floor {floor} is outside the valid range (1 to {MaxFloors}).\n");
                return;
            }


            if (CurrentFloor == floor)
            {

                Direction = Direction.Idle;
                Console.WriteLine($"\nElevator {Id} is already on floor {CurrentFloor}. Status is now Ready.\n");
                if (Type == ElevatorType.Standard || Type == ElevatorType.HighSpeed)
                {
                    if (CurrentLoad < 12)
                    {
                        if (!_building.WaitingPeople.TryGetValue(floor, out var waitingPeople) || waitingPeople > 0)
                        {
                            Console.WriteLine($"\nThere are currently {waitingPeople} waiting on floor {floor}.");
                            Thread.Sleep(5000);
                            _building.LoadPeopleToElevator(Id, waitingPeople);
                        }

                    }
                    else
                    {
                        if (!_building.WaitingPeople.TryGetValue(floor, out var CurrentLoad) || CurrentLoad > 0)
                        {
                            Console.WriteLine($"\nThere are currently {CurrentLoad} waiting on floor {floor}.");
                            Thread.Sleep(5000);
                            _building.LoadPeopleToElevator(Id, CurrentLoad);
                        }

                    }
                    return;
                }

            }

            Direction = floor > CurrentFloor ? Direction.Up : Direction.Down;
            Console.WriteLine($"\nElevator {Id} is moving {Direction} to floor {floor}...\n");
            ElevatorState = State.Moving;
            onElevatorMove?.Invoke(Id);

            while (CurrentFloor != floor)
            {
                int sleepTime = Type switch
                {
                    ElevatorType.HighSpeed => 1000,
                    ElevatorType.Freight => 1500,
                    _ => 1300
                };

                Thread.Sleep(sleepTime);
                CurrentFloor += Direction == Direction.Up ? 1 : -1;
                Console.WriteLine($"\nElevator {Id} is now on floor {CurrentFloor}.\n");
                Thread.Sleep(sleepTime);
                _building.UpdateStatus();
            }

            if (Type == ElevatorType.Freight)
            {
                if (CurrentLoad < 1)
                {
                    Console.Write("\nFreight elevator detected. \nPlease enter the weight of the cargo to be loaded (nearest whole number): ");
                    if (int.TryParse(Console.ReadLine(), out int weight) && weight > 0)
                    {
                        _building.LoadFreightToElevator(Id, weight);
                    }
                }
                else
                {
                    Thread.Sleep(5000);
                    _building.UnloadFreightFromElevator(Id, CurrentLoad);
                }

            }
            if (Type == ElevatorType.Standard || Type == ElevatorType.HighSpeed)
            {
                if (CurrentLoad < 1)
                {
                    if (!_building.WaitingPeople.TryGetValue(floor, out var waitingPeople) || waitingPeople > 0)
                    {
                        Console.WriteLine($"\nThere are currently {waitingPeople} waiting on floor {floor}.");
                        Thread.Sleep(5000);
                        _building.LoadPeopleToElevator(Id, waitingPeople);
                    }

                }
                else
                {
                    Thread.Sleep(5000);
                    _building.UnloadPeopleFromElevator(Id, CurrentLoad);
                }
                return;
            }
        }
        public void LoadPeople(int people)
        {
            if (ElevatorState == State.OutOfOrder)
            {
                Console.WriteLine($"Error: Elevator {Id} cannot load people as it is out of order.\n");
                return;
            }

            if (CurrentLoad + people > MaxCapacity)
            {
                Console.WriteLine($"Error: Elevator {Id} cannot load {people} people. Max capacity is {MaxCapacity}.\n");
            }
            else
            {
                CurrentLoad += people;
                string personOrPeople = people == 1 ? "person" : "people";
                Console.WriteLine($"\nElevator {Id} loaded {people} {personOrPeople}. Current load: {CurrentLoad}/{MaxCapacity}.\n");
                Thread.Sleep(5000);

            }
        }
        public void UnloadPeople(int people)
        {
            if (ElevatorState == State.OutOfOrder)
            {
                Console.WriteLine($"Error: Elevator {Id} cannot unload people as it is out of order.\n");
                return;
            }

            else
            {
                CurrentLoad -= people;
                Thread.Sleep(5000);
            }
        }
        public abstract void LoadFreight(int weight);
        public abstract void UnloadFreight(int weight);
        public void SetOutOfOrder()
        {
            ElevatorState = State.OutOfOrder;
            Direction = Direction.Idle;
            Console.WriteLine($"Elevator {Id} is out of order.\n");
        }

        public void Reactivate()
        {
            ElevatorState = State.Stationary;
            Console.WriteLine($"Elevator {Id} is now operational.\n");
        }
    }
}
