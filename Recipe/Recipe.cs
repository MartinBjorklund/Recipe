using System;
using System.Collections.Generic;
using System.Text;

namespace Recipe
{
    class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<Tag> Tags { get; set; }
    }

}
