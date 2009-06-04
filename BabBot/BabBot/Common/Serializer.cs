/*
    This file is part of BabBot.

    BabBot is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    BabBot is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BabBot.  If not, see <http://www.gnu.org/licenses/>.
  
    Copyright 2009 BabBot Team
*/
using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace BabBot.Common
{
    public class Serializer<T> where T : new()
    {
        public T Load(string FileName)
        {
            using (var fs = new FileStream(FileName, FileMode.Open))
            {
                var s = new XmlSerializer(typeof (T));
                try
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    return (T) s.Deserialize(fs);
                }
                catch
                {
                    return new T();
                }
                finally
                {
                    fs.Close();
                }
            }
        }

        public bool Save(string FileName, T obj)
        {
            bool RetValue = true;
            var s = new XmlSerializer(typeof (T));
            using (TextWriter writer = new StreamWriter(FileName))
            {
                try
                {
                    s.Serialize(writer, obj);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine(ex.StackTrace);
                    RetValue = false;
                }
                finally
                {
                    writer.Close();
                }
            }
            return RetValue;
        }
    }
}