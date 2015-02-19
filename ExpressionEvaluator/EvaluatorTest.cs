using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WESNet.Utilities.ExpressionEvaluator;
using System.Dynamic;

namespace ExpressionEvaluatorTest
{
    public partial class EvaluatorTest : Form
    {
        protected Evaluator _evaluator;

        public EvaluatorTest()
        {
            InitializeComponent();
            _evaluator = new Evaluator();
            _evaluator.IdentifierRegistry.Add("five", new IdentifierDefinition(null, null, "five", Enums.IdentifierDefinitionTypes.Variable, 5));
            _evaluator.IdentifierRegistry.Add("ten", new IdentifierDefinition(null, null, "ten", Enums.IdentifierDefinitionTypes.Variable, 10));
            _evaluator.IdentifierRegistry.Add("myname", new IdentifierDefinition(null, null, "myname", Enums.IdentifierDefinitionTypes.Variable, "Bill Severance"));
            _evaluator.UpdateProgress += UpdateProgress;
            DisplayIdentifiers();
        }

        private void btnEvaluate_Click(object sender, EventArgs e)
        {
            if (tbExpression.Text.Length > 0)
            {
                lvTokens.Items.Clear();
                lvOperators.Items.Clear();
                lvOperands.Items.Clear();
                lvIdentifiers.Items.Clear();
                
                Literal result = null;

                try
                {
                    result = _evaluator.Evaluate(tbExpression.Text);
                    if (result == null)
                    {
                        var sb = new StringBuilder();
                        foreach (EvaluatorError error in _evaluator.EvaluatorErrors)
                        {
                            sb.AppendLine(error.ToString());
                        }
                        if (sb.Length == 0) sb.Append("Expression returned no result value.\r\n");
                        lblResults.Text = "Unable to evaluate due to error:\r\n" + sb.ToString();
                    }
                    else
                    {
                        lblResults.Text = result.ToString();
                    }
                }
                catch (Exception exc)
                {
                    lblResults.Text = exc.Message;
                }

                DisplayAll();
            }
        }

