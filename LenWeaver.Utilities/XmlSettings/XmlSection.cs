using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace LenWeaver.Utilities {

    public class XmlSection {

        private string                  _name;

        private XmlCommentCollection?   _commentsList   = null;

        private XmlKeyList              _keys;


        internal XmlSection( string name ) {

            _name = name;
            _keys = new XmlKeyList();
        }


        public string                   Name {
            get { return _name; }
            set { _name = value; }
        }

        public XmlCommentCollection     Comments {
            get {
                if( _commentsList == null ) {
                    _commentsList = new XmlCommentCollection();
                }

                return _commentsList;
            }
        }
        public XmlKeyList               Keys {
            get { return _keys; }
        }

        #region Get Methods
        public bool                     GetValue( string name, bool defaultValue ) {

            bool    result      = defaultValue;

            XmlKey? key;


            key = _keys[name];
            if( key != null ) {
                switch( key.Value.ToLower() ) {
                    case "true": result = true; break;
                    case "false": result = false; break;
                }
            }

            return result;
        }
        public int                      GetValue( string name, int defaultValue ) {

            int     result      = defaultValue;

            XmlKey? key;


            key = _keys[name];
            if( key != null ) {
                Int32.TryParse( key.Value, out result );
            }

            return result;
        }
        public long                     GetValue( string name, long defaultValue ) {

            long    result      = defaultValue;

            XmlKey? key;


            key = _keys[name];
            if( key != null ) {
                Int64.TryParse( key.Value, out result );
            }

            return result;
        }
        public float                    GetValue( string name, float defaultValue ) {

            float   result      = defaultValue;

            XmlKey? key;


            key = _keys[name];
            if( key != null ) {
                Single.TryParse( key.Value, out result );
            }

            return result;
        }
        public double                   GetValue( string name, double defaultValue ) {

            double  result      = defaultValue;

            XmlKey? key;


            key = _keys[name];
            if( key != null ) {
                Double.TryParse( key.Value, out result );
            }

            return result;
        }
        public decimal                  GetValue( string name, decimal defaultValue ) {

            decimal result      = defaultValue;

            XmlKey? key;


            key = _keys[name];
            if( key != null ) {
                Decimal.TryParse( key.Value, out result );
            }

            return result;
        }
        public string                   GetValue( string name, string defaultValue ) {

            string  result      = defaultValue;

            XmlKey? key;


            key = _keys[name];
            if( key != null ) {
                result = key.Value;
            }

            return result;
        }
        public DateTime                 GetValue( string name, DateTime defaultValue ) {

            DateTime    result      = defaultValue;

            XmlKey?     key;


            key = _keys[name];
            if( key != null ) {
                DateTime.TryParse( key.Value, out result );
            }

            return result;
        }
        public XmlKey?                  GetValue( string name ) {

            return _keys[name];
        }

        public T                        GetValue<T>( string name, T defaultValue ) {

            T       result  = defaultValue;

            XmlKey? key;


            if( _keys.Exists( name ) ) {
                key = _keys[name];

                if( !String.IsNullOrEmpty( key!.Value ) ) {
                    if( typeof( T ).IsEnum ) {
                        result = (T)Enum.Parse( typeof( T ), key.Value );
                    }
                    else {
                        result = (T)Convert.ChangeType( key.Value, typeof( T ) );
                    }
                }
            }

            return result;
        }

        public int[]                    GetValue( string name, int[] defaultValue ) {

            int         asInt32;

            XmlKey?     key;

            List<int>   result      = new List<int>();


            key = _keys[name];
            if( key != null ) {
                foreach( string token in key.Value.Split( ',' ) ) {
                    if( Int32.TryParse( token, out asInt32 ) ) {
                        result.Add( asInt32 );
                    }
                    else {
                        throw new InvalidCastException( "Unable to convert delimited string to integer array." );
                    }
                }
            }
            else {
                result.AddRange( defaultValue );
            }

            return result.ToArray();
        }
        #endregion
        #region Set Methods
        public XmlKey                   SetValue( string name, bool value ) {

            return SetValue( new XmlKey( name, value ? "True" : "False", "Boolean" ) );
        }
        public XmlKey                   SetValue( string name, int value ) {

            return SetValue( new XmlKey( name, value.ToString(), "Int32" ) );
        }
        public XmlKey                   SetValue( string name, long value ) {

            return SetValue( new XmlKey( name, value.ToString(), "Int64" ) );
        }
        public XmlKey                   SetValue( string name, float value ) {

            return SetValue( new XmlKey( name, value.ToString(), "Single" ) );
        }
        public XmlKey                   SetValue( string name, double value ) {

            return SetValue( new XmlKey( name, value.ToString(), "Double" ) );
        }
        public XmlKey                   SetValue( string name, decimal value ) {

            return SetValue( new XmlKey( name, value.ToString(), "Decimal" ) );
        }
        public XmlKey                   SetValue( string name, string value ) {

            return SetValue( new XmlKey( name, value, "String" ) );
        }
        public XmlKey                   SetValue( string name, string value, string type ) {

            return SetValue( new XmlKey( name, value, type ) );
        }
        public XmlKey                   SetValue( string name, DateTime value ) {

            return SetValue( new XmlKey( name, value.ToString( DateTimeHelpers.ISO8601DateTimeFormat ), "DateTime" ) );
        }
        public XmlKey                   SetValue( XmlKey key ) {

            if( _keys.Exists( key.Name ) ) {
                _keys.Remove( key.Name );
            }

            return _keys.Add( key );
        }
        public XmlKey                   SetValue( string name, int[] value ) {

            string[]    asString = new string[value.Length];


            for( int i = 0; i < value.Length; i++ ) {
                asString[i] = value[i].ToString();
            }

            return SetValue( new XmlKey( name, String.Join( ",", asString ), "Int32[]" ) );
        }
        #endregion
    }
}