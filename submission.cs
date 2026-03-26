using Newtonsoft.Json;
using System;
//using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Schema;
using static System.Net.WebRequestMethods;
//using static System.Runtime.InteropServices.JavaScript.JSType;



//NationalParks.xsd page URL: https://namrata-335.github.io/cse445_a4/NationalParks.xsd
//NationalPsrks.xml url: https://namrata-335.github.io/cse445_a4/NationalParks.xml
//NationalParksErrors.xml urL;https://namrata-335.github.io/cse445_a4/NationalParksErrors.xml

namespace ConsoleApp1
{
    public class Submission
    {
        public static string xmlURL = "https://namrata-335.github.io/cse445_a4/NationalParks.xml";
        public static string xsdURL = "https://namrata-335.github.io/cse445_a4/NationalParks.xsd";
        public static string xmlErrorURL = "https://namrata-335.github.io/cse445_a4/NationalParksErrors.xml";

        public static string errors = "";

        public static void Main(string[] args)
        {

            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }
        //Takes the URL of an XML file and the URL of the corresponding XMLS(.xsd) file as input
        //and validate the XML file against the corresponding XMLS(XSD) file
        public static string Verification(string xmlURL, string xsdURL)
        {

            errors = "";

            // Create the XmlSchemaSet class
            XmlSchemaSet sc = new XmlSchemaSet();
            //Add the schema to the collection before performing validation
            sc.Add(null, xsdURL);

            try
            {

                // Define the validation settings.
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas = sc; // Association

                //Add event handler
                settings.ValidationEventHandler += new
                ValidationEventHandler(ValidationCallBack);


                //Create the XmlReader object.
                XmlReader reader = XmlReader.Create(xmlURL, settings);

                // Parse the file.
                while (reader.Read()) ;  // will call event handler if invalid
                Console.WriteLine("The XML file validation has completed");

                //Return the result as a string
                if (errors == "")
                    return "No errors are found";
                else
                    return errors;
            }

            catch (Exception ex)
            {
                return "Validation Error: " + ex.Message;
            }
        }



        // Display any validation errors.
        private static void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            errors += "Validation Error: " + e.Message + "\n";
        }

        //convert the XML without errors into a list of Json elements.
        public static string Xml2Json(string xmlURL)
        {

            // Load XML document formm URL
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlURL);

            // Convert XML to JSON
            string jsonText = JsonConvert.SerializeXmlNode( xmlDoc, Newtonsoft.Json.Formatting.Indented, false);
         

            //// The returned jsonText needs to be de-serializable by Newtonsoft.Json package.
            //(JsonConvert.DeserializeXmlNode(jsonText));
            return jsonText;


        }

    }
}
