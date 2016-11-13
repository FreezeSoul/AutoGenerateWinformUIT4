using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using DevExpress.XtraNavBar;
using System.Drawing;

namespace UILogic
{
    public class MenuHelper
    {
        private IMenuActionBehavior _menuActionBehavior;

        public MenuHelper(IMenuActionBehavior menuActionBehavior)
        {
            this._menuActionBehavior = menuActionBehavior;
        }

        public void InitMenuTree(NavBarControl navBarControl, string xmlPath)
        {
            var doc = XDocument.Load(xmlPath);
            var menuGroups = doc.Root.Elements("MenuGroup");
            var navBarGroupList = new List<NavBarGroup>();
            if (doc.Root.Attribute("IsClear").Value.ToLower().Equals("true"))
            {
                navBarControl.Items.Clear();
                navBarControl.Groups.Clear();
            }
            menuGroups.ToList().ForEach(elementGroup =>
                                            {
                                                NavBarGroup navBarGroup = null;
                                                var groupEnumerator = navBarControl.Groups.GetEnumerator();
                                                while (groupEnumerator.MoveNext())
                                                {
                                                    var navBarGroupTemp = groupEnumerator.Current as NavBarGroup;
                                                    if (navBarGroupTemp.Name == elementGroup.Attribute("Name").Value)
                                                    {
                                                        navBarGroup = navBarGroupTemp;
                                                        break;
                                                    }
                                                }
                                                if (navBarGroup == null)
                                                {
                                                    navBarGroup = new NavBarGroup()
                                                                          {
                                                                              Name = elementGroup.Attribute("Name").Value,
                                                                              Caption = elementGroup.Attribute("Describe").Value,
                                                                              Expanded = elementGroup.Attribute("Expanded") == null ? false : (elementGroup.Attribute("Expanded").Value.ToLower().Equals("true") ? true : false)
                                                                          };
                                                    navBarGroupList.Add(navBarGroup);
                                                }
                                                var menuItems = elementGroup.Elements("MenuItem").Where(item => item.Attribute("IsShowing").Value.ToLower().Equals("true"));
                                                menuItems.ToList().ForEach(elementItem =>
                                                                               {
                                                                                   NavBarItem navBarItem = null;
                                                                                   var itemEnumerator = navBarControl.Items.GetEnumerator();
                                                                                   while (itemEnumerator.MoveNext())
                                                                                   {
                                                                                       var navBarItemTemp = itemEnumerator.Current as NavBarItem;
                                                                                       if (navBarItemTemp.Name == elementItem.Attribute("Name").Value)
                                                                                       {
                                                                                           navBarItem = navBarItemTemp;
                                                                                           break;
                                                                                       }
                                                                                   }
                                                                                   if (navBarItem == null)
                                                                                   {
                                                                                       navBarItem = new NavBarItem()
                                                                                       {
                                                                                           Name = elementItem.Attribute("Name").Value,
                                                                                           Caption = elementItem.Attribute("Describe").Value,
                                                                                           //SmallImage = Image.FromFile(elementItem.Attribute("IconPath").Value),
                                                                                           Tag = new MenuActionArgs
                                                                                                     {
                                                                                                         Type = elementItem.Attribute("Type").Value,
                                                                                                         NameSpace = elementItem.Attribute("NameSpace").Value,
                                                                                                         ClassName = elementItem.Attribute("ClassName").Value,
                                                                                                         Describe = elementItem.Attribute("Describe").Value
                                                                                                     }
                                                                                       };
                                                                                       navBarItem.LinkClicked += NavBarItemLinkClicked;
                                                                                       var navBarItemLink = new NavBarItemLink(navBarItem);
                                                                                       navBarControl.Items.Add(navBarItem);
                                                                                       navBarGroup.ItemLinks.Add(navBarItemLink);
                                                                                   }
                                                                               });
                                            });
            navBarControl.Groups.AddRange(navBarGroupList.ToArray());
        }

        public void NavBarItemLinkClicked(object sender, NavBarLinkEventArgs e)
        {
            var menuActionArg = (sender as NavBarItem).Tag as MenuActionArgs;
            if (this._menuActionBehavior != null)
                this._menuActionBehavior.InvokeMethod.Invoke(menuActionArg);
        }
    }
}