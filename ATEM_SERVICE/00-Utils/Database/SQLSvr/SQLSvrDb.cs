using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Utils.SQL
{
    public class SQLSvrDb : ISQLDb
    {
        private DbContext context { get; set; }

        public SQLSvrDb(object context)
        {
            this.context = context as DbContext;
        }

        public void CreateCommand(SQLCommandHandler handler)
        {
            var conn = this.context.Database.GetDbConnection();

            try
            {
                conn.Open();

                using (var command = conn.CreateCommand())
                {
                    command.CommandTimeout = 0;
                    handler(new SQLSvrDbCommand(command));
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
    }
    public class SQLSvrDbCommand : ASQLDbCommand
    {
        private System.Data.Common.DbCommand command { get; set; }

        public SQLSvrDbCommand(System.Data.Common.DbCommand command)
        {
            this.CommandType = System.Data.CommandType.StoredProcedure;

            this.command = command;
        }

        #region Override Methods

        public override ISQLDbParameter AddParameter(Type type, string name, object value)
        {
            if (this.command == null)
                return null;

            System.Data.SqlClient.SqlParameter param = null;
            if (Utils.CommonUtil.IsNullOrEmpty(value))
            {
                param = new System.Data.SqlClient.SqlParameter(name, type);
                param.Value = DBNull.Value;
            }
            else
                param = new System.Data.SqlClient.SqlParameter(name, value);

            if (param != null)
                this.command.Parameters.Add(param);

            return new SQLSvrDbParameter(param);
        }
        public override ISQLDbParameter AddOutputParameter(Type type, string name, int? size = null)
        {
            if (this.command == null)
                return null;

            System.Data.SqlClient.SqlParameter output = new System.Data.SqlClient.SqlParameter(name, type);
            if (type == typeof(int))
                output.Value = 0;
            else if (type == typeof(DateTime))
                output.Value = DateTime.Now;
            else
                output.Value = "";

            if (size != null)
                output.Size = size.Value;

            output.Direction = System.Data.ParameterDirection.InputOutput;
            this.command.Parameters.Add(output);

            return new SQLSvrDbParameter(output);
        }
        public override void ClearParameters()
        {
            if (this.command == null)
                return;

            this.command.Parameters.Clear();
        }

        public override bool ExecuteReader(SQLReaderHandler handler)
        {
            if (this.command == null)
                return false;

            this.command.CommandText = this.CommandText;
            this.command.CommandType = this.CommandType;

            using (System.Data.Common.DbDataReader reader = this.command.ExecuteReader())
            {
                if (handler(new SQLSvrDbReader(reader)) == false)
                    return false;
            }

            return true;
        }
        public override object ExecuteScalar()
        {
            if (this.command == null)
                return null;

            this.command.CommandText = this.CommandText;
            this.command.CommandType = this.CommandType;

            return this.command.ExecuteScalar();
        }
        public override int ExecuteNonQuery()
        {
            if (this.command == null)
                return -1;

            this.command.CommandText = this.CommandText;
            this.command.CommandType = this.CommandType;

            return this.command.ExecuteNonQuery();
        }

        public override T ToObject<T>()
        {
            System.Collections.IList[] result = ToList(typeof(T));
            if (result != null)
            {
                if (result.Length > 0)
                {
                    List<T> list = (List<T>)result[0];
                    if (list != null)
                    {
                        if (list.Count > 0)
                            return list[0];
                    }
                }
            }

            return default(T);
        }
        public override List<T> ToList<T>()
        {
            System.Collections.IList[] result = ToList(typeof(T));
            if (result != null)
            {
                if (result.Length > 0)
                {
                    List<T> list = (List<T>)result[0];
                    if (list != null)
                        return list;
                }
            }

            return new List<T>();
        }
        public override System.Collections.IList[] ToList(params Type[] types)
        {
            System.Collections.IList[] results = new System.Collections.IList[types.Length];

            if (types.Length > 0)
            {
                this.ExecuteReader(new SQLReaderHandler((ISQLDbReader reader) =>
                {
                    for (int idx = 0; idx < types.Length; idx++)
                    {
                        Type type = types[idx];
                        if (reader.HasRows)
                            results[idx] = ToList(reader, type);
                        
                        if (reader.NextResult() == false)
                            break;
                    }

                    return true;
                }));
            }

            return results;
        }

        public override T MappingData<T>(ISQLDbReader reader)
        {
            return (T)MappingData(reader, typeof(T));
        }
        public override object MappingData(ISQLDbReader reader, Type otype)
        {
            object data = Activator.CreateInstance(otype);

            Type type = data.GetType();
            for (int colIdx = 0; colIdx < reader.TotalColumns; colIdx++)
            {
                string colName = reader.GetName(colIdx);
                System.Reflection.PropertyInfo prop = type.GetProperty(colName);
                if (prop != null)
                {
                    if (prop.CanWrite != true)
                        continue;

                    object val = reader.Value(colIdx, prop.PropertyType);
                    if (val == null)
                        continue;

                    prop.SetValue(data, val, null);
                }
            }

            return data;
        }

        #endregion

        private System.Collections.IList ToList(ISQLDbReader reader, Type type)
        {
            System.Collections.IList result =
                    (System.Collections.IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
            while (reader.Read())
            {
                result.Add(MappingData(reader, type));
            }

            return result;
        }
    }
    public class SQLSvrDbReader : ISQLDbReader
    {
        private System.Data.Common.DbDataReader reader { get; set; }

        public SQLSvrDbReader(System.Data.Common.DbDataReader reader)
        {
            this.reader = reader;
        }

        public bool HasRows
        {
            get
            {
                if (this.reader == null)
                    return false;

                return this.reader.HasRows;
            }
        }
        public bool Read()
        {
            if (this.reader == null)
                return false;

            return this.reader.Read();
        }
        public bool NextResult()
        {
            if (this.reader == null)
                return false;

            return this.reader.NextResult();
        }

        public int TotalColumns
        {
            get
            {
                if (this.reader == null)
                    return 0;

                return this.reader.FieldCount;
            }
        }
        public string GetName(int idx)
        {
            if (this.reader == null)
                return null;

            return reader.GetName(idx);
        }
        public object Value(int idx, Type type)
        {
            if (this.reader == null)
                return null;
            if (this.reader.IsDBNull(idx))
                return null;

            string typeName = type.FullName;
            if (typeName == typeof(string).Name)
                return reader.GetString(idx);
            else if (typeName.Contains(typeof(int).Name))
            {
                int nv = 0;
                if (int.TryParse(reader.GetValue(idx).ToString(), out nv))
                    return nv;
            }
            else if (typeName.Contains(typeof(decimal).Name))
            {
                decimal nv = 0;
                if (decimal.TryParse(reader.GetValue(idx).ToString(), out nv))
                    return nv;
            }
            else if (typeName.Contains(typeof(bool).Name))
                return reader.GetBoolean(idx);
            else if (typeName.Contains(typeof(DateTime).Name))
                return reader.GetDateTime(idx);
            else
                return reader.GetValue(idx);

            return null;
        }
    }
    public class SQLSvrDbParameter : ISQLDbParameter
    {
        private System.Data.SqlClient.SqlParameter parameter { get; set; }

        public SQLSvrDbParameter(System.Data.SqlClient.SqlParameter parameter)
        {
            this.parameter = parameter;
        }

        public object Value
        {
            get
            {
                if (this.parameter == null)
                    return null;

                return this.parameter.Value;
            }
        }
    }
}
