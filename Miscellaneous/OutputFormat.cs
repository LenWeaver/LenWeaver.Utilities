using System;
using System.Collections.Generic;

namespace LenWeaver.Utilities {

    public enum OutputFormat {
        CommaSeparatedValues,
        TabSeparatedValues,
        PipeSeparatedValues,
        XmlNodes,
        XmlAttributes
    }


    public static class OutputFormatHelpers {


        static OutputFormatHelpers() {}


        public static bool IsDefined( int of ) {

            return of >= (int)OutputFormat.CommaSeparatedValues && of <= (int)OutputFormat.XmlAttributes;
        }
        public static bool IsDefined( OutputFormat of ) {

            return IsDefined( (int)of );
        }

        public static OutputFormat Parse( int of ) {

            OutputFormat        result;


            if( IsDefined( of ) ) {
                result = (OutputFormat)of;
            }
            else {
                throw new ArgumentException( "Unknown value for integer OutputFormat." );
            }

            return result;
        }
        public static OutputFormat Parse( string of ) {

            OutputFormat        result;


            switch( of.Trim().ToUpper() ) {
                case "COMMASEPARATEDVALUES":
                case "COMMA SEPARATED VALUES":
                    result = OutputFormat.CommaSeparatedValues;
                    break;

                case "TABSEPARATEDVALUES":
                case "TAB SEPARATED VALUES":
                    result = OutputFormat.TabSeparatedValues;
                    break;

                case "PIPESEPARATEDVALUES":
                case "PIPE SEPARATED VALUES":
                    result = OutputFormat.PipeSeparatedValues;
                    break;

                case "XMLNODES":
                case "XML NODES":
                    result = OutputFormat.XmlNodes;
                    break;

                case "XMLATTRIBUTES":
                case "XML ATTRIBUTES":
                    result = OutputFormat.XmlAttributes;
                    break;

                default:
                    throw new ArgumentException( "Unknown value for string OutputFormat." );
            }

            return result;
        }

        public static string ToDisplayString( OutputFormat of ) {

            string      result;


            switch( of ) {
                case OutputFormat.CommaSeparatedValues:     result = "Comma Separated Values";  break;
                case OutputFormat.TabSeparatedValues:       result = "Tab Separated Values";    break;
                case OutputFormat.PipeSeparatedValues:      result = "Pipe Separated Values";   break;
                case OutputFormat.XmlNodes:                 result = "Xml Nodes";               break;
                case OutputFormat.XmlAttributes:            result = "Xml Attributes";          break;

                default:
                    throw new ArgumentException( "Unable to generate OutputFormat display string." );
            }

            return result;
        }
        public static string ToString( OutputFormat of ) {

            string      result;


            switch( of ) {
                case OutputFormat.CommaSeparatedValues:     result = "CommaSeparatedValues";    break;
                case OutputFormat.TabSeparatedValues:       result = "TabSeparatedValues";      break;
                case OutputFormat.PipeSeparatedValues:      result = "PipeSeparatedValues";     break;
                case OutputFormat.XmlNodes:                 result = "XmlNodes";                break;
                case OutputFormat.XmlAttributes:            result = "XmlAttributes";           break;

                default:
                    throw new ArgumentException( "Unable to generate OutputFormat display string." );
            }

            return result;
        }

        public static IEnumerable<OutputFormat> ForEach() {

            yield return OutputFormat.CommaSeparatedValues;
            yield return OutputFormat.TabSeparatedValues;
            yield return OutputFormat.PipeSeparatedValues;
            yield return OutputFormat.XmlNodes;
            yield return OutputFormat.XmlAttributes;
        }
    }
}