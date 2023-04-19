using PsychoTestAndroid.DataBase.Entity;

namespace PsychoTestAndroid.DataBase.Interfaces
{
    public interface IDbRepos
    {
        IRepository<DbTest> Test { get; }
        IRepository<DbScale> Scale { get; }
        IRepository<DbTestCalcInfo> TestCalcInfo { get; }
    }
}