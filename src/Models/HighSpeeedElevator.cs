using ElevatorChallenge.src.Models.Enums;
using ElevatorChallenge.src.Services;

namespace ElevatorChallenge.src.Models
{
    public class HighSpeedElevator(int id, int maxCapacity, int maxFloors, ElevatorType type, Building building) : ElevatorContoller(id, maxCapacity, maxFloors, type, building)
    {
        public override void DisplayStatus()
        {
            Console.WriteLine(
                $"{Id,-3} | HighSpeed | {ElevatorState,-10} | Floor {CurrentFloor,-3} | {Direction,-10} | Passengers: {CurrentLoad}/{MaxCapacity}");
        }

        public override void LoadFreight(int weight)
        {
            throw new NotSupportedException("This elevator does not support freight.");
        }

        public override void UnloadFreight(int weight)
        {
            throw new NotSupportedException("This elevator does not support freight.");
        }
    }
}
