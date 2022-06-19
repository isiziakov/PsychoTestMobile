using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace PsychoTestWeb.Models
{
    public class Service
    {
        IMongoCollection<User> Users; // коллекция в базе данных
        IMongoCollection<Patient> Patients;
        IMongoCollection<Test> Tests;
        IMongoCollection<BsonDocument> TestsBson;
        IMongoDatabase database;
        public Service()
        {
            string connectionString = "mongodb://localhost:27017/MobilePsychoTest";
            var connection = new MongoUrlBuilder(connectionString);
            // получаем клиента для взаимодействия с базой данных
            MongoClient client = new MongoClient(connectionString);
            // получаем доступ к самой базе данных
            database = client.GetDatabase(connection.DatabaseName);
            // обращаемся к коллекциям
            Users = database.GetCollection<User>("Users");
            Patients = database.GetCollection<Patient>("Patients");
            Tests = database.GetCollection<Test>("Tests");
            TestsBson = database.GetCollection<BsonDocument>("Tests");
        }

        //пользователи
        #region
        public IEnumerable<User> GetUsers()
        {
            // строитель фильтров
            var builder = new FilterDefinitionBuilder<User>();
            var filter = builder.Empty; // фильтр для выборки всех документов

            return Users.Find(filter).ToList();
        }
        #endregion

        //пациенты
        #region
        //список всех пациентов
        public async Task<IEnumerable<Patient>> GetPatients()
        {
            var builder = new FilterDefinitionBuilder<Patient>();
            var filter = builder.Empty;
            return await Patients.Find(filter).ToListAsync();
        }
        //получаем пациента по id
        public async Task<Patient> GetPatientById(string id)
        {
            return await Patients.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }

        //получаем количество страниц с пациентами, если на странице 10 пациентов
        public async Task<double> GetPatientsPagesCount()
        {
            var builder = new FilterDefinitionBuilder<Patient>();
            var filter = builder.Empty;
            long count = await Patients.CountDocumentsAsync(filter);
            return Math.Ceiling((double)count / 10.0);
        }

        //получаем часть пациентов для пагинации
        public async Task<IEnumerable<Patient>> GetPatientsWithCount(int pageNumber)
        {
            var builder = new FilterDefinitionBuilder<Patient>();
            var filter = builder.Empty;
            List<Patient> allPatients = await Patients.Find(filter).ToListAsync();
            int start = (pageNumber - 1) * 10;
            int count = 10;
            if (start + count >= allPatients.Count)
                count = allPatients.Count - start;
            Patient[] page = new Patient[count];
            allPatients.CopyTo(start, page, 0, count);
            return page;
        }
        //фильтрация по имени
        public async Task<IEnumerable<Patient>> GetPatientsByName(string value)
        {
            var builder = new FilterDefinitionBuilder<Patient>();
            var filter = builder.Empty;
            var allPatients = await Patients.Find(filter).ToListAsync();
            return allPatients.FindAll(x => x.name.ToLower().Contains(value.ToLower()) == true);
        }
        //получаем количество страниц с пациентами c фильтрацией по имени, если на странице 10 пациентов
        public async Task<double> GetPatientsByNamePagesCount(string value)
        {
            var patients = await GetPatientsByName(value);
            patients = patients.ToList();
            long count = patients.Count();
            return Math.Ceiling((double)count / 10.0);
        }
        //получаем часть пациентов c фильтрацией по имени для пагинации
        public async Task<IEnumerable<Patient>> GetPatientsByNameWithCount(int pageNumber, string name)
        {
            List<Patient> patients = new List<Patient>();
            var p = await GetPatientsByName(name);
            patients = p.ToList();
            int start = (pageNumber - 1) * 10;
            int count = 10;
            if (start + count >= patients.Count)
                count = patients.Count - start;
            Patient[] page = new Patient[count];
            patients.CopyTo(start, page, 0, count);
            return page;
        }
        // добавление пациента
        public async Task CreatePatient(Patient p)
        {
            await Patients.InsertOneAsync(p);
        }
        // обновление пациента
        public async Task UpdatePatient(string id, Patient p)
        {
            BsonDocument doc = new BsonDocument("_id", new ObjectId(id));
            await Patients.ReplaceOneAsync(doc, p);
        }
        // удаление пациента
        public async Task RemovePatient(string id)
        {
            await Patients.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }
        #endregion


        //Тесты
        #region
        //получаем краткий список всех тестов в формате id-название
        public async Task<IEnumerable<Test>> GetTestsView()
        {
            var documents = await TestsBson.Find(new BsonDocument()).ToListAsync();
            List<Test> tests = new List<Test>();
            foreach (BsonDocument doc in documents)
            {
                tests.Add(new Test { name = doc["IR"]["Name"]["#text"].AsString, id = doc["_id"].AsObjectId.ToString() });
            }
            return tests;
        }

        // получаем все тесты
        public async Task<List<object>> GetTests()
        {
            var documents = await TestsBson.Find(new BsonDocument()).ToListAsync();
            var dotNetObjList = documents.ConvertAll(BsonTypeMapper.MapToDotNetValue);
            return dotNetObjList;
        }
        //получаем тест по id
        public async Task<string> GetTestById(string id)
        {
            var bsonDoc = await TestsBson.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
            var dotNetObj = BsonTypeMapper.MapToDotNetValue(bsonDoc);
            return JsonConvert.SerializeObject(dotNetObj);
        }

        //Импорт теста
        public async Task ImportFile(string file)
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(file);
            var json = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.None, true);

            var jObj = JObject.Parse(json);
            int i = 0;
            foreach (var question in jObj["Questions"]["item"])
            {
                question["Question_id"] = i;
                i++;

                //если вопрос только с текстом — Question_Type = 0, если с изображением Question_Type = 1
                if (question["ImageFileName"] != null)
                    question["Question_Type"] = 1;
                else
                    question["Question_Type"] = 0;

                //если вопрос c выбором одного отета — Question_Choice = 1, если с вводом своего — Question_Choice = 0
                if (question["TypeQPanel"] != null)
                {
                    if (question["TypeQPanel"].ToString() == "2" || question["AnsString_Num"] != null ||
                                question["AnsString_ExanineAnsMethod"] != null || question["Ans_CheckUpperCase"] != null)
                    {
                        question["Question_Choice"] = 0;
                        if (question["Answers"]["item"] is JArray)
                            foreach (var answer in question["Answers"]["item"])
                                answer["Answer_type"] = 1;
                        else
                        {
                            JArray arr = new JArray();
                            question["Answers"]["item"]["Answer_Type"] = 1;
                            arr.Add(question["Answers"]["item"]);
                            question["Answers"]["item"] = arr;
                        }

                    }
                    else
                    {
                        question["Question_Choice"] = 1;
                        if (question["Answers"]["item"] is JArray)
                            foreach (var answer in question["Answers"]["item"])
                                answer["Answer_type"] = 0;
                        else
                        {
                            JArray arr = new JArray();
                            question["Answers"]["item"]["Answer_Type"] = 0;
                            arr.Add(question["Answers"]["item"]);
                            question["Answers"]["item"] = arr;
                        }
                    }
                }
                else
                {
                    question["Question_Choice"] = 1;
                    if (question["Answers"]["item"] is JArray)
                        foreach (var answer in question["Answers"]["item"])
                            answer["Answer_type"] = 0;
                    else
                    {
                        JArray arr = new JArray();
                        question["Answers"]["item"]["Answer_Type"] = 0;
                        arr.Add(question["Answers"]["item"]);
                        question["Answers"]["item"] = arr;
                    }
                }
                int j = 0;
                if (question["Answers"]["item"] is JArray)
                    foreach (var answer in question["Answers"]["item"])
                    {
                        answer["Answer_id"] = j;
                        j++;
                        //если ответ это текст — Answer_Type = 0, если это поле для ввода — Answer_Type = 1, если изображение — Answer_Type = 2;
                        if (answer["ImageFileName"] != null)
                            answer["Answer_Type"] = 2;
                    }
                else
                {
                    JArray arr = new JArray();
                    question["Answers"]["item"]["Answer_id"] = 0;
                    if (question["Answers"]["item"]["ImageFileName"] != null)
                        question["Answers"]["item"]["Answer_Type"] = 2;
                    arr.Add(question["Answers"]["item"]);
                    question["Answers"]["item"] = arr;
                }
            }
            var document = BsonDocument.Parse(Newtonsoft.Json.JsonConvert.SerializeObject(jObj));
            await TestsBson.InsertOneAsync(document);
        }
        #endregion


    }

}