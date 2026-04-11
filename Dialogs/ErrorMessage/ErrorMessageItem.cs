using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LenWeaver.Utilities {

    internal class ErrorMessageItem : ListBoxItem {

        private TextBlock   txtItemName;
        private TextBlock   txtItemValue;


        internal ErrorMessageItem( string name, string value ) {

            ColumnDefinition    columnDef;
            Grid                grd;


            grd                     = new Grid();
            
            columnDef               = new ColumnDefinition();
            columnDef.Width         = new GridLength( 80d );
            grd.ColumnDefinitions.Add( columnDef );

            columnDef               = new ColumnDefinition();
            columnDef.Width         = GridLength.Auto;
            grd.ColumnDefinitions.Add( columnDef );

            txtItemName             = new TextBlock();
            txtItemName.Text        = name;
            txtItemName.FontWeight  = FontWeights.Bold;
            txtItemName.Padding     = new Thickness( 0d );
            txtItemName.SetValue( Grid.ColumnProperty, 0 );

            grd.Children.Add( txtItemName );

            txtItemValue            = new TextBlock();
            txtItemValue.Text       = value;
            txtItemValue.Padding    = new Thickness( 0d );
            txtItemValue.SetValue( Grid.ColumnProperty, 1 );

            grd.Children.Add( txtItemValue );

            base.Content            = grd;
        }


        internal string ItemName {
            get{ return txtItemName.Text; }
        }
        internal string ItemValue {
            get{ return txtItemValue.Text; }
        }
    }
}