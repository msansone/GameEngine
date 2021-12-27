using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScintillaNET;
using System.IO;
using System.Media;

namespace FiremelonEditor2
{
    public partial class PythonScriptEditorControl : UserControl, IPythonScriptEditorControl
    {
        #region Contructors

        public PythonScriptEditorControl(IProjectController projectController)
        {
            InitializeComponent();

            setPythonSyntaxHighlighting();

            setupScintilla();

            IFiremelonEditorFactory firemelonEditorFactory = new FiremelonEditorFactory();

            projectLauncher_ = firemelonEditorFactory.NewProjectLauncher(projectController, this);

            IDialogFactory dialogFactory = new DialogFactory();

            findReplaceDialog_ = dialogFactory.NewFindReplaceDialog();

            findReplaceDialog_.FindNextString += new FindNextStringHandler(this.PythonScriptEditorControl_FindNextString);            
        }

        #endregion

        #region Private Variables

        private IFindReplaceDialog findReplaceDialog_;

        private IProjectLauncher projectLauncher_;

        private int maxLineNumberCharLength_;

        #endregion

        #region Properties

        public bool ChangesMade
        {
            get
            {
                return changesMade_;
            }
        }
        private bool changesMade_ = false;

        public IScriptDtoProxy Script
        {
            get
            {
                return script_;
            }
            set
            {
                script_ = value;

                loadScriptData();
            }
        }
        private IScriptDtoProxy script_ = null;

        #endregion

        #region Public Functions

        public void Save()
        {
            save();
        }
        
        #endregion

        #region Private Functions

        private void loadScriptData()
        {
            if (script_ == null)
            {
                scintilla.Text = string.Empty;

                scintilla.Enabled = false;

                tsbSave.Enabled = false;

                tslScriptName.Text = "No Loaded Script";
            }
            else if (string.IsNullOrEmpty(Script.ScriptPath))
            {
                scintilla.Text = string.Empty;

                scintilla.Enabled = false;

                tsbSave.Enabled = false;

                tslScriptName.Text = "No Loaded Script";
            }
            else
            {
                scintilla.Enabled = true;

                tsbSave.Enabled = true;

                scintilla.Text = File.ReadAllText(script_.ScriptPath);

                tslScriptName.Text = script_.Name;
            }

            changesMade_ = false;
        }

        private int findNext(string text, SearchFlags flags)
        {
            scintilla.SearchFlags = flags;
            scintilla.TargetStart = Math.Max(scintilla.CurrentPosition, scintilla.AnchorPosition);
            scintilla.TargetEnd = scintilla.TextLength;

            var pos = scintilla.SearchInTarget(text);

            if (pos >= 0)
            {
                scintilla.SetSel(scintilla.TargetStart, scintilla.TargetEnd);
            }
            else
            {
                SystemSounds.Asterisk.Play();
            }

            return pos;
        }

        private int findPrevious(string text, SearchFlags flags)
        {
            scintilla.SearchFlags = flags;
            scintilla.TargetStart = Math.Min(scintilla.CurrentPosition, scintilla.AnchorPosition);
            scintilla.TargetEnd = 0;

            var pos = scintilla.SearchInTarget(text);
            if (pos >= 0)
                scintilla.SetSel(scintilla.TargetStart, scintilla.TargetEnd);

            return pos;
        }

        private void highlightWord(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            // Indicators 0-7 could be in use by a lexer
            // so we'll use indicator 8 to highlight words.
            const int NUM = 8;

            // Remove all uses of our indicator
            scintilla.IndicatorCurrent = NUM;
            scintilla.IndicatorClearRange(0, scintilla.TextLength);

            // Update indicator appearance
            scintilla.Indicators[NUM].Style = IndicatorStyle.StraightBox;
            scintilla.Indicators[NUM].Under = true;
            scintilla.Indicators[NUM].ForeColor = Color.Green;
            scintilla.Indicators[NUM].OutlineAlpha = 50;
            scintilla.Indicators[NUM].Alpha = 30;

            // Search the document
            scintilla.TargetStart = 0;
            scintilla.TargetEnd = scintilla.TextLength;
            scintilla.SearchFlags = SearchFlags.None;
            while (scintilla.SearchInTarget(text) != -1)
            {
                // Mark the search results with the current indicator
                scintilla.IndicatorFillRange(scintilla.TargetStart, scintilla.TargetEnd - scintilla.TargetStart);

                // Search the remainder of the document
                scintilla.TargetStart = scintilla.TargetEnd;
                scintilla.TargetEnd = scintilla.TextLength;
            }
        }

