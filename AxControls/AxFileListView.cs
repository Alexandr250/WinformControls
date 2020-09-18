using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WinformControls.AxControls {
    public class AxFileListView : ListView {
        [DllImport( "user32.dll", CharSet = CharSet.Auto )]
        static extern IntPtr SendMessage( IntPtr hWnd, int Msg, int wParam, int lParam );

        private const int WM_CHANGEUISTATE = 0x127;
        private const int UIS_SET = 1;
        private const int UISF_HIDEFOCUS = 0x1;

        private int MakeLong( int wLow, int wHigh ) {
            int low = ( int )IntLoWord( wLow );
            short high = IntLoWord( wHigh );
            int product = 0x10000 * ( int )high;
            int mkLong = ( int )( low | product );
            return mkLong;
        }

        private short IntLoWord( int word ) {
            return ( short )( word & short.MaxValue );
        }

        internal class FileListViewItem : ListViewItem {
            private FileSystemInfo _fileSystemInfo;

            public FileListViewItem( FileSystemInfo fileSystemInfo, bool isParent = false ) {
                _fileSystemInfo = fileSystemInfo;

                string fileSize = string.Empty;
                string fileType = string.Empty;


                if ( _fileSystemInfo is FileInfo fileInfo ) {
                    fileSize = FileSizeToString( fileInfo.Length );
                    fileType = GetMimeType( fileInfo );
                }

                Text = isParent ? ".." : _fileSystemInfo.Name;
                SubItems.Add( isParent ? string.Empty : _fileSystemInfo.LastWriteTime.ToString( "dd.MM.yyyy hh:ss" ) );
                SubItems.Add( fileType );
                SubItems.Add( fileSize );

                
            }
        }
        

        private static string FileSizeToString( long fileSize ) {
            double result = fileSize;
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int pointer = 0;


            if ( result > 1024 ) {
                result /= 1024;
                pointer++;
            }

            if ( result > 1024 ) {
                result /= 1024;
                pointer++;
            }

            if ( result > 1024 ) {
                result /= 1024;
                pointer++;
            }

            if ( result > 1024 ) {
                result /= 1024;
                pointer++;
            }


            return Math.Round( result, 2 ).ToString() + " " + sizes[ pointer ];
        }

        private static string GetMimeType( FileInfo fileInfo ) {
            string mimeType = string.Empty;
            string ext = fileInfo.Extension.ToLower();

            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey( ext );

            if ( regKey != null && regKey.GetValue( "Content Type" ) != null )
                mimeType = regKey.GetValue( "Content Type" ).ToString();

            return mimeType;
        }

        private DirectoryInfo _currentDirectory;
        private static ImageList _imageList = new ImageList() { ImageSize = new Size( 16, 16 ), ColorDepth = ColorDepth.Depth32Bit };

        public DirectoryInfo CurrentDirectory {
            get => _currentDirectory;
            set {
                if ( _currentDirectory == value )
                    return;

                _currentDirectory = value;
            }
        }

        public AxFileListView() {
            SetStyle( ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw, true );

            SendMessage( Handle, WM_CHANGEUISTATE, MakeLong( UIS_SET, UISF_HIDEFOCUS ), 0 );

            

            FullRowSelect = true;
            HideSelection = false;
            GridLines = false;
            View = View.Details;

            SmallImageList = _imageList;

            CurrentDirectory = new DirectoryInfo( Directory.GetCurrentDirectory() );

            Columns.Add( "Имя" );
            Columns[0].Width = 250;

            Columns.Add( "Дата изменения" );
            Columns[0].Width = 150;

            Columns.Add( "Тип" );
            Columns.Add( "Размер" );

            ReloadFileListView();
        }

        private void ReloadFileListView() {
            Items.Clear();

            DirectoryInfo parentDirectory = _currentDirectory.Parent;

            if ( parentDirectory != null ) {
                FileListViewItem item = new FileListViewItem( parentDirectory, true );
                Items.Add( item );
            }


            foreach ( DirectoryInfo directoryInfo in _currentDirectory.EnumerateDirectories().OrderBy( di => di.Name ) ) {
                FileListViewItem item = new FileListViewItem( directoryInfo );
                Items.Add( item );
            }

            foreach ( FileInfo fileInfo in _currentDirectory.EnumerateFiles().OrderBy( fi => fi.Name ) ) {
                FileListViewItem item = new FileListViewItem( fileInfo );
                item.ImageIndex = GetImageIndex( fileInfo );
                Items.Add( item );
            }
        }

        private static int GetImageIndex( FileInfo fileInfo ) {
            //Icon icon = Icon.ExtractAssociatedIcon( fileInfo.FullName );
            Icon icon = GetIconFromShell( fileInfo.FullName );
            _imageList.Images.Add( fileInfo.Extension, icon );

            return _imageList.Images.Count - 1;
        }

        

        private static Icon GetIconFromShell( string ext, bool isLargeIcon = false ) {
            SHFILEINFO fileInfo = new SHFILEINFO();
            if ( !isLargeIcon )
                SHGetFileInfo( ext, FileAttributes.FILE_ATTRIBUTE_NORMAL, out fileInfo,
                    ( uint )20, ShellFileInfoFlags.SHGFI_USEFILEATTRIBUTES | ShellFileInfoFlags.SHGFI_ICON | ShellFileInfoFlags.SHGFI_SMALLICON );
            else
                SHGetFileInfo( ext, FileAttributes.FILE_ATTRIBUTE_NORMAL, out fileInfo,
                    ( uint )20, ShellFileInfoFlags.SHGFI_USEFILEATTRIBUTES | ShellFileInfoFlags.SHGFI_ICON );

            if ( fileInfo.hIcon == ( IntPtr )0 )
                return null;

            return Icon.FromHandle( fileInfo.hIcon );
        }

        protected override void OnHandleCreated( EventArgs e ) {
            base.OnHandleCreated( e );
            SetWindowTheme( Handle, "explorer", null );
        }

        private struct SHFILEINFO {
            public IntPtr hIcon;             // дескриптор системной иконки.
            public int iIcon;                // индекс системной иконки в коллекции системных иконок.
            public uint dwAttributes;        // атрибуты файла.
            public string szDisplayName;     // отображаемое имя файла.
            public string szTypeName;        // наименование типа файла.
        }

        public enum FileAttributes {
            FILE_ATTRIBUTE_DIRECTORY = 0x00000010,     // каталог.
            FILE_ATTRIBUTE_NORMAL = 0x00000080      // файл.
        }

        public enum ShellFileInfoFlags {
            SHGFI_ATTRIBUTES = 0x00000800,
            SHGFI_ATTR_SPECIFIED = 0x00020000,
            SHGFI_DISPLAYNAME = 0x00000200,     // возвращать отображаемое имя файла.
            SHGFI_EXETYPE = 0x00002000,
            SHGFI_ICON = 0x00000100,     // возвращать иконку.
            SHGFI_ICONLOCATION = 0x00001000,
            SHGFI_LARGEICON = 0x00000000,
            SHGFI_LINKOVERLAY = 0x00008000,
            SHGFI_OPENICON = 0x00000002,
            SHGFI_PIDL = 0x00000008,
            SHGFI_SELECTED = 0x00010000,
            SHGFI_SHELLICONSIZE = 0x00000004,
            SHGFI_SMALLICON = 0x00000001,     // возвращать маленькую иконку файла.
            SHGFI_SYSICONINDEX = 0x00004000,     // возвращать индекс в коллекции системных иконок.
            SHGFI_TYPENAME = 0x00000400,     // возвращать наименование типа файла.
            SHGFI_USEFILEATTRIBUTES = 0x00000010
        }

        [DllImport( "uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true )]
        internal static extern int SetWindowTheme( IntPtr hWnd, string appName, string partList );

        [DllImport( "Shell32.dll" )]
        private static extern IntPtr SHGetFileInfo( string drivePath, FileAttributes attributes, out SHFILEINFO fileInfo,
                        uint countBytesFileInfo, ShellFileInfoFlags flags );

    }
}
