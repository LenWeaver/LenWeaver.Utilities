using System;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    public interface IColorTheme : IComparable<IColorTheme> {
        public string   Name                    { get; }

        public Brush    LightPanel              { get; }
        public Brush    DarkPanel               { get; }
        public Brush    ButtonBackground        { get; }
        public Brush    ButtonForeground        { get; }
        public Brush    ControlBackground       { get; }
        public Brush    ControlForeground       { get; }
        public Brush    HighlightBackground     { get; }
        public Brush    HighlightForeground     { get; }
        public Brush    LabelForeground         { get; }
    }
}