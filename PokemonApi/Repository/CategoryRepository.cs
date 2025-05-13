using PokemonApi.Data;
using PokemonApi.Interfaces;
using PokemonApi.Models;

namespace PokemonApi.Repository {
    public class CategoryRepository : ICategoryRepository {
        private DataContext _Context;
        public CategoryRepository(DataContext context) {
            _Context = context;

        }
        public bool CategoryExists(int id) {
            return _Context.Categories.Any(c => c.Id == id);
        }

        public bool CreateCategory(Category category) {
            _Context.Add(category);
            return save();
        }

        public bool DeleteCategory(Category category) {
            _Context.Remove(category);
            return save();
        }

        public ICollection<Category> GetCategories() {
            return _Context.Categories.OrderBy(c => c.Name).ToList();
        }

        public Category GetCategory(int id) {
            return _Context.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Pokemon> GetPokemonsByCategory(int categoryId) {
            return _Context.PokemonCategories
                .Where(pc => pc.CategoryId == categoryId)
                .Select(pc => pc.Pokemon)
                .ToList();
        }

        public bool save() {
            var saved = _Context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(Category category) {
            _Context.Update(category);
            return save();
        }
    }
}
