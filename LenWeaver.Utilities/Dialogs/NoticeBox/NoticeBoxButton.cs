using System;
using System.Windows;

namespace LenWeaver.Utilities {

    public class NoticeBoxButton : IComparable, IComparable<NoticeBoxButton> {

        public bool                     IsCancel            { get; internal set; }
        public bool                     IsDefault           { get; internal set; }

        public NoticeBoxResult          Result              { get; init; }

        public string                   Text                { get; init; }

        public object?                  Tag                 { get; set; }               = null;


        #region Static Properties
        private static NoticeBoxButton? cancel              = null;
        private static NoticeBoxButton? no                  = null;
        private static NoticeBoxButton? none                = null;
        private static NoticeBoxButton? ok                  = null;
        private static NoticeBoxButton? yes                 = null;

        public static NoticeBoxButton Cancel {
            get {
                cancel              = new NoticeBoxButton( "Cancel", NoticeBoxResult.Cancel );

                cancel.IsCancel     = true;
                cancel.IsDefault    = false;


                return cancel;
            }
        }
        public static NoticeBoxButton No {
            get {
                no                  = new NoticeBoxButton( "No", NoticeBoxResult.No );

                no.IsCancel         = true;
                no.IsDefault        = false;

                return no;
            }
        }
        public static NoticeBoxButton None {
            get {
                none                = new NoticeBoxButton( "None", NoticeBoxResult.None );

                none.IsCancel       = false;
                none.IsDefault      = false;

                return none;
            }
        }
        public static NoticeBoxButton OK {
            get {
                ok                  = new NoticeBoxButton( "Ok", NoticeBoxResult.OK );

                ok.IsCancel         = false;
                ok.IsDefault        = true;

                return ok;
            }
        }
        public static NoticeBoxButton Yes {
            get {
                yes                 = new NoticeBoxButton( "Yes", NoticeBoxResult.Yes );

                yes.IsCancel        = false;
                yes.IsDefault       = true;

                return yes;
            }
        }

        public static NoticeBoxButton[] OKCancel            = { OK, Cancel };
        public static NoticeBoxButton[] YesNo               = { Yes, No };
        public static NoticeBoxButton[] YesNoCancel         = { Yes, No, Cancel };
        #endregion


        public NoticeBoxButton( string text ) : this( text, NoticeBoxResult.OK, false, false ) {}
        public NoticeBoxButton( string text, NoticeBoxResult result ) : this( text, result, false, false ) {}

        internal NoticeBoxButton( string text, NoticeBoxResult result, bool isDefault, bool isCancel ) {

            Text            = text;
            Result          = result;
            IsCancel        = isCancel;
            IsDefault       = isDefault;
        }


        public override string ToString() {

            return Text;
        }


        #region IComparable Implementation
        public int CompareTo( NoticeBoxButton? other ) {

            if( other is null ) throw new ArgumentNullException( nameof(other) );

            return Result.CompareTo( other.Result );
        }
        public int CompareTo( object? obj ) {

            return CompareTo( (NoticeBoxButton?)obj );
        }
        #endregion
    }
}