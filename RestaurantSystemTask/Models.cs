using System;
using System.Collections.Generic;
using System.Linq;

namespace RestaurantSystem.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString() => $"{Id}: {Name}";
    }

    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int PrepTime { get; set; }
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

        public override string ToString()
        {
            var ingredientsStr = string.Join(", ", Ingredients.Select(i => i.Name));
            return $"{Id}: {Name} | {Price:C} | {PrepTime} mins | Ingredients: {ingredientsStr}";
        }
    }

    public class OrderItem
    {
        public Dish Dish { get; set; }
        public int Quantity { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public int TableNumber { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        private decimal? _manualTotalCost;
        public decimal TotalCost
        {
            get => _manualTotalCost ?? Items.Sum(i => i.Dish.Price * i.Quantity);
            set => _manualTotalCost = value;
        }

        public override string ToString()
        {
            var itemsStr = string.Join(", ", Items.Select(i => $"{i.Quantity}x {i.Dish.Name}"));
            return $"Order {Id} | Table: {TableNumber} | Total: {TotalCost:C} | Items: {itemsStr}";
        }
    }
}