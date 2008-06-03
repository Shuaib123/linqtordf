using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.VisualStudio.TextTemplating;

namespace RdfMetal
{
    public class OntModelDirectiveProcessor : DirectiveProcessor
    {
        //this buffer stores the code that is added to the 
        //generated transformation class after all the processing is done 
        //---------------------------------------------------------------------
        private StringBuilder codeBuffer;


        //Using a Code Dom Provider creates code for the 
        //generated transformation class in either Visual Basic or C#.
        //If you want your directive processor to support only one language, you
        //can hard code the code you add to the generated transformation class.
        //In that case, you do not need this field.
        //--------------------------------------------------------------------------
        private CodeDomProvider codeDomProvider;


        //this stores the full contents of the text template that is being processed
        //--------------------------------------------------------------------------
        private String templateContents;


        //These are the errors that occur during processing. The engine passes 
        //the errors to the host, and the host can decide how to display them,
        //for example the the host can display the errors in the UI
        //or write them to a file.
        //---------------------------------------------------------------------
        private CompilerErrorCollection errorsValue;
        public new CompilerErrorCollection Errors
        {
            get { return errorsValue; }
        }


        //Each time this directive processor is called, it creates a new property.
        //We count how many times we are called, and append "n" to each new
        //property name. The property names are therefore unique.
        //-----------------------------------------------------------------------------
        private int directiveCount = 0;


        public override void Initialize(ITextTemplatingEngineHost host)
        {
            //we do not need to do any initialization work
        }


        public override void StartProcessingRun(CodeDomProvider languageProvider, String templateContents, CompilerErrorCollection errors)
        {
            //the engine has passed us the language of the text template
            //we will use that language to generate code later
            //----------------------------------------------------------
            this.codeDomProvider = languageProvider;
            this.templateContents = templateContents;
            this.errorsValue = errors;

            this.codeBuffer = new StringBuilder();
        }


        //Before calling the ProcessDirective method for a directive, the 
        //engine calls this function to see whether the directive is supported.
        //Notice that one directive processor might support many directives.
        //---------------------------------------------------------------------
        public override bool IsDirectiveSupported(string directiveName)
        {
            if (string.Compare(directiveName, "CoolDirective", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return true;
            }
            if (string.Compare(directiveName, "SuperCoolDirective", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return true;
            }
            return false;
        }


        public override void ProcessDirective(string directiveName, IDictionary<string, string> arguments)
        {
            if (string.Compare(directiveName, "CoolDirective", StringComparison.OrdinalIgnoreCase) == 0)
            {
                string fileName;

                if (!arguments.TryGetValue("FileName", out fileName))
                {
                    throw new DirectiveProcessorException("Required argument 'FileName' not specified.");
                }

                if (string.IsNullOrEmpty(fileName))
                {
                    throw new DirectiveProcessorException("Argument 'FileName' is null or empty.");
                }

                //Now we add code to the generated transformation class.
                //This directive supports either Visual Basic or C#, so we must use the
                //System.CodeDom to create the code.
                //If a directive supports only one language, you can hard code the code.
                //--------------------------------------------------------------------------

                CodeMemberField documentField = new CodeMemberField();

                documentField.Name = "document" + directiveCount + "Value";
                documentField.Type = new CodeTypeReference(typeof(XmlDocument));
                documentField.Attributes = MemberAttributes.Private;

                CodeMemberProperty documentProperty = new CodeMemberProperty();

                documentProperty.Name = "Document" + directiveCount;
                documentProperty.Type = new CodeTypeReference(typeof(XmlDocument));
                documentProperty.Attributes = MemberAttributes.Public;
                documentProperty.HasSet = false;
                documentProperty.HasGet = true;

                CodeExpression fieldName = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), documentField.Name);
                CodeExpression booleanTest = new CodeBinaryOperatorExpression(fieldName, CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(null));
                CodeExpression rightSide = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("XmlReaderHelper"), "ReadXml", new CodePrimitiveExpression(fileName));
                CodeStatement[] thenSteps = new CodeStatement[] { new CodeAssignStatement(fieldName, rightSide) };

                CodeConditionStatement ifThen = new CodeConditionStatement(booleanTest, thenSteps);
                documentProperty.GetStatements.Add(ifThen);

                CodeStatement s = new CodeMethodReturnStatement(fieldName);
                documentProperty.GetStatements.Add(s);

                CodeGeneratorOptions options = new CodeGeneratorOptions();
                options.BlankLinesBetweenMembers = true;
                options.IndentString = "    ";
                options.VerbatimOrder = true;
                options.BracingStyle = "C";

                using (StringWriter writer = new StringWriter(codeBuffer, CultureInfo.InvariantCulture))
                {
                    codeDomProvider.GenerateCodeFromMember(documentField, writer, options);
                    codeDomProvider.GenerateCodeFromMember(documentProperty, writer, options);
                }

            }//end CoolDirective


