using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text;

namespace mockup
{
    public class IsoStoreHelper
    {
        private const string folderName = "ivleMetro";

        private static IsolatedStorageFile _isoStore;
        public static IsolatedStorageFile IsoStore
        {
            get { return _isoStore ?? (_isoStore = IsolatedStorageFile.GetUserStoreForApplication()); }
        }

        public static void SaveContent(string dataName, object input, Type type)
        {
            if (!IsoStore.DirectoryExists(folderName))
            {
                IsoStore.CreateDirectory(folderName);
            }

            dataName = String.Format("{0}.json", dataName);
            string fileStreamName = System.IO.Path.Combine(folderName, dataName);

            if (IsoStore.FileExists(fileStreamName))
            {
                IsolatedStorageFileStream fileStream = IsoStore.OpenFile(fileStreamName, FileMode.Open, FileAccess.Write);
                using (StreamWriter writeFile = new StreamWriter(fileStream))
                {
                    string temp = Serialize(input, type);
                    writeFile.WriteLine(temp);
                    writeFile.Close();
                }
            }
            else
            {
                using (StreamWriter writeFile = new StreamWriter(new IsolatedStorageFileStream(fileStreamName, FileMode.Create, FileAccess.Write, IsoStore)))
                {
                    string temp = Serialize(input, type);
                    writeFile.WriteLine(temp);
                    writeFile.Close();
                }
            }
        }

        public static object LoadContent(string dataName, Type type)
        {
            object content = new object();

            dataName = String.Format("{0}.json", dataName);
            string fileStreamName = System.IO.Path.Combine(folderName, dataName);

            if (IsoStore.FileExists(fileStreamName))
            {
                using (IsolatedStorageFileStream stream = IsoStore.OpenFile(fileStreamName, FileMode.Open, FileAccess.Read))
                {
                    if (stream.Length > 0)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {    
                            string temp = reader.ReadLine();
                            content = Deserialize(temp, type);
                            reader.Close();
                        }
                    }
                }

            }
            
            return content;
        }

        public static object Deserialize(String input, Type objectType)
        {
            if (objectType == null || input == null)
                return null;

            JsonSerializer ser = new JsonSerializer();
            JsonReader jr = new JsonTextReader(new StringReader(input));
            return ser.Deserialize(jr, objectType);
        }

        public static string Serialize(Object input, Type objectType)
        {
            StringBuilder temp = new StringBuilder();

            if (objectType == null || input == null)
                return null;

            JsonSerializer ser = new JsonSerializer();
            JsonWriter jw = new JsonTextWriter(new StringWriter(temp));
            ser.Serialize(jw, input);
            jw.Flush();

            return temp.ToString();
        }
    }
}
