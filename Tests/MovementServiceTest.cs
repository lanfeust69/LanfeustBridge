using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace LanfeustBridge.Tests
{
    using Services;

    public class MovementServiceTest
    {
        [Fact]
        public void AllImplementedMovementsAreReturned()
        {
            var descriptions = MovementService.Service.GetAllMovements().ToList();
            Assert.Equal(4, descriptions.Count);
        }

        #pragma warning disable SA1201 // Elements must appear in the correct order
        public static IEnumerable<object[]> MovementIds
        {
            get
            {
                return MovementService.Service.GetAllMovements().Select(m => new object[] { m.Id });
            }
        }

        [Theory]
        [MemberData("MovementIds")]
        public void MovementCanBeRetrievedById(string movementId)
        {
            var movement = MovementService.Service.GetMovementDescription(movementId);
            Assert.NotNull(movement);
        }

        [Theory]
        [MemberData("MovementIds")]
        public void MovementsCanValidate(string movementId)
        {
            var valid = MovementService.Service.Validate(movementId, 2, 2);
            Assert.NotNull(valid);
        }

        [Fact]
        public void UnknownMovementReturnsNull()
        {
            var movement = MovementService.Service.GetMovementDescription("Unknown");
            Assert.Null(movement);
        }

        [Fact]
        public void UnknownMovementValidationReturnsNull()
        {
            var valid = MovementService.Service.Validate("Unknown", 2, 2);
            Assert.Null(valid);
        }
    }
}
