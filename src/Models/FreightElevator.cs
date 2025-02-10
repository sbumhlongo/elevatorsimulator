using ElevatorChallenge.src.Models.Enums;
using ElevatorChallenge.src.Services;

namespace ElevatorChallenge.src.Models
{
    public class FreightElevator(int id, int maxCapacity, int maxFloors, ElevatorType type, Building building) : ElevatorContoller(id, maxCapacity, maxFloors, type, building)
    {
        public override void DisplayStatus()
        {
            Console.WriteLine(
                $"{Id,-3} | Freight   | {ElevatorState,-10} | Floor {CurrentFloor,-3} | {Direction,-10} | Load: {CurrentLoad}kg/{MaxCapacity}kg");
        }

        public override void LoadFreight(int weight)
        {
            if (weight <= 0)
            {
                Console.WriteLine($"Error: Cannot load {weight}kg. Weight must be positive.");
                return;
            }

            if (CurrentLoad + weight > MaxCapacity)
            {
                Console.WriteLine($"Error: Elevator {Id} cannot load {weight}kg. Exceeds max capacity of {MaxCapacity}kg.");
                return;
            }

            CurrentLoad += weight;
            Console.WriteLine($"Elevator {Id} loaded {weight}kg. Current load: {CurrentLoad}kg/{MaxCapacity}kg.");
        }

        public override void UnloadFreight(int weight)
        {
            if (weight <= 0)
            {
                Console.WriteLine($"Error: Cannot unload {weight}kg. Weight must be positive.");
                return;
            }

            if (weight > CurrentLoad)
            {
                Console.WriteLine($"Error: Elevator {Id} cannot unload {weight}kg. Only {CurrentLoad}kg available.");
                return;
            }

            CurrentLoad -= weight;
            Console.WriteLine($"Elevator {Id} unloaded {weight}kg. Current load: {CurrentLoad}kg/{MaxCapacity}kg.");
        }
    }
}
