using System;
using System.Collections.Generic;

namespace RestApi.Models
{
    public class Recipe
    {
        public virtual Guid RecipeId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Comments { get; set; }
        public virtual DateTime ModifyDate { get; set; } 
        public virtual IList<RecipeStep>  Steps{ get; set; }
    }
}