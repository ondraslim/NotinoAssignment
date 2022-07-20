using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Notino.Homework
{
    public class Document
    {
        public string Title { get; set; }
        public string Text { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var sourceFileName = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Source Files\\Document1.xml");
            var targetFileName = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Target Files\\Document1.json");

            string input = string.Empty;
            try
            {
                FileStream sourceStream = File.Open(sourceFileName, FileMode.Open);

                // stream is not disposed -> use 'using' statement
                //var reader = new StreamReader(sourceStream);
                using var reader = new StreamReader(sourceStream);

                // declaration of var 'input' must be above the try/catch block
                //string input = reader.ReadToEnd();
                input = reader.ReadToEnd();
            }
            catch (Exception)
            {
                // 1. this try / catch block is useless, it does not handle the exception in any way;
                // at least log would be good, depends on the use case
                // 2. stack trace of the exception is lost by creating new exception, better version would be to throw it again (which makes this useless)
                //throw new Exception(ex.Message);

                // logger.LogError(ex)
                throw;
            }

            // parse may throw an exception and its handling is missing (try / catch)
            // would be better to create a document serialization service to seggregate the responsibilities
            // XML elements might be missing, not handled!
            // It also might be useful to consider use of XmlSerializer to create the Document object.
            // Depends on the structure of the input (complex and uninteresting content of the file -> XDocument, fixed content (title & text) -> XmlSerializer)
            var xdoc = XDocument.Parse(input);
            var doc = new Document
            {
                Title = xdoc.Root.Element("title").Value,
                Text = xdoc.Root.Element("text").Value
            };

            var serializedDoc = JsonConvert.SerializeObject(doc);

            // File.Open does not handle exceptions (access rights, full disk, path, ...)
            //  -> add try/catch, log error, return an approriate message to the user
            var targetStream = File.Open(targetFileName, FileMode.Create, FileAccess.Write);

            // stream not disposed
            //var sw = new StreamWriter(targetStream);
            using var sw = new StreamWriter(targetStream);
            sw.Write(serializedDoc);
        }
    }
}