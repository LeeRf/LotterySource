using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperLotto.Data
{
    public class SQL
    {
        private static readonly string delete_log_by_ids = " delete from exception_log where id in ({0})";

        public static string DeleteLogByIds(List<int> ids)
        {
            string idStr = string.Join(",", ids);
            return string.Format(delete_log_by_ids, idStr);
        }

        private static readonly string select_log_by_ids = " select * from exception_log where id in ({0})";

        public static string SelectLogByIds(List<int> ids)
        {
            string idStr = string.Join(",", ids);
            return string.Format(select_log_by_ids, idStr);
        }
    }
}
