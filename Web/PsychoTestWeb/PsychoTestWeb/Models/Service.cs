using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PsychoTestWeb.Models
{
    public class Service
    {
        //IGridFSBucket gridFS;   // файловое хранилище
        IMongoCollection<User> Users; // коллекция в базе данных
        IMongoCollection<Patient> Patients;
        IMongoCollection<Test> Tests;
        IMongoCollection<Result> Results;
        public Service()
        {
            // строка подключения
            string connectionString = "mongodb://localhost:27017/MobilePsychoTest";
            var connection = new MongoUrlBuilder(connectionString);
            // получаем клиента для взаимодействия с базой данных
            MongoClient client = new MongoClient(connectionString);
            // получаем доступ к самой базе данных
            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);
            // получаем доступ к файловому хранилищу
            //gridFS = new GridFSBucket(database);
            // обращаемся к коллекции Products
            Users = database.GetCollection<User>("Users");
            Patients = database.GetCollection<Patient>("Patients");
            Tests = database.GetCollection<Test>("Tests");
            Results = database.GetCollection<Result>("Results");
        }
        // получаем все документы, используя критерии фильтрации
        public IEnumerable<User> GetUsers()
        {
            // строитель фильтров
            var builder = new FilterDefinitionBuilder<User>();
            var filter = builder.Empty; // фильтр для выборки всех документов

            return Users.Find(filter).ToList();
        }

        // получаем один документ по id
        public User GetUser(string id)
        {
            return Users.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefault();
        }

        public IEnumerable<Patient> GetPatients()
        {
            var builder = new FilterDefinitionBuilder<Patient>();
            var filter = builder.Empty;
            return Patients.Find(filter).ToList();
        }

        // добавление пациента
        public void CreatePatient(Patient p)
        {
            Patients.InsertOne(p);
        }

        // обновление пациента
        public void UpdatePatient(string id, Patient p)
        {
            Patients.ReplaceOne(new BsonDocument("_id", new ObjectId(id)), p);
        }
        // удаление пациента
        public void RemovePatient(string id)
        {
            Patients.DeleteOne(new BsonDocument("_id", new ObjectId(id)));
        }
    }
}