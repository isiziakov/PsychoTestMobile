using SQLite;

namespace PsychoTestAndroid.DataBase.Entity
{
    [Table("Scales")]
    public class DbScale
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int _id { get; set; }
        
        public string TestId { get; set; }

        public string Scales { get; set; }

        public DbScale()
        {

        }

        public DbScale(string data, string testId)
        {
            Scales = data;
            TestId = testId;
        }
    }
}