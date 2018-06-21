using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.SQL
{
    public delegate void SQLCommandHandler(ASQLDbCommand command);
    public delegate bool SQLReaderHandler(ISQLDbReader reader);
    
    public interface ISQLDb
    {
        void CreateCommand(SQLCommandHandler handler);
    }
    public abstract class ASQLDbCommand
    {
        public string CommandText { get; set; }
        public System.Data.CommandType CommandType { get; set; }

        public abstract ISQLDbParameter AddParameter(Type type, string name, object value);
        public abstract ISQLDbParameter AddOutputParameter(Type type, string name, int? size = null);
        public abstract void ClearParameters();

        public ISQLDbParameter AddSearchParameter(Interfaces.ASearchCriteria criteria,
            bool sorting = true, bool isAssending = true, bool offsetRow = true, bool nextRowCount = true, bool totalRecord = true)
        {
            if (sorting)
                AddParameter(typeof(string), "Sorting", criteria.sorting);
            if (isAssending)
                AddParameter(typeof(bool), "IsAssending", criteria.isAssending);
            if (offsetRow)
                AddParameter(typeof(int), "OffsetRow", criteria.OffsetRow);
            if (nextRowCount)
                AddParameter(typeof(int), "NextRowCount", criteria.NextRowCount);

            if (totalRecord)
                return AddOutputParameter(typeof(int), "TotalRecord");

            return null;
        }
        public ISQLDbParameter AddErrorParameter()
        {
            return AddOutputParameter(typeof(string), "ErrorCode", size: 100);
        }

        public abstract bool ExecuteReader(SQLReaderHandler handler);
        public abstract object ExecuteScalar();
        public abstract int ExecuteNonQuery();

        public abstract T ToObject<T>() where T : class;
        public abstract List<T> ToList<T>() where T : class;
        public abstract System.Collections.IList[] ToList(params Type[] types);
        public abstract T MappingData<T>(ISQLDbReader reader) where T : class;
        public abstract object MappingData(ISQLDbReader reader, Type otype);
    }
    public interface ISQLDbReader
    {
        bool HasRows { get; }
        bool Read();
        bool NextResult();

        int TotalColumns { get; }
        string GetName(int idx);
        object Value(int idx, Type type);
    }
    public interface ISQLDbParameter
    {
        object Value { get; }
    }

    public abstract class ASQLDbResult
    {
        public virtual object Data { get; set; }
        public string Error { get; set; }

        public void ErrorParameter(ISQLDbParameter param)
        {
            if (param != null)
            {
                if (!Utils.CommonUtil.IsNullOrEmpty(param.Value))
                    this.Error = param.Value.ToString();
            }
        }
    }
}
