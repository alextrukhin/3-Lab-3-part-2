﻿using DAL;
using System.Text.RegularExpressions;

namespace BLL
{
    public class EntityService
    {
        List<Entity> data = new();
        EntityContext<Entity> db = new();
        public EntityService()
        {
            data = db.Provider.Load();
        }
        public void Insert(Entity input)
        {
            data.Add(input);
            db.Provider.Save(data);
        }
        public void Update(Entity input, int index)
        {
            data[index] = input;
            db.Provider.Save(data);
        }
        public void Delete(int index)
        {
            data.RemoveAt(index);
            db.Provider.Save(data);
        }
        public void Save() => db.Provider.Save(data);
        public int Length() => data.Count;
        public string DBName => db.DBName;
        public string DBType => db.DBType;
        public void SetProvider(string dbType, string dbName) { db.SetProvider(dbType, dbName); data = db.Provider.Load(); }
        public void SetProvider(string FileName) { db.SetProvider(FileName); data = db.Provider.Load(); }
        public string[] AvailableDBTypes => db.AvailableDBTypes;
        public static void ValidateName(string? name)
        {
            if (name == null || !Regex.Match(name, @"^\p{L}{1,32}$", RegexOptions.IgnoreCase).Success) throw new ArgumentException();
        }
        public static void ValidateID(string? id)
        {
            if (id == null || !Regex.Match(id, @"^КВ\d{8}$", RegexOptions.IgnoreCase).Success) throw new ArgumentException();
        }
        public static void ValidateCourse(int? course)
        {
            if (course < 1 || course > 6) throw new ArgumentException();
        }
        public static void ValidateMark(int? mark)
        {
            if (mark < 0 || mark > 5) throw new ArgumentException();
        }
        public static void ValidateCountry(string? country)
        {
            if (country == null || !Regex.Match(country, @"^(\p{L}| |-){1,32}$", RegexOptions.IgnoreCase).Success) throw new ArgumentException();
        }
        public static void ValidateForeignPassportNumber(string? number)
        {
            if (number == null || !Regex.Match(number, @"^(\p{L}|-|\d){1,32}$", RegexOptions.IgnoreCase).Success) throw new ArgumentException();
        }
        public List<Tuple<int, Student>> Search()
        {
            List<Tuple<int, Student>> Entities = new();
            if (data.Count == 0) { return Entities; }
            for (int i = 0; i < data.Count; i++)
            {
                Entity cur = data[i];
                if (cur is Student)
                {
                    Student student = cur as Student;
                    if (student is not null)
                    {
                        if (student.ForeignPassportNumber != null && student.Course == 1 && student.GPA == 5) Entities.Add(Tuple.Create(i, student));
                    }
                }
            }
            return Entities;
        }
        public Entity this[int position]
        {
            get => data[position];
            set => data[position]=value;
        }
    }
}
