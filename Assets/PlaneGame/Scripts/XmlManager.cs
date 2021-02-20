using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography;
using UnityEngine;
using System.Text;
using System;
using System.Xml.Linq;

public static class XmlManager {
    static byte[] _keyArray = Encoding.UTF8.GetBytes("20181023152962439585152873145549");

    static RijndaelManaged GetRijndaelManaged()
    {
        RijndaelManaged rDel = new RijndaelManaged
        {
            Key = _keyArray,
            Mode = CipherMode.ECB,
            Padding = PaddingMode.PKCS7
        };
        return rDel;
    }

    public static string Encrypt(string toEncrypt)
    {
        ICryptoTransform cTransform = GetRijndaelManaged().CreateEncryptor();
        byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    public static string Decrypt(string toDecrypt)
    {
        ICryptoTransform cTransform = GetRijndaelManaged().CreateDecryptor();
        byte[] toDecryptArray = Convert.FromBase64String(toDecrypt);
        byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
        return Encoding.UTF8.GetString(resultArray);
    }

    private static void EncrtyptSaveXML(string toEncrypt, string xmlpath)
    {
        //StreamReader sReader = File.OpenText(xmlpath);
        //string xmlData = sReader.ReadToEnd();
        //sReader.Close();
        string xxx = Encrypt(toEncrypt);
        StreamWriter writer = new StreamWriter(xmlpath,false);
        writer.Write(xxx);
        writer.Close();
        //writer = File.CreateText(xmlpath);
        //writer.Write(xxx);
        //writer.Close();
    }

    public static XElement DecrtyptLoadXML(string xmlpath)
    {
        if (File.Exists(xmlpath))
        {
            StreamReader sReader = File.OpenText(xmlpath);
            string xmlData = sReader.ReadToEnd();
            sReader.Close();
            string xxx = Decrypt(xmlData);

            XElement root = XElement.Parse(xxx);
            return root;
        }
        else
            return null;
    }

    public static void SetElementValue(string xmlpath, XElement root, string name, string valueName, string value)
    {
        //XElement root = DecrtyptLoadXML(xmlpath);
        XElement curXel = root.Element(name).Element(valueName);
        curXel.SetValue(value);

        EncrtyptSaveXML(root.ToString(), xmlpath);
//        Debug.Log(root.ToString());
    }

    public static void SetElementValue(string xmlpath,XElement root, string name, XElement value)
    {
        IEnumerable list = root.Element(name).Elements();
        foreach(XElement xel in list){
            xel.SetValue(value.Element(xel.Name).Value);
        }

        EncrtyptSaveXML(root.ToString(), xmlpath);
//        Debug.Log(root.ToString());
    }

    public static void AddElement(string xmlpath,XElement root, XElement value)
    {
        root.Add(value);

        EncrtyptSaveXML(root.ToString(), xmlpath);
//        Debug.Log(root.ToString());
    }

    public static void InitFile(string xmlpath, string dataString)
    {
        //string sourcePath = Application.streamingAssetsPath + "/BlockInfoXML.xml";
        //StreamReader reader = File.OpenText(sourcePath);

        string targetPath = xmlpath;
        if (!File.Exists(targetPath))
        {
            //bool isrewrite = true; // true=覆盖已存在的同名文件,false则反之
            //File.Copy(sourcePath, targetPath, isrewrite);

            StreamWriter writer;
            writer = File.CreateText(targetPath);
            writer.Write(Encrypt(dataString));
            writer.Close();
        }
    }

    public static string GetPersistentFilePath(string path)
    {
        return Application.persistentDataPath + "/" + path + ".xml";
    }

}
