using DoFramework.Services;

namespace DoFramework.Processing;

public class ConfigurationRepeater : Repeater<Type>
{
    public ConfigurationRepeater(IServiceContainer container) : base(container) { }

    public override IRepeater<Type> And(Type input)
    {
        _container.RegisterService(input);

        var obj = _container.GetService(input);

        var properties = input.GetProperties();

        var context = _container.GetService<IContext>();

        foreach (var property in properties)
        {
            var value = context.Get($"{input.Name}.{property.Name}");

            if (value != null)
            {
                property.SetValue(obj, Convert.ChangeType(value, property.PropertyType));
            }
        }

        return this;
    }
}
