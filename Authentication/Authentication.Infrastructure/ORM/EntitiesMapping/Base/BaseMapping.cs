namespace Authentication.Infrastructure.ORM.EntitiesMapping.Base;
public class BaseMapping
{
    protected string Schema { get; }
    private const string SchemaDefault = "Auth";

    protected BaseMapping() =>
        Schema = SchemaDefault;

    protected BaseMapping(string schema) =>
        Schema = schema;
}
