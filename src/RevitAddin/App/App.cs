#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using static $safeprojectname$.App.AppLayout;
#endregion

namespace $safeprojectname$.App
{
    // You shouldn't need to modify this class
    // Add new buttons in ButtonDefinitions.cs
    // Configure the panel layout in AppLayout.cs


    class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication a)
        {
            OnTabCreate(a);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }

        public RibbonPanel RibbonPanel(UIControlledApplication app, string tab, string panelName)
        {

            // Empty ribbon panel
            RibbonPanel ribbonPanel = null;
            // Try to create ribbon tab.
            try
            {
                app.CreateRibbonTab(tab);
            }
            catch { }
            // Try to create ribbon panel.
            try
            {
                if (tab == "Add-ins")
                {
                    return app.CreateRibbonPanel(Tab.AddIns, panelName);
                }
                else if (tab == "Analyze")
                {
                    return app.CreateRibbonPanel(Tab.Analyze, panelName);
                }
                else
                {
                    return app.CreateRibbonPanel(tab, panelName);
                }

            }
            catch { }
            // Search existing tab for your panel.
            List<RibbonPanel> panels = app.GetRibbonPanels(tab);
            foreach (RibbonPanel p in panels)
            {
                if (p.Name == panelName)
                {
                    ribbonPanel = p;
                }
            }

            //return panel
            return ribbonPanel;
        }

        private void OnTabCreate(UIControlledApplication application)
        {
            var appLayout = new AppLayout();

            foreach (var tab in appLayout.Get())
            {
                OnPanelCreate(application, tab);
            }
        }

        private void OnPanelCreate(UIControlledApplication application, TabInfo tab)
        {
            foreach (var panel in tab.Panels)
            {
                var ribbonPanel = RibbonPanel(application, tab.Name, panel.Name);
                OnButtonCreate(application, ribbonPanel, panel);
            }
        }

        private void OnButtonCreate(UIControlledApplication application, RibbonPanel ribbonPanel, PanelInfo panel)
        {
            string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            foreach (var button in panel.Buttons)
            {
                buttonFactory(ribbonPanel, executableLocation, button);
            }

            var slideout = panel.SlideOut;

            if (slideout == null || slideout.Count() == 0) return;

            ribbonPanel.AddSlideOut();

            foreach (var button in slideout)
            {
                buttonFactory(ribbonPanel, executableLocation, button);
            }
        }

        private void buttonFactory(RibbonPanel ribbon, string executableLocation, ButtonInfo button)
        {
            if (button.Name == null && button.Command == null)
            {
                ribbon.AddSeparator();
            }
            else if (button.StackedList != null)
            {
                AddStackButton(ribbon, executableLocation, button);
            }
            else if (button.PulldownList != null)
            {
                AddPulldownButton(ribbon, executableLocation, button);
            }
            else
            {
                AddButton(ribbon, executableLocation, button);
            }
        }

        private static PushButtonData GetButtonData(string dllLocation, ButtonInfo button)
        {
            var name = button.Name;
            var text = button.Text ?? "new tool";
            var command = button.Command;

            PushButtonData buttondata = new PushButtonData(
                            name,
                            text,
                            dllLocation,
                            command
                            );

            if (button.ToolTip != null) buttondata.ToolTip = button.ToolTip;
            if (button.LongDescription != null) buttondata.LongDescription = button.LongDescription;
            if (button.Availability != null) buttondata.AvailabilityClassName = button.Availability;

            BitmapImage image;
            var icon = button.Icon ?? "icon.png";
            image = new BitmapImage(new Uri($"pack://application:,,,/$safeprojectname$;component/Resources/{icon}"));
            buttondata.LargeImage = image;

            icon = button.SmallIcon ?? "icon_small.png";
            image = new BitmapImage(new Uri($"pack://application:,,,/$safeprojectname$;component/Resources/{icon}"));
            buttondata.Image = image;

            if (button.ToolTipImage != null)
            {
                image = new BitmapImage(new Uri($"pack://application:,,,/$safeprojectname$;component/Resources/{button.ToolTipImage}"));
                buttondata.ToolTipImage = image;
            }


            if (button.HelpUrl != null) buttondata.SetContextualHelp(new ContextualHelp(ContextualHelpType.Url, button.HelpUrl));            
            if (button.HelpFile != null) buttondata.SetContextualHelp(new ContextualHelp(ContextualHelpType.ChmFile, button.HelpFile));            

            return buttondata;
        }

        private void AddButton(RibbonPanel ribbon, string executableLocation, ButtonInfo button)
        {
            string dllLocation = Path.Combine(executableLocation, button.Dll ?? AppLayout.DefaultDll);

            var buttondata = GetButtonData(dllLocation, button);

            _ = ribbon.AddItem(buttondata) as PushButton;
        }

        private void AddStackButton(RibbonPanel ribbon, string executableLocation, ButtonInfo button)
        {

            string dllLocation = Path.Combine(executableLocation, button.Dll ?? AppLayout.DefaultDll);

            var stackButtons = button.StackedList.Select(b => GetButtonData(dllLocation, b)).ToList();

            if (stackButtons.Count == 2)
            {
                ribbon.AddStackedItems(stackButtons[0], stackButtons[1]);
            }
            else if (stackButtons.Count == 3)
            {
                ribbon.AddStackedItems(stackButtons[0], stackButtons[1], stackButtons[2]);
            }
        }

        private void AddPulldownButton(RibbonPanel ribbon, string executableLocation, ButtonInfo button)
        {
            string dllLocation = Path.Combine(executableLocation, button.Dll ?? AppLayout.DefaultDll);

            var pulldownButtons = button.PulldownList.Select(b => GetButtonData(dllLocation, b));

            var sbData = new SplitButtonData(button.Name, button.Text ?? "pulldown");
            var sb = ribbon.AddItem(sbData) as PulldownButton;

            foreach (var b in pulldownButtons)
            {
                sb.AddPushButton(b);
            }
        }

    }
}
