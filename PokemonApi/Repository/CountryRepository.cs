using AutoMapper;
using PokemonApi.Data;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Repository {
    public class CountryRepository : ICountryRepository {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public CountryRepository(DataContext context, IMapper mapper) {
            _context = context;
            _mapper = mapper;

        }
        public bool CountryExists(int id) {
            return _context.Countries.Any(c => c.Id == id);
        }

        public bool CreateCountry(Country country) {
            _context.Add(country);
            return save();
        }

        public bool DeleteCountry(Country country) {
            _context.Remove(country);
            return save();
        }

        public ICollection<Country> GetCountries() {
            return _context.Countries.ToList();
        }

        public Country GetCountry(int id) {
            return _context.Countries.Where(c => c.Id == id).FirstOrDefault();
        }

        public Country GetCountryByOwner(int ownerId) {
            return _context.Owners.Where(o => o.Id == ownerId)
                .Select(c => c.Country)
                .FirstOrDefault();
        }



        public ICollection<Owner> GetPokemonsByACountry(int countryId) {
            return _context.Owners.Where(c => c.Country.Id == countryId).ToList();
        }

        public bool save() {
            var save = _context.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool UpdateCountry(Country country) {
            _context.Update(country);
            return save();
        }
    }
}
