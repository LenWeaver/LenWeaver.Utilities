using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LenWeaver.Utilities {

    public static class MenuHelpers {


        static MenuHelpers() {}


        public static MenuItem? AddToMenu( ItemCollection items, string menuNamespace ) {

            return AddToMenu<MenuItem>( items, menuNamespace );
        }
        public static MenuItem? AddToMenu( ItemCollection items, string[] menuTokens ) {

            return AddToMenu<MenuItem>( items, menuTokens );
        }

        public static T? AddToMenu<T>( ItemCollection items, string menuNamespace ) where T : notnull, MenuItem, new() {

            T?      result;


            try {
                result = AddToMenu<T>( items, menuNamespace.Split( '.' ) );
            }
            catch( Exception ex ) {
                throw new ApplicationException( "Unable to add menu item to menu system.", ex );
            }

            return result;
        }
        public static T? AddToMenu<T>( ItemCollection items, string[] menuTokens ) where T : MenuItem, new() {

            bool                itemFound;

            T?                  result      = null;

            ItemCollection      menuItems   = items;


            try {
                for( int tokenIndex = 0; tokenIndex < menuTokens.Length; tokenIndex++ ) {
                    itemFound = false;

                    foreach ( object? o in menuItems ) {
                        if ( o is null ) {
                            throw new ArgumentNullException( "items argument contains a null." );
                        }
                        else if( o is Separator && menuTokens[tokenIndex] == "-" ) {
                            itemFound = true;
                            break;
                        }
                        else if( o is T && ((T)o).Header.ToString().Replace( "_", "" ) == menuTokens[tokenIndex].Replace( "_", "" ) ) {
                            itemFound = true;

                            result = (T)o;
                            break;
                        }
                    }

                    if( !itemFound ) {
                        if( menuTokens[tokenIndex] == "-" ) {
                            menuItems.Add( new Separator() );
                            break;
                        }
                        else {
                            result = new T();
                            result.Header = menuTokens[tokenIndex].Replace( "?", "..." );

                            menuItems.Add( result );
                        }                        
                    }

                    if( result == null ) break;

                    menuItems = result.Items;
                }
            }
            catch( Exception ex ) {
                throw new ApplicationException( "Unable to add menu item to menu system.", ex );
            }

            return result;
        }

        public static void RemoveFromMenu<T>( ItemCollection items, Predicate<T> shouldRemove ) {

            object          obj;

            ItemsControl?   itemsControl;


            for( int index = items.Count - 1; index >= 0; index-- ) {
                obj = items[index];

                if( obj is T && shouldRemove( (T)obj ) ) {
                    items.RemoveAt( index );
                }
                else {
                    itemsControl = obj as ItemsControl;
                    if( itemsControl != null ) {
                        RemoveFromMenu( itemsControl.Items, shouldRemove );
                    }
                }
            }
        }
    }
}