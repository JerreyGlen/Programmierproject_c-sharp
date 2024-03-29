﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using stdole;
using Inventor;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Compatibility.VB6;

namespace AddIn_Client
{
    public class InventorButton
    {
        private ButtonDefinition mButtonDef;
        public ButtonDefinition ButtonDef
        {
            get { return mButtonDef; }
            set { mButtonDef = value; }
        }


        /// <summary>
        /// The most comprehensive signature.
        /// </summary>
        public InventorButton(string displayName, string internalName, string description, string tooltip,
                                Icon standardIcon, Icon largeIcon,
                                CommandTypesEnum commandType, ButtonDisplayEnum buttonDisplayType)
        {
            Create(displayName, internalName, description, tooltip, AddinGlobal.ClassId,
                standardIcon, largeIcon, commandType, buttonDisplayType);
        }

        /// <summary>
        /// The signature does not care about Command Type (always editing) and Button Display (always with text).
        /// </summary>
        public InventorButton(string displayName, string internalName, string description, string tooltip,
                                Icon standardIcon, Icon largeIcon)
        {
            Create(displayName, internalName, description, tooltip, AddinGlobal.ClassId,
                null, null, CommandTypesEnum.kEditMaskCmdType, ButtonDisplayEnum.kAlwaysDisplayText);
        }

        /// <summary>
        /// The signature does not care about icons.
        /// </summary>
        public InventorButton(string displayName, string internalName, string description, string tooltip,
                                CommandTypesEnum commandType, ButtonDisplayEnum buttonDisplayType)
        {
            Create(displayName, internalName, description, tooltip, AddinGlobal.ClassId,
                null, null, commandType, buttonDisplayType);
        }

        /// <summary>
        /// This signature only cares about display name and icons.
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="standardIcon"></param>
        /// <param name="largeIcon"></param>
        public InventorButton(string displayName, Icon standardIcon, Icon largeIcon)
        {
            Create(displayName, displayName, displayName, displayName, AddinGlobal.ClassId,
                standardIcon, largeIcon, CommandTypesEnum.kEditMaskCmdType, ButtonDisplayEnum.kAlwaysDisplayText);
        }

        /// <summary>
        /// The simplest signature, which can be good for prototyping.
        /// </summary>
        public InventorButton(string displayName)
        {
            Create(displayName, displayName, displayName, displayName, AddinGlobal.ClassId,
                    null, null, CommandTypesEnum.kEditMaskCmdType, ButtonDisplayEnum.kAlwaysDisplayText);
        }

        /// <summary>
        /// The helper method for constructors to call to avoid duplicate code.
        /// </summary>
        public void Create(
            string displayName, string internalName, string description, string tooltip,
            string clientId,
            Icon standardIcon, Icon largeIcon,
            CommandTypesEnum commandType, ButtonDisplayEnum buttonDisplayType)
        {
            if (string.IsNullOrEmpty(clientId))
                clientId = AddinGlobal.ClassId;

            stdole.IPictureDisp standardIconIPictureDisp = null;
            stdole.IPictureDisp largeIconIPictureDisp = null;
            if (standardIcon != null)
            {
                standardIconIPictureDisp = Support.IconToIPicture(standardIcon) as stdole.IPictureDisp;
                largeIconIPictureDisp = (stdole.IPictureDisp)Support.IconToIPicture(largeIcon);
            }

            mButtonDef = AddinGlobal.InventorApp.CommandManager.ControlDefinitions.AddButtonDefinition(
                displayName, internalName, commandType,
                clientId, description, tooltip,
                standardIconIPictureDisp, largeIconIPictureDisp, buttonDisplayType);

            mButtonDef.Enabled = true;
            mButtonDef.OnExecute += ButtonDefinition_OnExecute;

            DisplayText = true;

            AddinGlobal.ButtonList.Add(this);
        }


        public bool DisplayBigIcon { get; set; }
        public bool DisplayText { get; set; }
        public bool InsertBeforeTarget { get; set; }

        public void SetBehavior(bool displayBigIcon, bool displayText, bool insertBeforeTarget)
        {
            DisplayBigIcon = displayBigIcon;
            DisplayText = displayText;
            InsertBeforeTarget = insertBeforeTarget;
        }

        public void CopyBehaviorFrom(InventorButton button)
        {
            this.DisplayBigIcon = button.DisplayBigIcon;
            this.DisplayText = button.DisplayText;
            this.InsertBeforeTarget = this.InsertBeforeTarget;
        }


        /// <summary>
        /// The button callback method.
        /// </summary>
        /// <param name="context"></param>
        private void ButtonDefinition_OnExecute(NameValueMap context)
        {
            if (Execute != null)
                Execute();
            else
                MessageBox.Show("Nothing to execute.");
        }

        /// <summary>
        /// The button action to be assigned from anywhere outside.
        /// </summary>
        public Action Execute;
    }
}

