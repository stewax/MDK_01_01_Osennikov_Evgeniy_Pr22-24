using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using ClassModule;
using System.Text;
using System.Threading.Tasks;

namespace ClassConnection
{
    public class Connection
    {
        public List<User> users = new List<User>();
        public List<Call> calls = new List<Call>();

        public enum tables
        {
            users, calls
        }
        public string localPath = "";

        public bool ItsNumber(string num)
        {
            if (string.IsNullOrEmpty(num))
                return false;
            foreach (char c in num)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        public bool ItsOnlyFIO(string FIO)
        {
            if (string.IsNullOrWhiteSpace(FIO))
                return false;
            foreach (char c in FIO)
            {
                if (!(char.IsLetter(c) || c == ' ' || c == '-' || c == '\''))
                {
                    return false;
                }
            }
            return true;
        }


        public OleDbDataReader QueryAccess(string query)
        {
            try
            {
                localPath = Directory.GetCurrentDirectory();
                OleDbConnection connect = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + localPath + "/accessbase.accdb");
                connect.Open();
                OleDbCommand cmd = new OleDbCommand(query, connect);
                OleDbDataReader reader = cmd.ExecuteReader();
                return reader;
            }
            catch
            {
                return null;
            }
        }
        public int SetLastId(tables tabel)
        {
            try
            {
                LoadData(tabel);
                switch (tabel.ToString())
                {
                    case "users":
                        if (users.Count >= 1)
                        {
                            int max_status = users[0].id;
                            max_status = users.Max(x => x.id);
                            return max_status + 1;
                        }
                        else return 1;
                    case "calls":
                        if (calls.Count >= 1)
                        {
                            int max_status = calls[0].id;
                            max_status = calls.Max(x => x.id);
                            return max_status + 1;
                        }
                        else return 1;
                }
                return -1;
            }
            catch
            {
                return -1;
            }
        }

        public void LoadData(tables zap)
        {
            try
            {
                OleDbDataReader itemQuery = QueryAccess("select * from [" + zap.ToString() + "]order by [Код]");
                if (zap.ToString() == "users")
                {
                    users.Clear();
                    while (itemQuery.Read())
                    {
                        User newE1 = new User();
                        newE1.id = Convert.ToInt32(itemQuery.GetValue(0));
                        newE1.phone_num = Convert.ToString(itemQuery.GetValue(1));
                        newE1.fio_user = Convert.ToString(itemQuery.GetValue(2));
                        newE1.passport_data = Convert.ToString(itemQuery.GetValue(3));
                        users.Add(newE1);
                    }
                }
                if (zap.ToString() == "calls")
                {
                    calls.Clear();
                    while (itemQuery.Read())
                    {
                        Call NewE1 = new Call();
                        NewE1.id = Convert.ToInt32(itemQuery.GetValue(0));
                        NewE1.user_id = Convert.ToInt32(itemQuery.GetValue(1));
                        NewE1.category_call = Convert.ToInt32(itemQuery.GetValue(2));
                        NewE1.date = Convert.ToString(itemQuery.GetValue(3));
                        NewE1.time_start = Convert.ToString(itemQuery.GetValue(4));
                        NewE1.time_end = Convert.ToString(itemQuery.GetValue(5));
                        calls.Add(NewE1);
                    }
                }
                if (itemQuery != null) itemQuery.Close();
            }
            catch
            {
                Console.WriteLine("NULL");
            }
        }
    }
}
