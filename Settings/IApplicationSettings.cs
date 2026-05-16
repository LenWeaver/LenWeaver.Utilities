using System;
using System.Collections.Generic;
using System.Text;

namespace LenWeaver.Utilities {

    public interface IApplicationSettings {

        SettingsSource      Source              { get; }

        string              ApplicationName     { get; set; }
        string              MachineName         { get; set; }
        string              Section             { get; set; }


        void                Delete              ( string? machineName, string? applicationName, string? section, string key );
        void                Delete              ( string? applicationName, string? section, string key );
        void                Delete              ( string? section, string key );
        void                Delete              ( string key );

        void                Set                 ( string? machineName, string? applicationName, string? section, string key, object? value );
        void                Set                 ( string? applicationName, string? section, string key, object? value );
        void                Set                 ( string? section, string key, object? value );
        void                Set                 ( string key, object? value );

        bool                IsKnownType         ( Type t );
        bool                IsKnownType         ( string typeName );

        IEnumerable<string> GetMachineNames     ();
        IEnumerable<string> GetApplicationNames ( string machineName );
        IEnumerable<string> GetSectionNames     ( string machineName, string applicationName );
        IEnumerable<string> GetKeyNames         ( string machineName, string applicationName, string sectionName );
        IEnumerable<string> GetKeyNames         ( string applicationName, string section );
        IEnumerable<string> GetKeyNames         ( string section );

        bool                TryGet              ( string? machineName, string? applicationName, string? section, string key, out object result );
        bool                TryGet              ( string? applicationName, string? section, string key, out object result );
        bool                TryGet              ( string? section, string key, out object result );
        bool                TryGet              ( string key, out object result );

        bool                TryGet<T>           ( string? machineName, string? applicationName, string? section, string key, out T result );
        bool                TryGet<T>           ( string? applicationName, string? section, string key, out T result );
        bool                TryGet<T>           ( string? section, string key, out T result );
        bool                TryGet<T>           ( string key, out T result );

        object              Get                 ( string? machineName, string? applicationName, string? section, string key, object defaultValue );
        object              Get                 ( string? applicationName, string? section, string key, object defaultValue );
        object              Get                 ( string? section, string key, object defaultValue );
        object              Get                 ( string key, object defaultValue );

        object?             Get                 ( string? machineName, string? applicationName, string? section, string key );
        object?             Get                 ( string? applicationName, string? section, string key );
        object?             Get                 ( string? section, string key );
        object?             Get                 ( string key );

        T                   Get<T>              ( string? machineName, string? applicationName, string? section, string key, T defaultValue );
        T                   Get<T>              ( string? applicationName, string? section, string key, T defaultValue );
        T                   Get<T>              ( string? section, string key, T defaultValue );
        T                   Get<T>              ( string key, T defaultValue );

        T?                  Get<T>              ( string? machineName, string? applicationName, string? section, string key );
        T?                  Get<T>              ( string? applicationName, string? section, string key );
        T?                  Get<T>              ( string? section, string key );
        T?                  Get<T>              ( string key );
    }


    public static class ApplicationSettings {

        #region Column Minimum and Maximum Lengths
        public const int            ApplicationNameMinLength    =   5;
        public const int            ApplicationNameMaxLength    = 100;
        public const int            MachineNameMinLength        =   5;
        public const int            MachineNameMaxLength        = 100;
        public const int            SectionMinLength            =   5;
        public const int            SectionMaxLength            = 100;
        public const int            KeyMinLength                =   3;
        public const int            KeyMaxLength                = 100;
        #endregion
    }
}