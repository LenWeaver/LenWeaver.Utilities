using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;

namespace LenWeaver.Utilities {

    public class XmlSettings {

        private string                  _filename;

        private XmlCommentCollection?   _commentsList   = null;
        private XmlSectionList?         _sectionList    = null;


        public XmlSettings( string filename ) {

            _filename = filename;

            if( _filename.Length > 0 ) {
                Load();
            }
        }
        public XmlSettings() : this( String.Empty ) { }


        public string Filename {
            get { return _filename; }
            set { _filename = value; }
        }

        public XmlCommentCollection Comments {
            get {
                if( _commentsList == null ) {
                    _commentsList = new XmlCommentCollection();
                }

                return _commentsList;
            }
        }
        public XmlSectionList Sections {
            get {
                if( _sectionList == null ) {
                    _sectionList = new XmlSectionList();
                }

                return _sectionList;
            }
        }

        public void Load() {

            XmlAttribute    attr;
            XmlDocument     xmlDoc;
            XmlNodeList     sections;

            XmlSection      section;


            try {
                _commentsList   = new XmlCommentCollection();
                _sectionList    = new XmlSectionList();

                if( File.Exists( _filename ) ) {
                    xmlDoc = new XmlDocument();
                    xmlDoc.Load( _filename );

                    foreach( XmlNode? node in xmlDoc.ChildNodes ) {
                        if( node != null ) { 
                            if( node.NodeType == XmlNodeType.Comment ) {
                                _commentsList.Add( node.Value );
                            }
                        }
                    }

                    sections = xmlDoc.SelectNodes( "//settings/section" );
                    if( sections.Count > 0 ) {
                        foreach( XmlNode? node in sections ) {
                            attr = node!.Attributes["name"];
                            if( attr != null ) {
                                section = _sectionList[attr.Value];

                                ProcessXmlSection( section, node );
                            }
                        }
                    }
                }
            }
            catch( Exception ex ) {
                throw new ApplicationException( "Unable to load xml document.", ex );
            }
        }
        public void Save() {

            XmlAttribute    attribute;
            XmlDocument     xmlDoc;
            XmlElement      keyElement;
            XmlElement      root;
            XmlElement      sectionElement;


            try {
                xmlDoc = new XmlDocument();

                xmlDoc.AppendChild( xmlDoc.CreateXmlDeclaration( "1.0", null, null ) );

                if( _commentsList != null ) {
                    foreach( string s in _commentsList ) {
                        xmlDoc.AppendChild( xmlDoc.CreateComment( s ) );
                    }
                }

                root = xmlDoc.CreateElement( "settings" );

                foreach( XmlSection section in Sections ) {
                    if( section.Keys.Count > 0 ) {
                        sectionElement = xmlDoc.CreateElement( "section" );

                        attribute = xmlDoc.CreateAttribute( "name" );
                        attribute.Value = section.Name;
                        sectionElement.Attributes.Append( attribute );

                        foreach( string s in section.Comments ) {
                            sectionElement.AppendChild( xmlDoc.CreateComment( s ) );
                        }

                        foreach( XmlKey key in section.Keys ) {
                            keyElement = xmlDoc.CreateElement( "key" );

                            attribute = xmlDoc.CreateAttribute( "name" );
                            attribute.Value = key.Name;
                            keyElement.Attributes.Append( attribute );

                            attribute = xmlDoc.CreateAttribute( "value" );
                            attribute.Value = key.Value;
                            keyElement.Attributes.Append( attribute );

                            attribute = xmlDoc.CreateAttribute( "type" );
                            attribute.Value = key.Type;
                            keyElement.Attributes.Append( attribute );

                            sectionElement.AppendChild( keyElement );
                        }

                        root.AppendChild( sectionElement );
                    }
                }

                xmlDoc.AppendChild( root );

                xmlDoc.Save( _filename );
            }
            catch( Exception ex ) {
                throw new ApplicationException( "Unable to save xml document.", ex );
            }
        }

        public XmlKey?   GetValue( string sectionName, string keyName ) {

            XmlKey?     result      = null;

            XmlSection  section;


            section = Sections[sectionName];
            if( section != null ) {
                result = section.Keys[keyName];
            }

            return result;
        }

        public bool     GetValue( string sectionName, string keyName, bool defaultValue ) {

            bool        result      = defaultValue;

            XmlSection  section;


            section = Sections[sectionName];
            if( section != null ) {
                result = section.GetValue( keyName, defaultValue );
            }

            return result;
        }
        public int      GetValue( string sectionName, string keyName, int defaultValue ) {

            int         result      = defaultValue;

            XmlSection  section;


            section = Sections[sectionName];
            if( section != null ) {
                result = section.GetValue( keyName, defaultValue );
            }

            return result;
        }
        public long     GetValue( string sectionName, string keyName, long defaultValue ) {

            long        result      = defaultValue;

            XmlSection  section;


            section = Sections[sectionName];
            if( section != null ) {
                result = section.GetValue( keyName, defaultValue );
            }

            return result;
        }
        public float    GetValue( string sectionName, string keyName, float defaultValue ) {

            float       result      = defaultValue;

            XmlSection  section;


            section = Sections[sectionName];
            if( section != null ) {
                result = section.GetValue( keyName, defaultValue );
            }

            return result;
        }
        public double   GetValue( string sectionName, string keyName, double defaultValue ) {

            double      result      = defaultValue;

            XmlSection  section;


            section = Sections[sectionName];
            if( section != null ) {
                result = section.GetValue( keyName, defaultValue );
            }

            return result;
        }
        public decimal  GetValue( string sectionName, string keyName, decimal defaultValue ) {

            decimal     result      = defaultValue;

            XmlSection  section;


            section = Sections[sectionName];
            if( section != null ) {
                result = section.GetValue( keyName, defaultValue );
            }

            return result;
        }
        public string   GetValue( string sectionName, string keyName, string defaultValue ) {

            string      result      = defaultValue;

            XmlSection  section;


            section = Sections[sectionName];
            if( section != null ) {
                result = section.GetValue( keyName, defaultValue );
            }

            return result;
        }
        public DateTime GetValue( string sectionName, string keyName, DateTime defaultValue ) {

            DateTime    result      = defaultValue;

            XmlSection  section;


            section = Sections[sectionName];
            if( section != null ) {
                result = section.GetValue( keyName, defaultValue );
            }

            return result;
        }

        public T GetValue<T>( string sectionName, string keyName, T defaultValue ) {

            T           result      = defaultValue;

            XmlSection  section;


            section = Sections[sectionName];
            if( section != null ) {
                result = section.GetValue( keyName, defaultValue );
            }

            return result;
        }

        public void SetValue( string sectionName, XmlKey key ) {

            XmlSection  section;


            section = Sections[sectionName];
            section.SetValue( key );
        }

        private void ProcessXmlSection( XmlSection section, XmlNode node ) {

            string          name;
            string          type;
            string          value;

            XmlAttribute    attr;


            foreach( XmlNode? n in node.ChildNodes ) {
                if( n != null ) { 
                    if( n.NodeType == XmlNodeType.Comment ) {
                        section.Comments.Add( n.Value );
                    }
                }
            }

            foreach( XmlNode? n in node.SelectNodes( "key" ) ) {
                if( n != null ) { 
                    name    = n.Attributes["name"].Value;
                    value   = n.Attributes["value"].Value;

                    attr    = n.Attributes["type"];
                    type    = attr != null ? attr.Value : "String";

                    section.Keys.Add( name, value, type );
                }
            }
        }
    }
}