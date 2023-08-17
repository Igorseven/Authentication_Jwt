namespace Authentication.Infrastructure.ORM.EntitiesMapping.Base;
public class BaseMapping
{
    protected string Schema { get; set; }
    private const string SchemaDefault = "Auth";

    public BaseMapping() =>
        Schema = SchemaDefault;

    public BaseMapping(string schema) =>
        Schema = schema;
}
