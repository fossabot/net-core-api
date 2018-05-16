using System;
using System.Collections.Generic;

namespace RestApi.Models
{
    public class RecipeStep
    {
        public virtual Guid RecipeStepId { get; set; }
        public virtual int StepNo { get; set; }
        public virtual string Instructions { get; set; }
        public virtual IList<RecipeStep> RecipeItems { get; set; }
    }
}