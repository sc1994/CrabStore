using System.Collections.Generic;

namespace Model.ViewModel
{
    public class PageInfo<T>
    {
        public List<T> Data { get; set; } = new List<T>();
        public string Sql { get; set; } = string.Empty;
        public int Total { get; set; } = 0;
    }
}
