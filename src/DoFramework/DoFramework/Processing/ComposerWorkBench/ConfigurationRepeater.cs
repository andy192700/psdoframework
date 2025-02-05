using DoFramework.Services;

namespace DoFramework.Processing;

/// <summary>
/// Represents a configuration repeater that registers and configures services.
/// </summary>
public class ConfigurationRepeater : Repeater<Type>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationRepeater"/> class with the specified service container.
    /// </summary>
    /// <param name="container">The service container used for registering and retrieving services.</param>
    public ConfigurationRepeater(IServiceContainer container) : base(container) { }

    /// <inheritdoc/>
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

