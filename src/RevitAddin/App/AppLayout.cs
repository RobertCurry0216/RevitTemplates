using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.App
{
    public partial class AppLayout
    {
        public IEnumerable<TabInfo> Get()
        {
            return new[]
            {
                new TabInfo
                {
                    Name = "New Tab",
                    Panels = new[]
                    {
                        new PanelInfo
                        {
                            Name = "New panel",
                            Buttons = new[]
                            {
                                button1,
                            },
                        },
                    },
                },

            };
        }

        // helpers
        public struct TabInfo
        {
            public string Name;
            public IEnumerable<PanelInfo> Panels;
        }

        public struct PanelInfo
        {
            public string Name;
            public IEnumerable<ButtonInfo> Buttons;
            public IEnumerable<ButtonInfo> SlideOut;
        }

        // place in button list to add seperator
        static ButtonInfo Separator = new ButtonInfo();

        private static ButtonInfo PulldownGroup(params ButtonInfo[] buttons)
        {
            return new ButtonInfo
            {
                Name = $"pdbtn_{buttons.First().Name}",
                PulldownList = buttons,
            };
        }

        private static ButtonInfo StackGroup(params ButtonInfo[] buttons)
        {
            if (buttons.Count() > 3 || buttons.Count() <= 1) throw new ArgumentException($"Buttons.StackGroup: must have 2 or 3 arguments, got={buttons.Count()}"); 
            return new ButtonInfo
            {
                Name = $"stkbtn_{buttons.First().Name}",
                StackedList = buttons,
            };
        }

    }
}
