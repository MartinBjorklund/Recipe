using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Recipe
{
    class DataRepository
    {
        const string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog = recipe; Integrated Security = True";

        public static List<Recipe> GetRecipes(string recipeTag)
        {
            List<Recipe> recipes = new List<Recipe>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                string sqlString = $"usp_getRecipe @recipeTag";
                SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
                sqlCommand.Parameters.Add(new SqlParameter("@recipeTag", recipeTag));

                var reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    Recipe recipe = new Recipe();
                    int.TryParse(reader[0].ToString(), out int id);
                    recipe.Id = id;
                    recipe.Name = reader[1].ToString();
                    recipe.Ingredients = GetIngredient(id);
                    recipes.Add(recipe);
                }
            }
            return recipes;
        }
        public static List<Ingredient> GetIngredient(int recipeId)
        {
            List<Ingredient> ingredients = new List<Ingredient>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                string sqlString = $"select BaseIngredients.id, BaseIngredients.Name, Amount From Ingredients inner join baseingredients on BaseIngredients.id = Ingredients.BaseIngredientid where Recipeid = @recipeId";
                SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
                sqlCommand.Parameters.Add(new SqlParameter("@recipeId", recipeId));

                var reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    Ingredient ingredient = new Ingredient();
                    int.TryParse(reader[0].ToString(), out int id);
                    ingredient.Id = id;
                    ingredient.IngredientName = reader[1].ToString();
                    ingredient.Amount = reader[2].ToString();
                    ingredients.Add(ingredient);
                }
            }
            return ingredients;
        }
        public static List<Recipe> GetAllRecipes()
        {
            List<Recipe> recipes = new List<Recipe>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                string sqlString = $"select Id, RecipeName from Recipe";
                SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);

                var reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    Recipe recipe = new Recipe();
                    int.TryParse(reader[0].ToString(), out int id);
                    recipe.Id = id;
                    recipe.Name = reader[1].ToString();
                    recipe.Ingredients = GetIngredient(id);
                    recipes.Add(recipe);
                }
            }
            return recipes;
        }

        public static void CreateRecipes(string input, List<Ingredient> ingredients)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                string sqlString = $"insert Recipe(RecipeName) values (@input)";
                SqlCommand sqlcommand = new SqlCommand(sqlString, sqlConnection);
                sqlcommand.Parameters.Add(new SqlParameter("@input", input));
                sqlcommand.ExecuteNonQuery();
            }
        }
        public static void DeleteRecipeFromDatabase(string id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();

                string sqlString = $"exec usp_DeleteRecipe @RecipeId";
                SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
                sqlCommand.Parameters.Add(new SqlParameter("@RecipeId", id));

                sqlCommand.ExecuteNonQuery();
            }
        }

        public static void EditRecipe(string input, int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                string sqlstring = "UPDATE RECIPE SET RecipeName = @input WHERE Recipe.id = @id";
                SqlCommand sqlCommand = new SqlCommand(sqlstring, sqlConnection);
                sqlCommand.Parameters.Add(new SqlParameter("@input", input));
                sqlCommand.Parameters.Add(new SqlParameter("@id", id));
                sqlCommand.ExecuteNonQuery();
            }


        }
    }
    
}