        private void save()
        {
            if (script_ != null)
            {
                File.WriteAllText(script_.ScriptPath, scintilla.Text);

                changesMade_ = false;

                tslScriptName.Text = script_.Name;
            }
        }

        private void setPythonSyntaxHighlighting()
        {
            // Reset the styles
            scintilla.StyleResetDefault();
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 10;
            scintilla.StyleClearAll(); // i.e. Apply to all

            // Set the lexer
            scintilla.Lexer = Lexer.Python;

            // Known lexer properties:
            // "tab.timmy.whinge.level",
            // "lexer.python.literals.binary",
            // "lexer.python.strings.u",
            // "lexer.python.strings.b",
            // "lexer.python.strings.over.newline",
            // "lexer.python.keywords2.no.sub.identifiers",
            // "fold.quotes.python",
            // "fold.compact",
            // "fold"

            // Some properties we like
            scintilla.SetProperty("tab.timmy.whinge.level", "1");
            scintilla.SetProperty("fold", "1");

            // Use margin 2 for fold markers
            scintilla.Margins[2].Type = MarginType.Symbol;
            scintilla.Margins[2].Mask = Marker.MaskFolders;
            scintilla.Margins[2].Sensitive = true;
            scintilla.Margins[2].Width = 20;

            // Reset folder markers
            for (int i = Marker.FolderEnd; i <= Marker.FolderOpen; i++)
            {
                scintilla.Markers[i].SetForeColor(SystemColors.ControlLightLight);
                scintilla.Markers[i].SetBackColor(SystemColors.ControlDark);
            }

            // Style the folder markers
            scintilla.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            scintilla.Markers[Marker.Folder].SetBackColor(SystemColors.ControlText);
            scintilla.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            scintilla.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            scintilla.Markers[Marker.FolderEnd].SetBackColor(SystemColors.ControlText);
            scintilla.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            scintilla.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            scintilla.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            scintilla.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            // Enable automatic folding
            scintilla.AutomaticFold = (AutomaticFold.Show | AutomaticFold.Click | AutomaticFold.Change);

            // Set the styles
            scintilla.Styles[Style.Python.Default].ForeColor = Color.FromArgb(0x80, 0x80, 0x80);
            scintilla.Styles[Style.Python.CommentLine].ForeColor = Color.FromArgb(0x00, 0x7F, 0x00);
            scintilla.Styles[Style.Python.CommentLine].Italic = true;
            scintilla.Styles[Style.Python.Number].ForeColor = Color.FromArgb(0x00, 0x7F, 0x7F);
            scintilla.Styles[Style.Python.String].ForeColor = Color.FromArgb(0x7F, 0x00, 0x7F);
            scintilla.Styles[Style.Python.Character].ForeColor = Color.FromArgb(0x7F, 0x00, 0x7F);
            scintilla.Styles[Style.Python.Word].ForeColor = Color.FromArgb(0x00, 0x00, 0x7F);
            scintilla.Styles[Style.Python.Word].Bold = true;
            scintilla.Styles[Style.Python.Triple].ForeColor = Color.FromArgb(0x7F, 0x00, 0x00);
            scintilla.Styles[Style.Python.TripleDouble].ForeColor = Color.FromArgb(0x7F, 0x00, 0x00);
            scintilla.Styles[Style.Python.ClassName].ForeColor = Color.FromArgb(0x00, 0x00, 0xFF);
            scintilla.Styles[Style.Python.ClassName].Bold = true;
            scintilla.Styles[Style.Python.DefName].ForeColor = Color.FromArgb(0x00, 0x7F, 0x7F);
            scintilla.Styles[Style.Python.DefName].Bold = true;
            scintilla.Styles[Style.Python.Operator].Bold = true;
            // scintilla.Styles[Style.Python.Identifier] ... your keywords styled here
            scintilla.Styles[Style.Python.CommentBlock].ForeColor = Color.FromArgb(0x7F, 0x7F, 0x7F);
            scintilla.Styles[Style.Python.CommentBlock].Italic = true;
            scintilla.Styles[Style.Python.StringEol].ForeColor = Color.FromArgb(0x00, 0x00, 0x00);
            scintilla.Styles[Style.Python.StringEol].BackColor = Color.FromArgb(0xE0, 0xC0, 0xE0);
            scintilla.Styles[Style.Python.StringEol].FillLine = true;
            scintilla.Styles[Style.Python.Word2].ForeColor = Color.FromArgb(0x40, 0x70, 0x90);
            scintilla.Styles[Style.Python.Decorator].ForeColor = Color.FromArgb(0x80, 0x50, 0x00);
            
            // Important for Python
            scintilla.ViewWhitespace = WhitespaceMode.VisibleAlways;

            // Keyword lists:
            // 0 "Keywords",
            // 1 "Highlighted identifiers"

            //var python2 = "and as assert break class continue def del elif else except exec finally for from global if import in is lambda not or pass print raise return try while with yield";
            var python3 = "False None True and as assert break class continue def del elif else except finally for from global if import in is lambda nonlocal not or pass raise return try while with yield";
            var cython = "cdef cimport cpdef";

            scintilla.SetKeywords(0, python3 + " " + cython);
            // scintilla.SetKeywords(1, "add your own keywords here");
        }

