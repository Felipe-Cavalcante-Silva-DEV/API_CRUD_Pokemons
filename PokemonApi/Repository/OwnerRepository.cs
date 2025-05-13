using PokemonApi.Data;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Repository {
    public class OwnerRepository : IOwnerRepository {
        private readonly DataContext _context;
        public OwnerRepository(DataContext context) {
            _context = context;

        }

        public bool CreateOwner(Owner owner) {
            _context.Add(owner);
            return save();
        }

        public bool DeleteOwner(Owner owner) {
            _context.Remove(owner);
            return save();
        }

        public Owner GetOwner(int ownerId) {
            return _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnerOfAPokemon(int pokeId) {
            return _context.PokemonOwners.Where(po => po.PokemonId == pokeId)
                .Select(o => o.Owner)
                .ToList();
        }

        public ICollection<Owner> GetOwners() {

            return _context.Owners.OrderBy(o => o.LastName).ToList();
        }

        public ICollection<Pokemon> GetPokemonsByOwner(int ownerId) {
            return _context.PokemonOwners.Where(p => p.Owner.Id == ownerId).Select(p => p.Pokemon).ToList();
        }

        public bool OwnerExists(int ownerId) {
            return _context.Owners.Any(o => o.Id == ownerId);
        }

        public bool save() {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;

        }

        public bool UpdateOwner(Owner owner) {
            _context.Update(owner);
            return save();

        }
    }
}
