using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Helpers
{
    class AllowAllSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem) => true;

        public bool AllowReference(Reference reference, XYZ position) => false;
    }
}
