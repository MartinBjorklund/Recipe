using Recipe;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Recipe
{
    public class RecepieUI
    {
        const string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog = recipe; Integrated Security = True";

        public static void Run()
        {
            DisplayMenu();
        }
        public static void DisplayMenu()
        {

            while (true)
            {
                Console.Clear();
                Console.WriteLine(@"

                      ██████╗ ███████╗ ██████╗██╗██████╗ ███████╗    ██████╗ ██████╗ ███╗   ███╗
                      ██╔══██╗██╔════╝██╔════╝██║██╔══██╗██╔════╝   ██╔════╝██╔═══██╗████╗ ████║
                      ██████╔╝█████╗  ██║     ██║██████╔╝█████╗     ██║     ██║   ██║██╔████╔██║
                      ██╔══██╗██╔══╝  ██║     ██║██╔═══╝ ██╔══╝     ██║     ██║   ██║██║╚██╔╝██║
                      ██║  ██║███████╗╚██████╗██║██║     ███████╗██╗╚██████╗╚██████╔╝██║ ╚═╝ ██║
                      ╚═╝  ╚═╝╚══════╝ ╚═════╝╚═╝╚═╝     ╚══════╝╚═╝ ╚═════╝ ╚═════╝ ╚═╝     ╚═╝                
                                                                                                      ");
                Console.WriteLine();
                Console.WriteLine("PRESS (1) TO LIST ALL RECIPES ");
                Console.WriteLine("PRESS (2) TO SEARCH BY INGREDIENT ");
                Console.WriteLine("PRESS (3) TO CREATE YOUR OWN RECIPE");
                Console.WriteLine("PRESS (4) TO EDIT A RECIPE");
                Console.WriteLine("PRESS (5) TO DELETE A RECIPE ");
                Console.WriteLine("PRESS (6) TO SHOW A RANDOM RECIPE");
                Console.WriteLine("PRESS (7) TO QUIT");

                char Input = char.Parse(Console.ReadLine());
                if (Input == '7')
                {
                    break;
                }
                List<Recipe> recipes = new List<Recipe>();
                recipes = DataRepository.GetAllRecipes();
                switch (Input)
                {
                    case '1':
                        ShowAllRecipes(recipes);
                        Console.ReadKey();
                        break;
                    case '2':
                        SearchByIngredient();
                        break;
                    case '3':
                        CreateRecipe();
                        break;
                    case '4':
                        EditRecipeDialog(recipes);
                        break;
                    case '5':
                        DeleteRecipe(recipes);
                        break;
                    case '6':
                        RandomRecipe(recipes);
                        break;
                    default:
                        Console.WriteLine("ENTER A VALID INPUT");
                        break;
                }

            }

        }

        private static void EditRecipeDialog(List<Recipe> recipes)
        {
            ShowAllRecipes(recipes);
            Console.WriteLine();
            Console.WriteLine("Which recipe would you like to edit? ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Whats the new name of the recipe? ");
            string input = Console.ReadLine();

            DataRepository.EditRecipe(input, id);
        }


        private static void RandomRecipe(List<Recipe> recipes)
        {
            Console.Clear();
            Random random = new Random();
            int[] recipeIds = new int[recipes.Count];
            int i = 0;
            foreach (var recipe in recipes)
            {
                recipeIds[i] = recipe.Id;
                i++;
            }

            int randomRecipe = random.Next(0, recipes.Count);

            foreach (var recipe in recipes)
            {

                if (recipeIds[randomRecipe] == recipe.Id)
                {
                    Console.WriteLine(recipe.Name);
                    Console.WriteLine();
                    foreach (var item in recipe.Ingredients)
                    {

                        Console.WriteLine($"{item.IngredientName} {item.Amount}");
                    }
                }
            }

            Console.ReadKey();
        }


        private static void ShowAllRecipes(List<Recipe> recipes)
        {
            Console.Clear();
            foreach (var recipe in recipes)
            {
                Console.Write(($"{recipe.Id}) {recipe.Name}").PadRight(20));
                Console.Write("Ingredients: " );
                int i = 1;
                foreach (var item in recipe.Ingredients)
                {
                    Console.Write($"{item.IngredientName} {item.Amount}");
                    if (i != recipe.Ingredients.Count)
                    {
                        Console.Write(", ");
                    }
                    i++;
                }
                Console.WriteLine();
                Console.WriteLine("-------------------------------------------------------------------------------");
            }
        }

        private static void CreateRecipe()
        {
            Console.Clear();
            Console.WriteLine("whats your recipe name?");
            Console.Write("Name: ");
            string input = Console.ReadLine();
            Console.WriteLine("How many ingredients?");
            int.TryParse(Console.ReadLine(), out int numberOfIngredients);

            List<Ingredient> ingredients = new List<Ingredient>();
            for (int i = 0; i < numberOfIngredients; i++)
            {
                Ingredient ingredient = new Ingredient();
                Console.WriteLine("Which ingredient would you like to add?");
                ingredient.IngredientName = Console.ReadLine();
                Console.WriteLine("Amount: ");
                ingredient.Amount = Console.ReadLine();
                ingredients.Add(ingredient);
            }

            DataRepository.CreateRecipes(input, ingredients);
        }

        private static void DeleteRecipe(List<Recipe> recipes)
        {
            ShowAllRecipes(recipes);
            Console.WriteLine();
            Console.WriteLine("Which recipe would you like to delete? ");
            string id = Console.ReadLine();
            Console.WriteLine($"Are you sure you want to delete recipe number {id}");
            string input = Console.ReadLine();
            if (input.ToLower() == "yes")
            {
                DataRepository.DeleteRecipeFromDatabase(id);
            }
            Console.ReadKey();
        }

        private static void SearchByIngredient()
        {
            Console.Clear();
            Console.WriteLine("Search for recipes based on a ingredient!");
            Console.WriteLine();
            Console.Write("Search: ");
            string ingredient = Console.ReadLine();
            List<Recipe> recipes = new List<Recipe>();
            recipes = DataRepository.GetRecipes(ingredient);
            DisplayRecipes(recipes);

        }

        private static void DisplayRecipes(List<Recipe> recipes)
        {
            Console.WriteLine();
            foreach (var recipe in recipes)
            {
                Console.WriteLine($"{recipe.Id}) {recipe.Name}");
            }

            Console.ReadKey();
        }

    }
}