        private void lvTokens_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvTokens.SelectedItems.Count == 0)
            {
                lblTokenDetail.Text = "No Token Selected";
            }
            else
            {
                var lvItem = lvTokens.SelectedItems[0];
                lblTokenDetail.Text = ((Token)lvItem.Tag).ToString();
            }
        }

        protected void UpdateProgress (object sender, EventArgs e)
        {
            DisplayAll();
        }

        protected void DisplayAll()
        {
            DisplayTokens();
            DisplayOperators();
            DisplayOperands();
            DisplayIdentifiers();
            Refresh();
        }

        public void DisplayTokens()
        {
            lvTokens.BeginUpdate();
            lvTokens.Items.Clear();

            var i = 1;
            foreach (var token in _evaluator.RPNTokens)
            {
                object tokenValue = "";
                if (token.TokenText != null)
                {
                    tokenValue = token.TokenText;
                }
                var lvItem = new ListViewItem(i.ToString());
                lvItem.SubItems.Add(token.TokenType.ToString());
                try
                {
                    lvItem.SubItems.Add(tokenValue.ToString());
                }
                catch (EvaluatorException exc)
                {
                    lvItem.SubItems.Add("NA - " + exc.EvaluatorError.ToString());
                }
                lvItem.Tag = token;
                lvTokens.Items.Add(lvItem);
                i++;
            }
            lvTokens.EndUpdate();
        }

        public void DisplayOperators()
        {
            lvOperators.BeginUpdate();
            lvOperators.Items.Clear();
            var i = 1;
            foreach (Operator op in _evaluator.OperatorStack)
            {
                var lvItem = new ListViewItem(i.ToString());
                lvItem.SubItems.Add(op.Symbol);
                lvItem.SubItems.Add(op.OperatorName);
                lvItem.SubItems.Add(op.Priority.ToString());
                lvItem.Tag = op;
                lvOperators.Items.Add(lvItem);
                i++;
            }
            lvOperators.EndUpdate();
        }

        public void DisplayOperands()
        {
            lvOperands.BeginUpdate();
            lvOperands.Items.Clear();
            var i = 1;
            foreach (Operand oprnd in _evaluator.OperandStack)
            {
                var lvItem = new ListViewItem(i.ToString());
                lvItem.SubItems.Add(oprnd.OperandTypeName);
                lvItem.SubItems.Add(oprnd.Value == null ? "NULL" : oprnd.Value.ToString());
                lvItem.Tag = oprnd;
                lvOperands.Items.Add(lvItem);
                i++;
            }
            lvOperands.EndUpdate();
        }

        public void DisplayIdentifiers()
        {
            lvIdentifiers.BeginUpdate();
            lvIdentifiers.Items.Clear();
            var i = 1;
            foreach (IdentifierDefinition identifierDefinition in _evaluator.IdentifierRegistry.Values)
            {
                var lvItem = new ListViewItem(i.ToString());
                lvItem.SubItems.Add(identifierDefinition.IdentifierDefinitionType.ToString());
                lvItem.SubItems.Add(identifierDefinition.FullyQualifiedName);
                lvItem.SubItems.Add(identifierDefinition.Value == null ? "NULL" : identifierDefinition.Value.ToString());
                lvItem.Tag = identifierDefinition;
                lvIdentifiers.Items.Add(lvItem);
                i++;
            }
            foreach (List<MethodDefinition> methodDefinitionList in _evaluator.MethodRegistry.Values)
            {
                
                foreach (MethodDefinition methodDefinition in methodDefinitionList)
                {
                    var j = 1;
                    var lvItem = new ListViewItem(string.Format("{0}.{1:##}", i,j));
                    if (j > 1)
                    {
                        lvItem.SubItems.Add("");
                        lvItem.SubItems.Add("");
                    }
                    else
                    {
                        lvItem.SubItems.Add(methodDefinition.IdentifierDefinitionType.ToString());
                        lvItem.SubItems.Add(methodDefinition.TypeName.TrimEnd('.'));
                    }
                    lvItem.SubItems.Add(methodDefinition.ToString());
                    j++;
                    lvItem.Tag = methodDefinition;
                    lvIdentifiers.Items.Add(lvItem);
                }
                i++;
            }

            lvIdentifiers.EndUpdate();
        }

        private void lvOperands_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvOperands.SelectedItems.Count == 0)
            {
                lblTokenDetail.Text = "No Operand Selected";
            }
            else
            {
                var lvItem = lvOperands.SelectedItems[0];
                lblTokenDetail.Text = ((Literal)lvItem.Tag).ToString();
            }
        }

        private void lvOperators_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvOperators.SelectedItems.Count == 0)
            {
                lblTokenDetail.Text = "No Operator Selected";
            }
            else
            {
                var lvItem = lvOperators.SelectedItems[0];
                lblTokenDetail.Text = ((Operator)lvItem.Tag).ToString();
            }
        }

        private void lvIdentifiers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvIdentifiers.SelectedItems.Count == 0)
            {
                lblTokenDetail.Text = "No Token Selected";
            }
            else
            {
                var lvItem = lvIdentifiers.SelectedItems[0];
                var identifierDefinition = (IdentifierDefinition)(lvItem.Tag);
                switch (identifierDefinition.IdentifierDefinitionType)
                {
                    case Enums.IdentifierDefinitionTypes.IdentifierDefinition:
                    case Enums.IdentifierDefinitionTypes.Variable:
                    case Enums.IdentifierDefinitionTypes.Class:
                        lblTokenDetail.Text = identifierDefinition.ToString();
                        break;
                    case Enums.IdentifierDefinitionTypes.Method:
                        lblTokenDetail.Text = ((MethodDefinition)identifierDefinition).ToString();
                        break;
                    case Enums.IdentifierDefinitionTypes.DNNToken:
                        lblTokenDetail.Text = ((DNNTokenDefinition)identifierDefinition).ToString();
                        break;
                }
            }
        }
    }
}