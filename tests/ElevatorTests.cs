using ElevatorChallenge.src.Models.Enums;
using ElevatorChallenge.src.Services;
using Xunit;

namespace ElevatorChallenge.tests
{
    public class ElevatorTests
    {
        private readonly Building _building;
        private readonly TestElevator _elevator;

        public ElevatorTests()
        {
            _building = new Building(15, 1, 1, 1); // Use a real Building instance
            _elevator = new TestElevator(3, 10, 6, ElevatorType.Standard, _building);
        }

        [Fact]
        public void MoveToFloor_ElevatorMovesToValidFloor()
        {
            int targetFloor = 1;
            _elevator.MoveToFloor(targetFloor);
            Assert.Equal(targetFloor, _elevator.CurrentFloor);
            Assert.Equal(Direction.Idle, _elevator.Direction);
            Assert.Equal(State.Stationary, _elevator.ElevatorState);
        }

        [Fact]
        public void MoveToFloor_OutOfOrder_DoesNotMove()
        {
            _elevator.SetOutOfOrder();
            int targetFloor = 3;

            _elevator.MoveToFloor(targetFloor);

            Assert.NotEqual(targetFloor, _elevator.CurrentFloor);
            Assert.Equal(State.OutOfOrder, _elevator.ElevatorState);
        }

        private class TestElevator(int id, int maxCapacity, int maxFloors, ElevatorType type, Building building)
            : ElevatorContoller(id, maxCapacity, maxFloors, type, building)
        {
            public override void DisplayStatus() { }
            public override void LoadFreight(int weight) { }
            public override void UnloadFreight(int weight) { }
        }
    }
}
