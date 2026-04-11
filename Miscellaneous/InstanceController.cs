
//I haven't found a way to make an instance controller that will work
//on .net core and be portable.

//using System;
//using System.Collections.Generic;
//using System.IO.Pipes;
//using System.Text;
//using System.Threading;

//namespace LenWeaver.Utilities {


//    public class InstanceController {

//        public delegate void InstanceControllerDelegate( InstanceControllerEventArgs e );


//        private             Mutex                       mutex;
//        private             NamedPipeClientStream?      clientStream    = null;
//        private             NamedPipeServerStream?      serverStream    = null;

//        private     event   InstanceControllerDelegate? receivedEvent   = null;

//        public      int                                 InstanceID      { get; private set; }       = -1;
//        public      string                              ApplicationID   { get; }



//        public InstanceController( string applicationID, string serverMachineName ) {


//            ApplicationID   = applicationID;

//            if( Mutex.TryOpenExisting( ApplicationID, out Mutex? result ) ) {
//                //This instance is not the first/only instance.
//                mutex           = result;
//                serverStream    = new NamedPipeServerStream( applicationID,
//                                                             PipeDirection.InOut,
//                                                             maxNumberOfServerInstances: 1,
//                                                             PipeTransmissionMode.Byte,
//                                                             PipeOptions.WriteThrough );

//                serverStream.BeginWaitForConnection( ConnectionResultCallback, null );
//            }
//            else {
//                mutex           = new Mutex( initiallyOwned:false, applicationID, out bool created );
//                clientStream    = new NamedPipeClientStream( serverMachineName,
//                                                             applicationID,
//                                                             PipeDirection.InOut,
//                                                             PipeOptions.WriteThrough );
//            }
//        }
//        public InstanceController( string applicationID ) : this( applicationID, "." ) {}


//        private void ConnectionResultCallback( IAsyncResult result ) {

//            System.Diagnostics.Debug.WriteLine( "ConnectionCallback called." );
//        }


//        public event InstanceControllerDelegate Receive {
//            add     => receivedEvent += value;
//            remove  => receivedEvent -= value;
//        }

//        public void Send( string data ) {

//            Send( ASCIIEncoding.UTF8.GetBytes( data) );
//        }
//        public void Send( Byte[] data ) {

//            try {
//                if( clientStream != null ) {
//                    clientStream.Write( data, offset: 0, count: data.Length );
//                }
//                else if( serverStream != null ) {
//                    serverStream.Write( data, offset: 0, count: data.Length );
//                }
//                else {
//                    throw new ArgumentNullException( "ClientStream is null." );
//                }
//            }
//            catch( Exception ex ) {
//                throw new InvalidOperationException( "Unable to send data to server.", ex );
//            }
//        }
//    }
//}