namespace RestaurantSystem.Services
{
    using Models;

    public class RestaurantManager
    {
        private List<Ingredient> _ingredients = new List<Ingredient>();
        private List<Dish> _dishes = new List<Dish>();
        private List<Order> _orders = new List<Order>();

        private int _ingredientIdCounter = 1;
        private int _dishIdCounter = 1;
        private int _orderIdCounter = 1;
        
        // Ingredient Management
        public Ingredient AddIngredient(string name)
        {
            var ingredient = new Ingredient { Id = _ingredientIdCounter++, Name = name };
            _ingredients.Add(ingredient);
            return ingredient;
        }

        public bool DeleteIngredient(int id)
        {
            bool isUsed = _dishes.Any(d => d.Ingredients.Any(i => i.Id == id));
            if (isUsed) throw new InvalidOperationException("Cannot delete ingredient because it is used in a dish.");
            
            var ingredient = _ingredients.FirstOrDefault(i => i.Id == id);
            if (ingredient != null)
            {
                _ingredients.Remove(ingredient);
                return true;
            }
            
            return false;
        }

        public void UpdateIngredient(int id, string newName)
        {
            var ingredient = _ingredients.FirstOrDefault(i => i.Id == id);
            if (ingredient != null) ingredient.Name = newName;
        }
        
        public List<Ingredient> GetAllIngredients() => _ingredients;
        
        // Dish Management
        public Dish AddDish(string name, decimal price, int prepTime)
        {
            var dish = new Dish { Id = _dishIdCounter++, Name = name, Price = price, PrepTime = prepTime };
            _dishes.Add(dish);
            return dish;
        }

        public void DeleteDish(int id) => _dishes.RemoveAll(d => d.Id == id);

        public void UpdateDish(int id, string? newName, decimal? newPrice, int? newPrepTime)
        {
            var dish = GetDish(id);
            if (dish != null)
            {
                if (!string.IsNullOrEmpty(newName)) dish.Name = newName;
                if (newPrice.HasValue) dish.Price = newPrice.Value;
                if (newPrepTime.HasValue) dish.PrepTime = newPrepTime.Value;
            }
        }

        public void AddIngredientToDish(int dishId, int ingredientId)
        {
            var dish = GetDish(dishId);
            var ingredient = _ingredients.FirstOrDefault(i => i.Id == ingredientId);
            
            if (dish != null && ingredient != null && !dish.Ingredients.Contains(ingredient))
                dish.Ingredients.Add(ingredient);
        }

        public void RemoveIngredientFromDish(int dishId, int ingredientId)
        {
            var dish = GetDish(dishId);
            dish?.Ingredients.RemoveAll(i => i.Id == ingredientId);
        }
        
        public Dish GetDish(int id) => _dishes.FirstOrDefault(d => d.Id == id);
        
        // Order Management
        public Order AddOrder(int tableNumber)
        {
            var order = new Order { Id = _orderIdCounter++, TableNumber = tableNumber };
            _orders.Add(order);
            return order;
        }
        
        public void DeleteOrder(int id) => _orders.RemoveAll(o => o.Id == id);

        public void UpdateOrderDishQuantity(int orderId, int dishId, int newQuantity)
        {
            var order = GetOrder(orderId);
            if (order == null) return;
            
            var item = order.Items.FirstOrDefault(i => i.Dish.Id == dishId);
            if (item != null)
            {
                if (newQuantity <= 0) order.Items.Remove(item);
                else item.Quantity = newQuantity;
            }
            else if (newQuantity > 0)
            {
                var dish = GetDish(dishId);
                if (dish != null) order.Items.Add(new OrderItem { Dish = dish, Quantity = newQuantity });
            }
        }

        public void UpdateOrderTotalCost(int orderId, decimal newTotal)
        {
            var order = GetOrder(orderId);
            if (order != null) order.TotalCost = newTotal;
        }

        public void UpdateOrderTable(int orderId, int newTable)
        {
            var order = GetOrder(orderId);
            if (order != null) order.TableNumber = newTable;
        }
        
        public Order GetOrder(int id) => _orders.FirstOrDefault(o => o.Id == id);
        
        // Search
        public List<Ingredient> SearchIngredients(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return new List<Ingredient>();
            return _ingredients.Where(i => i.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Order> SearchOrders(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return new List<Order>();
            return _orders.Where(o => o.Items.Any(i => i.Dish.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))).ToList();
        }
    }
}