using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RestApi.Mappings;
using RestApi.Models;

namespace RestApi.Controllers
{
    [Route("api/[controller]")]
    public class RecipesController : Controller
    {
        private readonly Repository _repository = Repository.Instance;

        [HttpGet]
        public IEnumerable<Recipe> Get()
        {
            return _repository.GetAllRecipes();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var recipe = _repository.GetRecipe(id);
            if (recipe != null)
                return new ObjectResult(recipe);
            return new NotFoundResult();
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] Recipe recipe)
        {
            if (recipe.RecipeId == Guid.Empty)
            {
                return new ObjectResult(_repository.AddRecipe(recipe));
            }

            var existingOne = _repository.GetRecipe(recipe.RecipeId);
            existingOne.Name = recipe.Name;
            existingOne.Comments = recipe.Comments;
            _repository.UpdateRecipe(existingOne);
            return new ObjectResult(existingOne);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] Recipe recipe)
        {
            var existingOne = _repository.GetRecipe(recipe.RecipeId);
            existingOne.Name = recipe.Name;
            existingOne.Comments = recipe.Comments;
            _repository.UpdateRecipe(recipe);
            return new ObjectResult(existingOne);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _repository.DeleteRecipe(id);
            return new StatusCodeResult(200);
        }
    }
}