﻿using System.Reflection; // To use GetTypeInfo method.

namespace Northwind.Maui.Client.Controls;

public class EnumPicker : Picker
{
    public Type EnumType
    {
        get => (Type)GetValue(EnumTypeProperty);
        set => SetValue(EnumTypeProperty, value);
    }

    public static readonly BindableProperty EnumTypeProperty =
        BindableProperty.Create(
            propertyName: nameof(EnumType),
            returnType: typeof(Type),
            declaringType: typeof(EnumPicker),
            propertyChanged: (bindable, oldValue, newValue) =>
            {
                EnumPicker picker = (EnumPicker)bindable;

                if (oldValue != newValue) 
                {
                    picker.ItemsSource = null;
                }

                if (newValue != null)
                {
                    if (!((Type)newValue).GetTypeInfo().IsEnum)
                    {
                        throw new ArgumentException("EnumPicker: EnumType property must be enumeration type");
                    }

                    picker.ItemsSource = Enum.GetValues((Type)newValue);
                }
            });
}
