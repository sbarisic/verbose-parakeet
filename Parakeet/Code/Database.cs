using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Data.SqlClient;

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

		/*public static Korisnik GetKorisnik(string KorisnickoIme) {
			lock (LockObj) {
				Database Db = new Database();
				Db.Connect();
				try {
					SqlCommand Cmd = new SqlCommand("select * from Korisnik where KorisnickoIme = @KorisnickoIme", Db.Con);
					Cmd.Parameters.AddWithValue("@KorisnickoIme", KorisnickoIme);

					using (SqlDataReader Reader = Cmd.ExecuteReader()) {
						if (Reader.Read()) {
							Korisnik K = new Korisnik();

							K.ID = Reader.GetInt32(Reader.GetOrdinal("ID"));
							K.KorisnickoIme = Reader.GetString(Reader.GetOrdinal("KorisnickoIme"));

							if (!Reader.IsDBNull(Reader.GetOrdinal("Ime")))
								K.Ime = Reader.GetString(Reader.GetOrdinal("Ime"));

							if (!Reader.IsDBNull(Reader.GetOrdinal("Prezime")))
								K.Prezime = Reader.GetString(Reader.GetOrdinal("Prezime"));

							if (!Reader.IsDBNull(Reader.GetOrdinal("Salt")))
								K.Salt = Reader.GetString(Reader.GetOrdinal("Salt"));

							if (!Reader.IsDBNull(Reader.GetOrdinal("Hash")))
								K.Hash = Reader.GetString(Reader.GetOrdinal("Hash"));

							return K;
						}
					}

					return null;
				} finally {
					Db.Disconnect();
				}
			}
		}*/

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
			DatabaseTableAttribute DbTableAttr = typeof(T).GetCustomAttribute<DatabaseTableAttribute>();
			TableName = DbTableAttr.TableName;
		}

		static T CreateInstance<T>(SqlDataReader Reader) where T : DbTableEntry {

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

		public static T SelectByID<T>(int ID) where T : DbTableEntry {
			return UseDB<T>((Db) => {
				SqlCommand Cmd = CreateSelectCommand<T>(Db, new[] { new DbFilter("ID", ID) });

				using (SqlDataReader Reader = Cmd.ExecuteReader()) {
					if (Reader.Read()) {
						return CreateInstance<T>(Reader);
					}
				}

				throw new Exception();
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
		public int ID {
			get; set;
		}
	}

	[DatabaseTable("ParakeetUser")]
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

	[DatabaseTable("ContentID")]
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

		public int ContentType {
			get; set;
		}

		public string FileName {
			get; set;
		}
	}
}