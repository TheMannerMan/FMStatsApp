using FMStatsApp.Models;
using System.Collections.Concurrent;
using System.Reflection;

namespace FMStatsApp.Services
{
    public interface IPlayerScoringService
    {
        List<Role> CalculateRoleScores(Player player);
        List<Role> CalculateRoleScores(Player player, IEnumerable<string> selectedRoles);
    }

    public class PlayerScoringService : IPlayerScoringService
    {
        private static readonly ConcurrentDictionary<string, PropertyInfo> _propertyCache = new();

        public List<Role> CalculateRoleScores(Player player)
        {
            return CalculateRoleScores(player, RoleCatalog.AllRoles.Select(r => r.ShortName));
        }

        public List<Role> CalculateRoleScores(Player player, IEnumerable<string> selectedRoles)
        {
            var roles = new List<Role>();
            var relevantRoles = RoleCatalog.AllRoles
                .Where(r => selectedRoles.Contains(r.ShortName));

            foreach (var roleDef in relevantRoles)
            {
                var roleScore = CalculateRoleScore(player, roleDef);
                roles.Add(new Role(roleDef.Name, roleDef.ShortName, roleScore));
            }

            return roles.OrderByDescending(r => r.Score).ToList();
        }

        private float CalculateRoleScore(Player player, RoleDefinition roleDef)
        {
            float totalScore = 0;
            int totalWeight = roleDef.AttributeWeights.Values.Sum();

            foreach (var (attribute, weight) in roleDef.AttributeWeights)
            {
                var attributeValue = GetPlayerAttributeValue(player, attribute);
                totalScore += attributeValue * weight;
            }

            return totalWeight > 0 ? totalScore / totalWeight : 0;
        }

        private int GetPlayerAttributeValue(Player player, string attributeName)
        {
            var propertyInfo = _propertyCache.GetOrAdd(attributeName, name =>
            {
                var prop = typeof(Player).GetProperty(name);
                if (prop == null)
                {
                    throw new ArgumentException($"Property '{name}' not found in Player class.");
                }
                return prop;
            });

            return (int)propertyInfo.GetValue(player);
        }
    }
}