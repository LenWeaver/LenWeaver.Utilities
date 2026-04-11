using System;
using System.Collections.Generic;
using System.Windows;

namespace LenWeaver.Utilities {

    public class InputBox {

        public  string  Prompt      { get; set; }   = "Enter a value:";
        public  string  Text        { get; set; }   = String.Empty;
        public  string  Title       { get; set; }   = "Enter A Value";

        public  Window? Owner       { get; set; }   = null;


        public  InputBox() {}


        public bool? ShowDialog() {

            bool?               result      = null;

            InputBoxWindow      input;


            try {
                input                           = new InputBoxWindow();
                input.Title                     = Title;
                input.txtPrompt.Text            = Prompt;
                input.txtText.Text              = Text;

                if( Owner != null ) {
                    input.Owner                 = Owner;
                    input.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                }
                else {
                    input.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }

                result                          = input.ShowDialog();

                Text                            = input.txtText.Text;
            }
            catch( Exception ex ) {
                ErrorMessage.Show( ex );
            }

            return result;
        }


        public static bool? Show( Window? owner, string title, string prompt, out string text ) {

            bool?           result  = null;

            InputBox        ib;


            text                    = String.Empty;
            
            try {
                ib          = new InputBox();
                ib.Title    = title;
                ib.Prompt   = prompt;
                ib.Owner    = owner;

                result      = ib.ShowDialog();

                text        = ib.Text;
            }
            catch( Exception ex ) {
                ErrorMessage.Show( ex );
            }

            return result;
        }
        public static bool? Show( string title, string prompt, out string text ) {

            return Show( null, title, prompt, out text );
        }
    }
}