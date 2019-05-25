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
        returnString = returnString + "]";
        return returnString;
    }

    public static string XMLfromODS(string path)
    {
        string xml = "";

        ZipArchive archive = null;
        try
        {
            archive = ZipFile.OpenRead(path);
        }
        catch
        {
            Debug.LogError("Error opening file: "+path);
        }

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

        archive.Dispose();

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
                    for (int i = 0; i < columnsRepeated - 1; i++)
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

    public static AttackType ParseAttackType(string value)
    {
        try
        {
            return (AttackType)Enum.Parse(typeof(AttackType), value);
        }
        catch
        {
            Debug.LogWarning("Failed to parse AttackType: \"" + value + "\". Defaulting to AttackType.Bullet");
            return AttackType.Bullet;
        }
    }

    public static AttackMode ParseAttackMode(string value)
    {
        try
        {
            return (AttackMode)Enum.Parse(typeof(AttackMode), value);
        }
        catch
        {
            Debug.LogWarning("Failed to parse AttackMode: \"" + value + "\". Defaulting to AttackMode.Self");
            return AttackMode.Self;
        }
    }

    public static TargetingType ParseTargetingType(string value)
    {
        try
        {
            return (TargetingType)Enum.Parse(typeof(TargetingType), value);
        }
        catch
        {
            Debug.LogWarning("Failed to parse TargetingType: \"" + value + "\". Defaulting to TargetingType.Bound");
            return TargetingType.Bound;
        }
    }

    public static AIType ParseAIType(string value)
    {
        try
        {
            return (AIType)Enum.Parse(typeof(AIType), value);
        }
        catch
        {
            Debug.LogWarning("Failed to parse AIType: \"" + value + "\". Defaulting to AIType.PassiveAI");
            return AIType.PassiveAI;
        }
    }

    public static int ParseInt(string value)
    {
        try
        {
            return int.Parse(value);
        }
        catch
        {
            Debug.LogWarning("Failed to parse int: \"" + value + "\". Defaulting to 0");
            return 0;
        }
    }

    public static float ParseFloat(string value)
    {
        try
        {
            return float.Parse(value);
        }
        catch
        {
            Debug.LogWarning("Failed to parse float: \"" + value + "\". Defaulting to 0");
            return 0f;
        }
    }

    public static HullModel ParseHullModel(string value)
    {
        try
        {
            return (HullModel)Enum.Parse(typeof(HullModel), value);
        }
        catch
        {
            Debug.LogWarning("Failed to parse HullModel: \"" + value + "\". Defaulting to HullModel.Alpha1");
            return HullModel.Alpha1;
        }
    }

    public static WeaponType ParseWeaponType(string value)
    {
        try
        {
            return (WeaponType)Enum.Parse(typeof(WeaponType), value);
        }
        catch
        {
            Debug.LogWarning("Failed to parse WeaponType: \"" + value + "\". Defaulting to WeaponType.Cannon");
            return WeaponType.Cannon;
        }
    }

    public static string FormatNumber(float number, int length)
    {
        //int digits = Mathf.FloorToInt(Mathf.Log10(number));
        string value = number.ToString();
        return value.Substring(0, Mathf.Min(value.Length, length));
    }
}
