using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LenWeaver.Utilities {

    public class AboutBox {

        public  string?     Company                 { get; set; }
        public  string?     Copyright               { get; set; }
        public  string?     ProductDescription      { get; set; }
        public  string?     ProductName             { get; set; }

        public  Uri?        UpperLeft               { get; set; }   = null;
        public  Uri?        UpperRight              { get; set; }   = null;
        public  Uri?        LowerLeft               { get; set; }   = null;
        public  Uri?        LowerRight              { get; set; }   = null;

        public  Assembly?   SourceAssembly          { get; set; }
        public  Version?    ProductVersion          { get; set; }
        public  Window?     Owner                   { get; set; }


        public AboutBox() {}


        public bool? ShowDialog() {

            bool?                           result                  = null;

            AboutBoxWindow                  aboutBox;

            AssemblyCompanyAttribute?       companyAttribute;
            AssemblyCopyrightAttribute?     copyrightAttribute;
            AssemblyDescriptionAttribute?   descriptionAttribute;
            AssemblyProductAttribute?       productAttribute;
            AssemblyVersionAttribute?       versionAttribute;


            try {
                aboutBox                                = new AboutBoxWindow();

                if( SourceAssembly != null ) {
                    companyAttribute                    = SourceAssembly.GetCustomAttribute<AssemblyCompanyAttribute>();
                    copyrightAttribute                  = SourceAssembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
                    descriptionAttribute                = SourceAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>();
                    productAttribute                    = SourceAssembly.GetCustomAttribute<AssemblyProductAttribute>();
                    versionAttribute                    = SourceAssembly.GetCustomAttribute<AssemblyVersionAttribute>();

                    Company                             = companyAttribute      == null ? String.Empty  : companyAttribute.Company;
                    Copyright                           = copyrightAttribute    == null ? String.Empty  : copyrightAttribute.Copyright;
                    ProductDescription                  = descriptionAttribute  == null ? String.Empty  : descriptionAttribute.Description;
                    ProductName                         = productAttribute      == null ? String.Empty  : productAttribute.Product;
                    ProductVersion                      = versionAttribute      == null ? SourceAssembly.GetName().Version : Version.Parse( versionAttribute.Version );
                }

                aboutBox.lblProductName.Content         = ProductName;
                aboutBox.lblVersion.Content             = ProductVersion?.ToString() ?? null;
                aboutBox.lblCopyright.Content           = Copyright;
                aboutBox.lblCompany.Content             = $"By {Company}";

                aboutBox.tbProductDescription.Text      = ProductDescription;

                if( LowerLeft   != null )               aboutBox.imgLowerLeft.Source    = new BitmapImage( LowerLeft );
                if( LowerRight  != null )               aboutBox.imgLowerRight.Source   = new BitmapImage( LowerRight );
                if( UpperLeft   != null )               aboutBox.imgUpperLeft.Source    = new BitmapImage( UpperLeft );
                if( UpperRight  != null )               aboutBox.imgUpperRight.Source   = new BitmapImage( UpperRight );

                if( Owner != null ) {
                    aboutBox.Owner                      = Owner;
                    aboutBox.WindowStartupLocation      = WindowStartupLocation.CenterOwner;
                }
                else {
                    aboutBox.WindowStartupLocation      = WindowStartupLocation.CenterScreen;
                }

                result                                  = aboutBox.ShowDialog();
            }
            catch( Exception ex ) {
                throw new ApplicationException( "Unable to display AboutBox.", ex );
            }

            return result;
        }
    }
}