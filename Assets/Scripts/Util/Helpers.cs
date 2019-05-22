using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
using UnityEngine;

public static class Helpers
{

	public static float PerlinNoise(float x, float y, float[,] frequencies, float offset)
    {
        float noiseSample = 0f;
        float totalWeight = 0f;
        for (int i = 0; i < frequencies.GetLength(1); i++)
        {
            noiseSample += frequencies[0, i] * Mathf.PerlinNoise(x * frequencies[1, i] + offset, y * frequencies[1, i] + offset);
            totalWeight += frequencies[0, i];
        }
        return noiseSample / totalWeight;
    }

    public static string ToString<T>(IEnumerable<T> collection)
    {
        string returnString = "[";
        foreach (T element in collection)
        {
            returnString += element.ToString() + ", ";
        }
        if (returnString.Length > 1)
        {
            returnString = returnString.Substring(0, returnString.Length - 2);
        }
        returnString =  returnString + "]";
        return returnString;
    }

    public static string XMLfromODS(string path)
    {
        string xml;

        using (ZipArchive archive = ZipFile.OpenRead(path))
        {
            ZipArchiveEntry content = null;
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                if (entry.Name.ToLower() == "content.xml")
                {
                    content = entry;
                }
            }

            using (Stream stream = content.Open())
            {
                var bytesResult = new byte[] { };
                var bytes = new byte[2000];
                var i = 0;

                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    var arrayLength = bytesResult.Length;
                    Array.Resize<byte>(ref bytesResult, arrayLength + i);
                    Array.Copy(bytes, 0, bytesResult, arrayLength, i);
                }
                xml = Encoding.UTF8.GetString(bytesResult);
            }
        }
        return xml;
    }

    public static string[,] StringTablefromODSXML(string xml)
    {
        XDocument doc = XDocument.Parse(xml);

        List<List<string>> listTable = new List<List<string>>();
        IEnumerable<XElement> xmlRows = doc.Descendants("{urn:oasis:names:tc:opendocument:xmlns:table:1.0}table-row");
        foreach (XElement xmlRow in xmlRows)
        {
            List<string> tableRow = new List<string>();
            IEnumerable<XElement> xmlCells = xmlRow.Descendants("{urn:oasis:names:tc:opendocument:xmlns:table:1.0}table-cell");
            foreach (XElement cell in xmlCells)
            {
                XAttribute attribute = cell.Attribute("{urn:oasis:names:tc:opendocument:xmlns:table:1.0}number-columns-repeated");
                IEnumerable<XElement> paragraphs = cell.Descendants("{urn:oasis:names:tc:opendocument:xmlns:text:1.0}p");

                string text = "";

                foreach (XElement paragraph in paragraphs)
                {
                    text = paragraph.Value;
                }

                if (attribute != null)
                {
                    int columnsRepeated = int.Parse(attribute.Value);
                    for (int i = 0; i < columnsRepeated; i++)
                    {
                        tableRow.Add(text);
                    }
                }

                tableRow.Add(text);
            }
            listTable.Add(tableRow);
        }

        string[,] arrayTable = new string[listTable[0].Count, listTable.Count];
        for (int x = 0; x < arrayTable.GetLength(0); x++)
        {
            for (int y = 0; y < arrayTable.GetLength(1); y++)
            {
                arrayTable[x, y] = listTable[y][x];
            }
        }

        return arrayTable;
    }
}
