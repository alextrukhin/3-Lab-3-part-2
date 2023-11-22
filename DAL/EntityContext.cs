﻿using System.Text.RegularExpressions;

namespace DAL
{
    public interface IProvider<T> where T : class
    {
        public List<T> Load();
        public void Save(List<T> listToSave);
    }
    public class EntityContext<T> where T : class, new()
    {
        public IProvider<T> Provider;
        string _DBName = "custom-db";
        public string DBName { get => _DBName; set { _DBName = value ?? throw new ArgumentException(); } }
        public string DBType = "txt";
        public string DBFile => $"{DBName}.{DBType}";
        public string[] AvailableDBTypes => new string[] { "json", "xml", "bin", "txt" };
        public EntityContext() => Provider = new CustomProvider<T>(DBFile);
        public void SetProvider(string dbType, string dbName)
        {
            DBType = dbType;
            DBName = dbName;
            switch (dbType)
            {
                case "json":
                    Provider = new JSONProvider<T>(DBFile);
                    break;
                case "xml":
                    Provider = new XMLProvider<T>(DBFile);
                    break;
                case "bin":
                    Provider = new BinaryProvider<T>(DBFile);
                    break;
                case "txt":
                    Provider = new CustomProvider<T>(DBFile);
                    break;
            }
        }
        public void SetProvider(string FileName)
        {
            if(!Regex.IsMatch(FileName, @"^[a-zA-Z0-9_\-\.]+\.[a-zA-Z0-9]+$")) throw new ArgumentException();
            string[] Parts = FileName.Split('.');
            SetProvider(Parts[1], Parts[0]);
        }
    }
}
