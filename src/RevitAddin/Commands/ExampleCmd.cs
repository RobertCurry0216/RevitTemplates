#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
#endregion

namespace $safeprojectname$.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class ExampleCmd : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;
            // Modify document within a transaction

            using (Transaction tx = new Transaction(doc))
            {
                try
                {
                    tx.Start("Transaction Name");

                    // do stuff

                    tx.Commit();
                }
                catch (Exception e)
                {
                    TaskDialog.Show("Error", e.Message);
                    tx.RollBack();
                    return Result.Failed;
                }
            }
            return Result.Succeeded;
        }
    }
}
