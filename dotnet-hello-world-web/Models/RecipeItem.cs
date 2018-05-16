using System;

namespace RestApi.Models
{
    public class RecipeItem
    {
        public virtual Guid ItemId { get; set; }
        public virtual string Name { get; set; }
        public virtual float Quantity { get; set; }
        public virtual string MeasurementUnit { get; set; }
    }
}