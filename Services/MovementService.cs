using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LanfeustBridge.Services
{
    using LanfeustBridge.Models;

    public class MovementService
    {
        private static readonly Lazy<MovementService> s_service = new Lazy<MovementService>(() => new MovementService());

        private readonly Lazy<Dictionary<string, IMovement>> _mouvementInstances =
            new Lazy<Dictionary<string, IMovement>>(FindAllMovements);

        public static MovementService Service => s_service.Value;

        private Dictionary<string, IMovement> MouvementInstances => _mouvementInstances.Value;

        public IEnumerable<MovementDescription> GetAllMovements()
        {
            return MouvementInstances.Values.Select(m => m.MovementDescription).OrderBy(m => m.Order);
        }

        public IMovement GetMovement(string id)
        {
            MouvementInstances.TryGetValue(id, out var movement);
            return movement;
        }

        public MovementDescription GetMovementDescription(string id)
        {
            return GetMovement(id)?.MovementDescription;
        }

        public MovementValidation Validate(string id, int nbTables, int nbRounds)
        {
            return GetMovement(id)?.Validate(nbTables, nbRounds);
        }

        private static Dictionary<string, IMovement> FindAllMovements()
        {
            return typeof(MovementService).GetTypeInfo().Assembly.GetTypes()
                .Where(t => t.GetTypeInfo().IsClass && typeof(IMovement).IsAssignableFrom(t))
                .Select(t => (IMovement)Activator.CreateInstance(t))
                .ToDictionary(m => m.MovementDescription.Id);
        }
    }
}
