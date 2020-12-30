using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace LenWeaver.Utilities {

    public class PrintPreview {

        public  Window?             Owner       { get; set; }       = null;

        public  FlowDocument?       Document    { get; set; }       = null;


        public PrintPreview() {}


        public  bool?   ShowDialog() {

            bool?                   result;

            PrintPreviewWindow      preview;


            try {
                preview                 = new PrintPreviewWindow();
                preview.Owner           = Owner;
                preview.fdr.Document    = Document;

                result                  = preview.ShowDialog();
            }
            catch( Exception ex ) {
                throw new ApplicationException( "Unable to execute print preview.", ex );
            }

            return result;
        }
    }
}