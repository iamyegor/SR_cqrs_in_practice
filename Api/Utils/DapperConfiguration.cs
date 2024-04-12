using System.Reflection;
using Dapper;
using Logic.Interfaces;

namespace Api.Utils;

public static class DapperConfiguration
{
    public static void ConfigureSnakeCaseMapping()
    {
        Assembly assembly = typeof(ILogicAssembly).Assembly;

        foreach (var modelType in assembly.GetTypes())
        {
            SqlMapper.SetTypeMap(
                modelType,
                new CustomPropertyTypeMap(
                    modelType,
                    (type, columnName) =>
                        type.GetProperties()
                            .FirstOrDefault(prop =>
                                prop.Name.ToLower() == columnName.Replace("_", "").ToLower()
                            )!
                )
            );
        }
    }
}
