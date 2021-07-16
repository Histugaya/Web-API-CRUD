namespace FreeShareAPI.Controllers.Base
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Converter"></typeparam>
    public class BaseDataManager<Converter>
        where Converter:class, new()
    {
        protected readonly Converter converter;

       /// <summary>
       /// default constructor
       /// </summary>
        public BaseDataManager() : base()
        {
            converter = new Converter();
        }
    }
}