using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using LanfeustBridge.Models;

namespace LanfeustBridge.Services
{
    public class MovementService
    {
        private static Lazy<MovementService> _service = new Lazy<MovementService>(() => new MovementService());

        public static MovementService Service { get { return _service.Value; } }

        private Lazy<Dictionary<string, IMovement>> _mouvementInstances =
            new Lazy<Dictionary<string, IMovement>>(FindAllMovements);

        private Dictionary<string, IMovement> MouvementInstances { get { return _mouvementInstances.Value; } }

        public IEnumerable<MovementDescription> GetAllMovements()
        {
            return MouvementInstances.Values.Select(m => m.MovementDescription);
        }

        public IMovement GetMovement(string id)
        {
            MouvementInstances.TryGetValue(id, out IMovement movement);
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
