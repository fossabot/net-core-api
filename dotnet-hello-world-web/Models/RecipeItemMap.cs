using FluentNHibernate.Mapping;
using RestApi.Models;

namespace RestApi.Mappings
{
    public class RecipeItemMap: ClassMap<RecipeItem>
    {
        public RecipeItemMap()
        {
            Id(x => x.ItemId);
            Map(x => x.Name);
            Map(x => x.Quantity);
            Map(x => x.MeasurementUnit);
            Table("RecipeItems");
        }
    }
}