using System;
using System.Collections.Generic;
using System.Linq;
using Fi.Infra.Schema.Model;
using Fi.Infra.Schema.Utility;
using Fi.Test.IntegrationTests.IntegrationTestHelper;
using Fi.Test.Utility;
using Fi.TemplateUniqueName.Schema.Model;

namespace Fi.Test.Extensions;

public static class BuilderExtensions
{
    public static T AddSchemaDefaults<T>(this T value)
    {
        var properties = value.GetType().GetProperties();
        foreach (var property in properties)
        {
            // if (property.Name == "Set_your_props_here")
            // {
            //     property.SetValue(value, "set_the_value_of_the_prop_here");
            // }
        }

        return value;
    }

    public static IList<T> AddAllSchemaDefaultsToItems<T>(this IList<T> value)
    {
        foreach (var item in value)
        {
            item.AddSchemaDefaults();
        }

        return value;
    }
}