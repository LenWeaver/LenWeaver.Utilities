using System;
using System.Collections.Generic;
using System.Windows;

namespace LenWeaver.Utilities {


    public interface ISkin {
        ResourceDictionary      Shared          { get; }

        ResourceDictionary      Button          { get; }
        ResourceDictionary      Calendar        { get; }
        ResourceDictionary      ComboBox        { get; }
        ResourceDictionary      DatePicker      { get; }
        ResourceDictionary      ListBox         { get; }
        ResourceDictionary      TextBox         { get; }
    }
}