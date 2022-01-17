using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.DTO;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;

namespace GameStore.BLL.Services
{
    public static class KeyGenerator
    {
        public static string GenerateKey(GameDto entity, ISoftDeletableRepository<Game> gameRepository)
        {
            var key = entity.Name.Substring(0, Math.Min(5, entity.Name.Length));

            var sameKey = gameRepository.GetSingle(
                game => game.Key,
                item => item.OrderByDescending(game => game.Key),
                predicates: game => game.Key.StartsWith(key),
                includeDeleted: true);

            if (string.IsNullOrEmpty(sameKey) || !sameKey.Any(char.IsDigit) || sameKey.Equals(key))
            {
                return key += 1;
            }

            var restKey = sameKey.ToCharArray(key.Length, sameKey.Length - key.Length).Reverse().ToList();
            var number = int.Parse(new string(restKey.TakeWhile(char.IsDigit).Reverse().ToArray()));

            return key += number + 1;
        }
    }
}
