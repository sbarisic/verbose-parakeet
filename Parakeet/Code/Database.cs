using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Data.SqlClient;
using System.CodeDom;
using System.Web.UI.WebControls;

namespace Parakeet.Code {
	public class Database {
		static SqlConnection _Con;
		static int RefCounter = 0;

		public SqlConnection Con {
			get {
				return _Con;
			}
		}

		public void Connect() {
			if (_Con != null) {
				if (_Con.State == System.Data.ConnectionState.Open) {
					Interlocked.Increment(ref RefCounter);
					return;
				}
			}

			string ConnectionString = Cfg.DatabaseConnection;

			if (string.IsNullOrEmpty(ConnectionString))
				throw new Exception("Connection string not found");

			_Con = new SqlConnection(ConnectionString);
			_Con.Open();
			Interlocked.Increment(ref RefCounter);
		}

		public void Disconnect() {
			if (Interlocked.Decrement(ref RefCounter) <= 0)
				_Con.Close();
		}
	}

	public static class ParakeetDb {
		delegate T UseDBFunc<T>(Database Db);
		static object LockObj = new object();

		static T UseDB<T>(UseDBFunc<T> F) {
			lock (LockObj) {
				Database Db = new Database();
				Db.Connect();

				try {
					return F(Db);
				} finally {
					Db.Disconnect();
				}
			}
		}

		static void GetTableData<T>(out string TableName) where T : DbTableEntry {
			DbTableAttribute DbTableAttr = typeof(T).GetCustomAttribute<DbTableAttribute>();
			TableName = DbTableAttr.TableName;
		}

		static object GetReaderValue(SqlDataReader Reader, int Ordinal, Type T) {
			if (T.IsEnum) {
				int EnumVal = Reader.GetInt32(Ordinal);
				object Val = Utils.ToEnum(EnumVal, T);
				return Val;
			} else if (T == typeof(int))
				return Reader.GetInt32(Ordinal);
			else if (T == typeof(string))
				return Reader.GetString(Ordinal);
			else if (T == typeof(float))
				return Reader.GetFloat(Ordinal);
			else if (T == typeof(double))
				return Reader.GetDouble(Ordinal);
			else
				throw new NotImplementedException();
		}

		static IEnumerable<PropertyInfo> GetProps<T>(params Type[] SkipAttribTypes) where T : DbTableEntry {
			PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			foreach (var P in Props) {
				bool Skip = false;

				if (SkipAttribTypes.Length != 0) {
					foreach (var SkipType in SkipAttribTypes) {
						if (P.GetCustomAttribute(SkipType) != null) {
							Skip = true;
							continue;
						}
					}
				}

				if (!Skip)
					yield return P;
			}
		}

		static T CreateInstance<T>(SqlDataReader Reader) where T : DbTableEntry {
			T Inst = Activator.CreateInstance<T>();
			PropertyInfo[] Props = GetProps<T>().ToArray();

			for (int i = 0; i < Props.Length; i++) {
				string Name = Props[i].Name;
				int Ordinal = Reader.GetOrdinal(Name);

				if (!Reader.IsDBNull(Ordinal)) {
					object Val = GetReaderValue(Reader, Ordinal, Props[i].PropertyType);
					Props[i].SetValue(Inst, Val);
				}
			}

			return Inst;
		}

		static bool ContainsFilterByName(DbFilter[] Filters, string Name) {
			for (int i = 0; i < Filters.Length; i++) {
				if (Filters[i].Name == Name)
					return true;
			}

			return false;
		}

		static bool HasAttr<T>(PropertyInfo P) where T : Attribute {
			if (P.GetCustomAttribute<T>() != null)
				return true;

			return false;
		}

