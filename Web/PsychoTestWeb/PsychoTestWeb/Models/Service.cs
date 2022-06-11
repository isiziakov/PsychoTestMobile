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
            // обращаемся к коллекциям
            Users = database.GetCollection<User>("Users");
            Patients = database.GetCollection<Patient>("Patients");
            Tests = database.GetCollection<Test>("Tests");
            Results = database.GetCollection<Result>("Results");
        }
        // получаем всех пользователей, используя критерии фильтрации
        public IEnumerable<User> GetUsers()
        {
            // строитель фильтров
            var builder = new FilterDefinitionBuilder<User>();
            var filter = builder.Empty; // фильтр для выборки всех документов

            return Users.Find(filter).ToList();
        }

        // получаем одного пользователя по id
        public User GetUser(string id)
        {
            return Users.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefault();
        }

        // получаем всех пациетов, используя критерии фильтрации
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
        // получаем все тесты, используя критерии фильтрации
        public IEnumerable<Test> GetTests()
        {
            var builder = new FilterDefinitionBuilder<Test>();
            var filter = builder.Empty;
            return Tests.Find(filter).ToList();
        }

        // получаем результаты пациента
        public IEnumerable<Result> GetPatientsResults(string patientId)
        {
            var builder = new FilterDefinitionBuilder<Result>();
            var filter = builder.Empty;
            filter = filter & builder.Regex("patient", new BsonRegularExpression(patientId));
            return Results.Find(filter).ToList();
        }

        public IEnumerable<Result> GetResults()
        {
            var builder = new FilterDefinitionBuilder<Result>();
            var filter = builder.Empty;
            return Results.Find(filter).ToList();
        }

        public void UpdateResults(string id, Result r)
        {
            Results.ReplaceOne(new BsonDocument("_id", new ObjectId(id)), r);
        }
    }

}