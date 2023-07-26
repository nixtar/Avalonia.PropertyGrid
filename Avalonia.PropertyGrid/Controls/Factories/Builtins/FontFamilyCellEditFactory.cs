﻿using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Avalonia.PropertyGrid.Controls.Factories.Builtins
{
    internal class FontFamilyCellEditFactory : AbstractCellEditFactory
    {
        public override int ImportPriority => base.ImportPriority - 1000000;

        public override Control HandleNewProperty(PropertyCellContext context)
        {
            var propertyDescriptor = context.Property;
            var target = context.Target;

            if (propertyDescriptor.PropertyType != typeof(FontFamily))
            {
                return null;
            }

            ComboBox control = new ComboBox();
            control.ItemsSource = FontManager.Current.SystemFonts.ToArray();
            control.HorizontalAlignment = Layout.HorizontalAlignment.Stretch;
            control.ItemTemplate = new FuncDataTemplate<FontFamily>((value, namescope) =>            
                new TextBlock
                {
                    [!TextBlock.TextProperty] = new Binding("Name"),
                    [!TextBlock.FontFamilyProperty] = new Binding()
                }
            );

            control.SelectionChanged += (s, e) =>
            {
                var item = control.SelectedItem;

                if (item is FontFamily ff)
                {                    
                    if (ff != propertyDescriptor.GetValue(target) as FontFamily)
                    {
                        SetAndRaise(context, control, ff);
                    }
                }
            };

            return control;
        }

        public override bool HandlePropertyChanged(PropertyCellContext context)
        {
            var propertyDescriptor = context.Property;
            var target = context.Target;
            var control = context.CellEdit;

            if (propertyDescriptor.PropertyType != typeof(FontFamily))
            {
                return false;
            }

            ValidateProperty(control, propertyDescriptor, target);

            if (control is ComboBox cb)
            {
                FontFamily value = propertyDescriptor.GetValue(target) as FontFamily;
                cb.SelectedItem = value;
                return true;
            }

            return false;
        }
    }
}
