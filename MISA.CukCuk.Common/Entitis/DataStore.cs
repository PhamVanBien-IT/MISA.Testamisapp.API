using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Testamis.Common.Entitis
{
    public class DataStore
    {
        /// <summary>
        /// Lưu giá trị mảng nhân viên
        /// </summary>
        /// private static DataStore _instance = null;
        private static DataStore _instance = null;
        private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

        private DataStore()
        {
        }

        public static DataStore Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataStore();
                }

                return _instance;
            }
        }

        public void Set<T>(string key, T value)
        {
            if (_data.ContainsKey(key))
            {
                _data[key] = value;
            }
            else
            {
                _data.Add(key, value);
            }
        }

        public T Get<T>(string key)
        {
            if (_data.ContainsKey(key))
            {
                return (T)_data[key];
            }

            return default(T);
        }
    }
}