        private void setupScintilla()
        {
            scintilla.ClearCmdKey(Keys.Control | Keys.F);
            scintilla.ClearCmdKey(Keys.Control | Keys.S);
        }
        
        #endregion

        #region Event Handlers

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // If the dialog is not visible, but there is valid search token in it, do the search without showing the form.
            // Unless, there is selected text that is different than the search token.
            bool dialogIsVisible = findReplaceDialog_.Visible;
            bool hasValidSearchToken = string.IsNullOrEmpty(findReplaceDialog_.TokenToFind) == false;
            string selectedText = scintilla.SelectedText;

            bool showDialog = false;

            if (dialogIsVisible == false)
            {
                // If a valid search token is already on the form, potentially don't need to show it.
                if (hasValidSearchToken == false)
                {
                    showDialog = true;
                }
                else
                {
                    // A valid search token already exists. If there is a new search token selected, show the form.
                    if (string.IsNullOrEmpty(selectedText) == false && findReplaceDialog_.TokenToFind != selectedText)
                    {
                        findReplaceDialog_.TokenToFind = selectedText;

                        showDialog = true;
                    }
                }
            }

            if (showDialog == true)
            {
                findReplaceDialog_.Show(this, scintilla.SelectedText);
            }
            else
            {
                // If a new string was selected, update the find field.
                if (string.IsNullOrEmpty(scintilla.SelectedText) == false)
                {
                    findReplaceDialog_.TokenToFind = scintilla.SelectedText;
                }

                findNext(findReplaceDialog_.TokenToFind, findReplaceDialog_.SearchFlags);                
            }
        }

        private void PythonScriptEditorControl_FindNextString(object sender, FindNextStringEventArgs e)
        {
            findNext(e.Find, e.SearchFlags);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
        }

        private void scintilla_TextChanged(object sender, EventArgs e)
        {
            if (script_ != null)
            {
                changesMade_ = true;

                tslScriptName.Text = script_.Name + " *";

                // Did the number of characters in the line number display change?
                // i.e. nnn VS nn, or nnnn VS nn, etc...
                var maxLineNumberCharLength = scintilla.Lines.Count.ToString().Length;

                if (maxLineNumberCharLength == maxLineNumberCharLength_)
                {
                    return;
                }

                // Calculate the width required to display the last line number and include some padding for good measure.
                const int padding = 2;

                scintilla.Margins[0].Width = scintilla.TextWidth(Style.LineNumber, new string('9', maxLineNumberCharLength + 1)) + padding;

                maxLineNumberCharLength_ = maxLineNumberCharLength;
            }
        }

        private void tsbRun_Click(object sender, EventArgs e)
        {
            save();

            projectLauncher_.ExportScriptsOnly = tsbScriptsOnly.Checked;

            projectLauncher_.Launch();
        }

        private void tsbRunWithConsole_Click(object sender, EventArgs e)
        {
            save();

            projectLauncher_.ExportScriptsOnly = tsbScriptsOnly.Checked;

            projectLauncher_.LaunchWithConsole();
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            save();
        }

        private void tsbScriptsOnly_Click(object sender, EventArgs e)
        {
            tsbScriptsOnly.Checked = !tsbScriptsOnly.Checked;
        }

        #endregion
    }
}