		[SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<Pending>")]
		static SqlCommand CreateSelectCommand<T>(Database Db, DbFilter[] Filters) where T : DbTableEntry {
			GetTableData<T>(out string TableName);

			if (Filters == null)
				Filters = new DbFilter[] { };

			string FilterStr = "";
			if (Filters.Length > 0) {
				FilterStr = "where " + string.Join(" and ", Filters.Select(F => string.Format("({0} = @{0})", F.Name)));
			}

			SqlCommand Cmd = new SqlCommand(string.Format("select * from {0} {1}", TableName, FilterStr), Db.Con);

			for (int i = 0; i < Filters.Length; i++) {
				Cmd.Parameters.AddWithValue(Filters[i].Name, Filters[i].Value);
			}

			return Cmd;
		}

		[SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<Pending>")]
		static SqlCommand CreateUpdateCommand<T>(Database Db, T Value, DbFilter[] Filters) where T : DbTableEntry {
			GetTableData<T>(out string TableName);

			if (Filters == null)
				Filters = new DbFilter[] { };

			string FilterStr = "";
			if (Filters.Length > 0) {
				FilterStr = "where " + string.Join(" and ", Filters.Select(F => string.Format("({0} = @{0})", F.Name)));
			}

			PropertyInfo[] Props = GetProps<T>().Where(P => !ContainsFilterByName(Filters, P.Name)).ToArray();
			if (Props.Length == 0)
				throw new Exception("No values to set!");

			string ValueStr = string.Join(", ", Props.Select(P => string.Format("{0} = @{0}", P.Name)));

			SqlCommand Cmd = new SqlCommand(string.Format("update {0} set {1} {2}", TableName, ValueStr, FilterStr), Db.Con);

			for (int i = 0; i < Filters.Length; i++) {
				Cmd.Parameters.AddWithValue(Filters[i].Name, Filters[i].Value);
			}

			for (int i = 0; i < Props.Length; i++) {
				object Val = Props[i].GetValue(Value);

				if (Val == null)
					Val = DBNull.Value;

				Cmd.Parameters.AddWithValue(Props[i].Name, Val);
			}

			return Cmd;
		}

		[SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<Pending>")]
		static SqlCommand CreateInsertCommand<T>(Database Db, T Value) where T : DbTableEntry {
			GetTableData<T>(out string TableName);

			PropertyInfo[] Props = GetProps<T>().Where(P => !HasAttr<DbIDColumn>(P)).ToArray();
			string[] PropNames = Props.Select(P => P.Name).ToArray();
			string[] PropNames2 = Props.Select(P => "@" + P.Name).ToArray();

			SqlCommand Cmd = new SqlCommand(string.Format("insert into {0}({1}) output INSERTED.ID values({2})", TableName, string.Join(", ", PropNames), string.Join(", ", PropNames2)), Db.Con);

			for (int i = 0; i < Props.Length; i++) {
				object Val = Props[i].GetValue(Value);

				if (Val == null)
					Val = DBNull.Value;

				Cmd.Parameters.AddWithValue(Props[i].Name, Val);
			}

			return Cmd;
		}

		public static T[] Select<T>(params DbFilter[] Filters) where T : DbTableEntry {
			return UseDB((Db) => {
				SqlCommand Cmd = CreateSelectCommand<T>(Db, Filters);
				List<T> Vals = new List<T>();

				using (SqlDataReader Reader = Cmd.ExecuteReader()) {
					while (Reader.Read()) {
						Vals.Add(CreateInstance<T>(Reader));
					}
				}

				return Vals.ToArray();
			});
		}

		public static void UpdateByID<T>(T Val) where T : DbTableEntry {
			UseDB<object>((Db) => {
				SqlCommand Cmd = CreateUpdateCommand(Db, Val, new[] { new DbFilter("ID", Val.ID) });
				int Result = Cmd.ExecuteNonQuery();

				if (Result < 0)
					throw new Exception("Database error");

				return null;
			});
		}

		public static T Insert<T>(T Val) where T : DbTableEntry {
			return UseDB((Db) => {
				if (Val == null)
					Val = Activator.CreateInstance<T>();

				SqlCommand Cmd = CreateInsertCommand(Db, Val);
				Val.ID = (int)Cmd.ExecuteScalar();

				return Val;
			});
		}
	}

	public class DbFilter {
		public string Name;
		public object Value;

		public DbFilter(string Name, object Value) {
			this.Name = Name;
			this.Value = Value;
		}
	}

	public class DbTableEntry {
		[DbIDColumn]
		public int ID {
			get; set;
		}
	}

	[DbTable("ParakeetUser")]
	public class ParakeetUser : DbTableEntry {
		public string Username {
			get; set;
		}

		public string Salt {
			get; set;
		}

		public string Hash {
			get; set;
		}
	}

	[DbTable("ContentID")]
	public class ContentID : DbTableEntry {
		public int OwnerID {
			get; set;
		}

		public string Name {
			get; set;
		}

		public string Description {
			get; set;
		}

		public ContentIDType ContentType {
			get; set;
		}

		public string FileName {
			get; set;
		}
	}

	public enum ContentIDType : int {
		None = 0,
		Image,
		Video
	}
}