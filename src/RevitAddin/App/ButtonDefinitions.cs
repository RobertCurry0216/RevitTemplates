using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.App
{

    public partial class AppLayout
    {
        public const string DefaultDll = "$safeprojectname$.dll";

        public struct ButtonInfo
        {
            // Name and Command are the only required parameters
            // everything else will be filled with default values if left null

            public string Name; // Internal revit name, should be unique, Required
            public string Command; // Revit command full class name, Required

            public string Text; // Text that shows up below the button
            public string ToolTip;
            public string ToolTipImage;
            public string LongDescription; // ToolTip displayed when you hover over the button
            public string Icon; // image resource
            public string SmallIcon; // image resource
            public string Availability; // Availability full class nam

            // only set one of these parameters
            public string HelpUrl; 
            public string HelpFile;

            // only set these if they differ from the defaults
            public string Dll;

            // Use the helper methods StackGroup and PulldownGroup
            // rather than setting these directly
            public IEnumerable<ButtonInfo> StackedList;
            public IEnumerable<ButtonInfo> PulldownList;
        }

        // Add button info here
        private ButtonInfo button1 = new ButtonInfo
        {
            Name = "btn_ExampleCmd",
            Text = "New",
            Command = "$safeprojectname$.Commands.ExampleCmd",
        };
    }
}
