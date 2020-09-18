using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace WinformControls.AxControls.Designer {
    [PermissionSet( SecurityAction.Demand, Name = "FullTrust" )]
    public class IntEditor : UITypeEditor {
        public virtual bool IsDropDownResizable {
            get {
                return true;
            }
        }

        public override UITypeEditorEditStyle GetEditStyle( ITypeDescriptorContext context ) {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue( ITypeDescriptorContext context, IServiceProvider provider, object value ) {
            if ( value.GetType() != typeof( int ) && value.GetType() != typeof( byte ) )
                return value;

            Control instance = null;
            if ( context.Instance is Control )
                instance = context.Instance as Control;

            IWindowsFormsEditorService editor = ( IWindowsFormsEditorService ) provider.GetService( typeof( IWindowsFormsEditorService ) );
            if ( editor != null ) {

                int v = 0;

                if ( value.GetType() == typeof( int ) )
                    v = ( int ) value;
                if ( value.GetType() == typeof( byte ) )
                    v = ( byte ) value;

                IntControl intControl = new IntControl( v );

                if ( instance != null )
                    intControl.MaxValue = Math.Min( instance.Width, instance.Height ) / 2;

                editor.DropDownControl( intControl );

                if ( value.GetType() == typeof( int ) )
                    return intControl.Value;
                if ( value.GetType() == typeof( byte ) )
                    return ( byte ) intControl.Value;
            }
            return value;
        }
    }
}
