﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Nemerle.VisualStudio.Project;
using System.Diagnostics;

namespace Nemerle.VisualStudio.GUI
{
	public partial class PromptProjectRenameForm : Form
	{
		public PromptProjectRenameForm(string projectName)
		{
			InitializeComponent();
			ProjectName = projectName;
			string suffix = "-Upgraded";
			switch (NetFrameworkProjectConstants.VisualStudioVersion)
			{
				case 10: suffix = "-VS_2010"; break;
				case 11: suffix = "-VS_2012"; break;
				case 12: suffix = "-VS_2013"; break;
				case 14: suffix = "-VS_2015"; break;
			}

			_newProjectName.Text = projectName + suffix;
		}

		public string ProjectName { get; private set; }

		private void PromptProjectRenameForm_FormClosing(object _, FormClosingEventArgs e)
		{
		}

		bool ValidateData()
		{
			var newName = _newProjectName.Text;
			var isError = string.Equals(ProjectName, newName, StringComparison.InvariantCultureIgnoreCase);

			if (isError)
			{
				_errorProvider.SetError(_newProjectName, "The new project name must differ from the old name.");
				_yesButton.Enabled = false;
				return false;
			}

			var isInvalidName = newName.Any(c => Path.GetInvalidFileNameChars().Contains(c));

			if (isInvalidName)
			{
				_errorProvider.SetError(_newProjectName, "The new project name contains invalid characters.");
				_yesButton.Enabled = false;
				return false;
			}

			_errorProvider.SetError(_newProjectName, "");
			_yesButton.Enabled = true;
			return true;
		}

		private void _newProjectName_Validating(object _, CancelEventArgs e)
		{
			_yesButton.Enabled = ValidateData();
		}

		private void _yesButton_Click(object sender, EventArgs e)
		{
			if (ValidateData())
			{
				Close();
				DialogResult = System.Windows.Forms.DialogResult.Yes;
				ProjectName = _newProjectName.Text;
			}
		}

		private void _newProjectName_TextChanged(object sender, EventArgs e)
		{
			_yesButton.Enabled = ValidateData();
		}
	}
}