            //One directive processor can contain many directives.
            //If you want to support more directives, the code goes here...
            //-----------------------------------------------------------------
            if (string.Compare(directiveName, "supercooldirective", StringComparison.OrdinalIgnoreCase) == 0)
            {
                //code for SuperCoolDirective goes here...
            }//end SuperCoolDirective


            //Track how many times the processor has been called.
            //-----------------------------------------------------------------
            directiveCount++;

        }//end ProcessDirective


        public override void FinishProcessingRun()
        {
            this.codeDomProvider = null;

            //important: do not do this:
            //the get methods below are called after this method 
            //and the get methods can access this field
            //-----------------------------------------------------------------
            //this.codeBuffer = null;
        }


        public override string GetPreInitializationCodeForProcessingRun()
        {
            //Use this method to add code to the start of the 
            //Initialize() method of the generated transformation class.
            //We do not need any pre-initialization, so we will just return "".
            //-----------------------------------------------------------------
            //GetPreInitializationCodeForProcessingRun runs before the 
            //Initialize() method of the base class.
            //-----------------------------------------------------------------
            return String.Empty;
        }


        public override string GetPostInitializationCodeForProcessingRun()
        {
            //Use this method to add code to the end of the 
            //Initialize() method of the generated transformation class.
            //We do not need any post-initialization, so we will just return "".
            //------------------------------------------------------------------
            //GetPostInitializationCodeForProcessingRun runs after the
            //Initialize() method of the base class.
            //-----------------------------------------------------------------
            return String.Empty;
        }


        public override string GetClassCodeForProcessingRun()
        {
            //Return the code to add to the generated transformation class.
            //-----------------------------------------------------------------
            return codeBuffer.ToString();
        }


        public override string[] GetReferencesForProcessingRun()
        {
            //This returns the references that we want to use when 
            //compiling the generated transformation class.
            //-----------------------------------------------------------------
            //We need a reference to this assembly to be able to call 
            //XmlReaderHelper.ReadXml from the generated transformation class.
            //-----------------------------------------------------------------
            return new string[]
            {
                "System.Xml",
                this.GetType().Assembly.Location
            };
        }


        public override string[] GetImportsForProcessingRun()
        {
            //This returns the imports or using statements that we want to 
            //add to the generated transformation class.
            //-----------------------------------------------------------------
            //We need CustomDP to be able to call XmlReaderHelper.ReadXml
            //from the generated transformation class.
            //-----------------------------------------------------------------
            return new string[]
            {
                "System.Xml",
                "RdfMetal"
            };
        }

    }
    public static class XmlReaderHelper
    {
        public static XmlDocument ReadXml(string fileName)
        {
            XmlDocument d = new XmlDocument();

            using (XmlTextReader reader = new XmlTextReader(fileName))
            {
                try
                {
                    d.Load(reader);
                }
                catch (System.Xml.XmlException e)
                {
                    throw new DirectiveProcessorException("Unable to read the XML file.", e);
                }
            }
            return d;
        }
    }//end class XmlReaderHelper

}
